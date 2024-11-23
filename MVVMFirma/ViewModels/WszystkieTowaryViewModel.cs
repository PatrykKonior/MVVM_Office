using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTowaryViewModel : WorkspaceViewModel
    {
        #region DB
        private readonly DesignOfficeEntities designOfficeEntities;

        
        #endregion
        #region LoadCommand
        private BaseCommand _LoadCommand;
        #endregion
        #region Properties
        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                {
                    _LoadCommand = new BaseCommand(() => load());
                }
                return _LoadCommand;
            }
        }
        #endregion
        #region List
        private ObservableCollection<Materials> _List;
        public ObservableCollection<Materials> List
        {
            get
            {
                if (_List == null)
                    load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }
        #endregion
        public WszystkieTowaryViewModel()
        {
            base.DisplayName = "Towary";
            designOfficeEntities = new DesignOfficeEntities();

        }
        #region Helpers
        private void load()
        {
            List = new ObservableCollection<Materials>
            (
                designOfficeEntities.Materials.ToList()
            );
        }
        #endregion
    }


}