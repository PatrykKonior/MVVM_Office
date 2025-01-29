using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace MVVMFirma.ViewModels
{
    public class NowyWydatekViewModel : JedenViewModel<Expenses>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        #region Modal
        
        public string SelectedProjectFullName
        {
            get
            {
                var project = ProjectsItems.FirstOrDefault(p => p.Key == ProjectID);
                return project != null ? $"{project.Value}" : "Nie wybrano projektu";
            }
        }

        public ICommand ShowAllProjectsCommand { get; }

        #endregion

        #region Constructor

        public NowyWydatekViewModel() : base("Nowy Wydatek")
        {
            item = new Expenses();
            LoadData();
            ExpenseDate = DateTime.Now;
            
            // Obsługa wyboru projektu
            ShowAllProjectsCommand = new RelayCommand(ShowAllProjects);
            Messenger.Default.Register<KeyAndValue>(this, "SelectedProject", GetSelectedProject);
        }

        #endregion

        #region Validators
        public bool HasErrors => _validationErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationErrors.ContainsKey(propertyName) ? _validationErrors[propertyName] : null;
        }

        private void ValidateProperty(string propertyName)
        {
            List<string> errors = new List<string>();

            switch (propertyName)
            {
                case nameof(ProjectID):
                    errors.AddRange(StringValidator.ValidateRequired(ProjectID?.ToString(), "Projekt"));
                    break;
                case nameof(ExpenseDate):
                    errors.AddRange(DateValidator.ValidateNotInFuture(ExpenseDate, "Data Wydatku"));
                    break;
                case nameof(ExpenseDescription):
                    errors.AddRange(StringValidator.ValidateRequired(ExpenseDescription, "Opis Wydatku"));
                    errors.AddRange(StringValidator.ValidateMaxLength(ExpenseDescription, 255, "Opis Wydatku"));
                    break;
                case nameof(NetAmount):
                    errors.AddRange(DecimalValidator.ValidateGreaterThanZero(NetAmount, "Kwota Netto"));
                    break;
                case nameof(VATAmount):
                    errors.AddRange(DecimalValidator.ValidateNotNegative(VATAmount, "Kwota VAT"));
                    errors.AddRange(DecimalValidator.ValidateNotGreaterThan(VATAmount, NetAmount, "Kwota VAT", "Kwota Netto"));
                    break;
                case nameof(GrossAmount):
                    if (GrossAmount != NetAmount + VATAmount)
                        errors.Add("Kwota Brutto powinna być sumą Kwoty Netto i Kwoty VAT.");
                    break;
            }

            if (errors.Any())
                _validationErrors[propertyName] = errors;
            else
                _validationErrors.Remove(propertyName);

            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties
        
        private void ShowAllProjects()
        {
            Messenger.Default.Send("ProjektyAll");
        }

        private void GetSelectedProject(KeyAndValue selectedProject)
        {
            if (selectedProject != null)
            {
                ProjectID = selectedProject.Key;
                OnPropertyChanged(() => SelectedProjectFullName);
            }
        }

        public int? ProjectID
        {
            get => item.ProjectID;
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectID);
                OnPropertyChanged(() => SelectedProject);
                ValidateProperty(nameof(ProjectID));
            }
        }

        public KeyAndValue SelectedProject
        {
            get => ProjectsItems.FirstOrDefault(p => p.Key == ProjectID);
            set
            {
                ProjectID = value?.Key;
                OnPropertyChanged(() => SelectedProject);
            }
        }

        public DateTime? ExpenseDate
        {
            get => item.ExpenseDate;
            set
            {
                item.ExpenseDate = value;
                OnPropertyChanged(() => ExpenseDate);
                ValidateProperty(nameof(ExpenseDate));
            }
        }

        public string ExpenseDescription
        {
            get => item.ExpenseDescription;
            set
            {
                item.ExpenseDescription = value;
                OnPropertyChanged(() => ExpenseDescription);
                ValidateProperty(nameof(ExpenseDescription));
            }
        }

        public decimal? NetAmount
        {
            get => item.NetAmount ?? 0;
            set
            {
                item.NetAmount = value;
                OnPropertyChanged(() => NetAmount);
                OnPropertyChanged(() => GrossAmount);
                ValidateProperty(nameof(NetAmount));
                ValidateProperty(nameof(GrossAmount));
            }
        }

        public decimal? VATAmount
        {
            get => item.VATAmount ?? 0;
            set
            {
                item.VATAmount = value;
                OnPropertyChanged(() => VATAmount);
                OnPropertyChanged(() => GrossAmount);
                ValidateProperty(nameof(VATAmount));
                ValidateProperty(nameof(GrossAmount));
            }
        }

        public decimal? GrossAmount
        {
            get => item.GrossAmount ?? (NetAmount + VATAmount);
            set
            {
                item.GrossAmount = value;
                OnPropertyChanged(() => GrossAmount);
                ValidateProperty(nameof(GrossAmount));
            }
        }

        public ObservableCollection<KeyAndValue> ProjectsItems { get; set; }

        #endregion

        #region Helpers

        private void LoadData()
        {
            // Pobierz dane z bazy jako lista
            var rawProjects = new ExpensesAdd(designOfficeEntities).GetProjectsKeyAndValueItems().ToList();

            // Formatowanie danych w pamięci
            ProjectsItems = new ObservableCollection<KeyAndValue>(
                rawProjects.Select(p => new KeyAndValue
                {
                    Key = p.Key,
                    Value = $"{p.Value}" 
                })
            );
        }

        public override void Save()
        {
            if (HasErrors)
            {
                // Wyświetl komunikat w interfejsie użytkownika
                MessageBox.Show("Nie można zapisać, ponieważ istnieją błędy walidacji.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Upewnij się, że kwoty są obliczone przed zapisaniem
            item.GrossAmount = NetAmount + VATAmount;

            designOfficeEntities.Expenses.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
