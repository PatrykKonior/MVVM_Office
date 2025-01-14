﻿using System;
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
using GalaSoft.MvvmLight.Messaging;


namespace MVVMFirma.ViewModels
{
    public abstract class WszystkieViewModel<T> : WorkspaceViewModel // pod T będą podstawione konkretne typy elementów listy
    {
        #region DataBase
        protected readonly DesignOfficeEntities designOfficeEntities; // to jest pole, które reprezentuje bazę 2danych
        #endregion

        #region Command
        private BaseCommand _LoadCommand; // to jest komenda, która będzie wywoływać metodę Load pokazującą towary
        
        public ICommand LoadCommand // to jest właściwość, która udostępnia komendę ładującą towary
        {
            get
            {
                if (_LoadCommand == null) _LoadCommand = new BaseCommand(() => Load());
                return _LoadCommand;
            }
        }
        private BaseCommand _AddCommand; // to jest komenda, która będzie wywoływać metodę Add wywołująca okno do dodawania i zostanie podpieta pod przycisk dodaj

        public ICommand AddCommand // to jest właściwość, która udostępnia komendę ładującą towary
        {
            get
            {
                if (_AddCommand == null) _AddCommand = new BaseCommand(() => Add());
                return _AddCommand;
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
        #region Sort and Filtr
        // do sortowania
        public string SortField { get; set; }
        public List<string> SortComboboxItems 
        { 
            get
            {
                return GetComboboxSortList();
            }
        }
        public abstract List<string> GetComboboxSortList();
        private BaseCommand _SortCommand;
        public ICommand SortCommand
        {
            get
            {
                if (_SortCommand == null)
                    _SortCommand = new BaseCommand(() => Sort());
                return _SortCommand;
            }
        }
        public abstract void Sort();
        // do filtrowania
        public string FindField { get; set; }
        public List<string> FindComboboxItems
        {
            get
            {
                return GetComboboxFindList();
            }
        }
        public abstract List<string> GetComboboxFindList();
        public string FindTextBox { get; set; }
        private BaseCommand _FindCommand;
        public ICommand FindCommand
        {
            get
            {
                if (_FindCommand == null)
                    _FindCommand = new BaseCommand(() => Find());
                return _FindCommand;
            }
        }
        public abstract void Find();
        #endregion

        #region Helpers
        public abstract void Load(); // to jest metoda, która pobiera towary z bazy danych
        public void Add()
        {
            Messenger.Default.Send(DisplayName + "Add");
        }
        #endregion
    }
}