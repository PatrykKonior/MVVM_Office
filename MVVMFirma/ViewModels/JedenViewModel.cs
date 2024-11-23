using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;


namespace MVVMFirma.ViewModels
{
    public abstract class JedenViewModel<T> : WorkspaceViewModel
    {
        #region DB

        protected DesignOfficeEntities designOfficeEntities;

        #endregion

        #region Item

        protected T item;

        #endregion
        #region Command

        // to jest koenda, która zostanie podpięta pod przycisk zapisz i zamknij i ona wywoła funkcje SaveAndClose
        private BaseCommand _SaveCommand;

        public ICommand SaveCommand

        {
            get
            {
                if (_SaveCommand == null)
                    _SaveCommand = new BaseCommand(() => SaveAndClose());
                return _SaveCommand;
            }
        }

        #endregion
        #region Constructor

        public JedenViewModel(string displayName)
        {
            base.DisplayName = displayName;
            designOfficeEntities = new DesignOfficeEntities();
        }

        #endregion

        #region Helpers

        public abstract void Save();
        public void SaveAndClose()
        {
            Save();
            base.OnRequestClose(); //zamknięcie zakładki
        }

        #endregion
    }
}