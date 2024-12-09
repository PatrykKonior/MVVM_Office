using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowyWydatekViewModel : JedenViewModel<Expenses>
    {
        #region Constructor

        public NowyWydatekViewModel() : base("Nowy Wydatek")
        {
            item = new Expenses();
            LoadData();
            ExpenseDate = DateTime.Now; 
        }

        #endregion

        #region Properties

        public int? ProjectID
        {
            get => item.ProjectID;
            set
            {
                item.ProjectID = value;
                OnPropertyChanged(() => ProjectID);
                OnPropertyChanged(() => SelectedProject);
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
            }
        }

        public string ExpenseDescription
        {
            get => item.ExpenseDescription;
            set
            {
                item.ExpenseDescription = value;
                OnPropertyChanged(() => ExpenseDescription);
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
            }
        }

        public decimal? GrossAmount
        {
            get => item.GrossAmount ?? (NetAmount + VATAmount);
            set
            {
                item.GrossAmount = value;
                OnPropertyChanged(() => GrossAmount);
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
            // Upewnij się, że kwoty są obliczone przed zapisaniem
            item.GrossAmount = NetAmount + VATAmount;

            designOfficeEntities.Expenses.Add(item);
            designOfficeEntities.SaveChanges();
        }

        #endregion
    }
}
