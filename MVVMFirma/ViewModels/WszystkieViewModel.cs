using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Linq.Expressions;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using MVVMFirma.ViewModels;


namespace MVVMFirma.ViewModels
{
    public abstract class WszystkieViewModel<T> : WorkspaceViewModel // pod T będą podstawione konkretne typy elementów listy
    {
        #region DataBase
        protected readonly DesignOfficeEntities designOfficeEntities; // to jest pole, które reprezentuje bazę 2danych
        #endregion

        #region LoadCommand
        private BaseCommand _LoadCommand; // to jest komenda, która będzie wywoływać metodę Load pokazującą towary
        #endregion

        #region Properties
        public ICommand LoadCommand // to jest właściwość, która udostępnia komendę ładującą towary
        {
            get
            {
                if (_LoadCommand == null) _LoadCommand = new BaseCommand(() => Load());
                return _LoadCommand;
            }
        }
        #endregion

        #region List
        private ObservableCollection<T> _List; // to jest kolekcja, która przechowuje towary z bazy danych
        public ObservableCollection<T> List // to jest właściwość, która udostępnia kolekcję z towarami
        {
            get
            {
                if (_List == null) Load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List); // ta metoda powiadamia widok, że właściwość List uległa zmianie
            }
        }
        #endregion

        #region Constructor

        public WszystkieViewModel(String displayName)
        {
            designOfficeEntities = new DesignOfficeEntities();
            base.DisplayName = displayName;
        }
        #endregion

        #region Helpers
        public abstract void Load(); // to jest metoda, która pobiera towary z bazy danych
        #endregion
    }
}