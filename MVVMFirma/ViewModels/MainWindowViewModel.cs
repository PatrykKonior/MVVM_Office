﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Helper;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace MVVMFirma.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        #endregion

        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }
        private List<CommandViewModel> CreateCommands()
        {
            Messenger.Default.Register<string>(this, open);
            return new List<CommandViewModel>
            {

                new CommandViewModel(
                    "Lista Projektów",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieProjektyViewModel()))),

                new CommandViewModel(
                    "Przydział projektów",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieProjectAssignmentsViewModel()))),

                new CommandViewModel(
                    "Dodaj Nowy Kontrakt",
                    new BaseCommand(() => this.CreateView(new NowyKontraktViewModel()))),

                new CommandViewModel(
                    "Lista Kontraktów",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieKontraktyViewModel()))),

                new CommandViewModel(
                    "Lista Zadań",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieZadaniaViewModel()))),

                new CommandViewModel(
                    "Towary",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieTowaryViewModel()))),

                new CommandViewModel(
                    "Nowy Towar",
                    new BaseCommand(() => this.CreateView(new NowyTowarViewModel()))),

                new CommandViewModel(
                    "Materiały w projektach",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieProjectMaterialsViewModel()))),

                new CommandViewModel(
                    "Lista Faktur",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieFakturyViewModel()))),

                new CommandViewModel(
                    "Dodaj Nową Fakturę",
                    new BaseCommand(() => this.CreateView(new NowaFakturaViewModel()))),

                new CommandViewModel(
                    "Wykaz Sprzedaży",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieSprzedazeViewModel()))),

                new CommandViewModel(
                    "Wykaz Sprzedanych produktów",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieSaleItemsViewModel()))),

                new CommandViewModel(
                    "Dodaj Wydatek",
                    new BaseCommand(() => this.CreateView(new NowyWydatekViewModel()))),

                new CommandViewModel(
                    "Lista Wydatków",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieWydatkiViewModel()))),

                new CommandViewModel(
                    "Dodaj Płatność",
                    new BaseCommand(() => this.CreateView(new NowaPlatnoscViewModel()))),

                new CommandViewModel(
                    "Lista Płatności",
                    new BaseCommand(() => this.CreateShowAll(new WszystkiePlatnosciViewModel()))),

                new CommandViewModel(
                    "Lista Klientów",
                    new BaseCommand(() => this.CreateShowAll(new WszyscyKlienciViewModel()))),

                new CommandViewModel(
                    "Dodaj Nowego Klienta",
                    new BaseCommand(() => this.CreateView(new NowyKlientViewModel()))),

                new CommandViewModel(
                    "Lista Pracowników",
                    new BaseCommand(() => this.CreateShowAll(new WszyscyPracownicyViewModel()))),

                new CommandViewModel(
                    "Dodaj Nowego Pracownika",
                    new BaseCommand(() => this.CreateView(new NowyPracownikViewModel()))),

                new CommandViewModel(
                    "Działy w firmie",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieDzialyViewModel()))),

                new CommandViewModel(
                    "Dodaj Czas Pracy",
                    new BaseCommand(() => this.CreateView(new NowyCzasPracyViewModel()))),

                new CommandViewModel(
                    "Czas Pracy",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieCzasyPracyViewModel()))),
            };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            //workspace.Dispos();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers
        private void CreateView(WorkspaceViewModel nowy)
        {
            NowyTowarViewModel workspace = new NowyTowarViewModel();
            this.Workspaces.Add(nowy);
            this.SetActiveWorkspace(nowy);
        }

        private void CreateShowAll(WorkspaceViewModel model)
        {
            var workspace = this.Workspaces.FirstOrDefault(vm => vm.GetType() == model.GetType());
            if (workspace == null)
            {
                this.Workspaces.Add(model);
                workspace = model;
            }

            this.SetActiveWorkspace(workspace);
        }
        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        private void open(string name)
        {
            if (name == "TowaryAdd")
                CreateView(new NowyTowarViewModel());
            if (name == "FakturyAdd")
                CreateView(new NowaFakturaViewModel());
            if (name == "KlienciAdd")
                CreateView(new NowyKlientViewModel());
            if (name == "PracownicyAdd")
                CreateView(new NowyPracownikViewModel());
            if (name == "Czasy PracyAdd")
                CreateView(new NowyCzasPracyViewModel());
            if (name == "WydatkiAdd")
                CreateView(new NowyWydatekViewModel());
            if (name == "KontraktyAdd")
                CreateView(new NowyKontraktViewModel());
            if (name == "Lista płatnościAdd")
                CreateView(new NowaPlatnoscViewModel());

        }
        #endregion
    }
}