using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Helper;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

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
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Towary",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieTowaryViewModel()))),

                new CommandViewModel(
                    "Nowy Towar",
                    new BaseCommand(() => this.CreateView(new NowyTowarViewModel()))),

                new CommandViewModel(
                    "Lista Faktur",
                    new BaseCommand(() => this.CreateShowAll(new WszystkieFakturyViewModel()))),

                new CommandViewModel(
                    "Dodaj Nową Fakturę",
                    new BaseCommand(() => this.CreateView(new NowaFakturaViewModel()))),

                new CommandViewModel(
                    "Lista Klientów",
                    new BaseCommand(() => this.CreateShowAll(new WszyscyKlienciViewModel()))),
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
        #endregion
    }
}
