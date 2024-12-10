using System;
using System.Collections.ObjectModel;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForAdd;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class NowyDzialViewModel : JedenViewModel<Departments>
    {
        #region Constructor
        public NowyDzialViewModel() : base("Nowy Dział")
        {
            item = new Departments();
            LoadData();
        }
        #endregion

        #region Properties
        public string DepartmentName
        {
            get => item.DepartmentName;
            set
            {
                item.DepartmentName = value;
                OnPropertyChanged(() => DepartmentName);
            }
        }

        public int? ManagerID
        {
            get => item.ManagerID;
            set
            {
                item.ManagerID = value;
                OnPropertyChanged(() => ManagerID);
                OnPropertyChanged(() => SelectedManager);
            }
        }

        public KeyAndValue SelectedManager
        {
            get => ManagersItems.FirstOrDefault(m => m.Key == ManagerID);
            set
            {
                ManagerID = value?.Key;
                OnPropertyChanged(() => SelectedManager);
            }
        }

        public ObservableCollection<KeyAndValue> ManagersItems { get; set; }
        #endregion

        #region Helpers
        private void LoadData()
        {
            var rawManagers = new EmployeesAdd(designOfficeEntities).GetEmployeesKeyAndValueItems().ToList();
            ManagersItems = new ObservableCollection<KeyAndValue>(rawManagers);
        }

        public override void Save()
        {
            designOfficeEntities.Departments.Add(item);
            designOfficeEntities.SaveChanges();
        }
        #endregion
    }
}
