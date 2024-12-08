using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMFirma.Models.Entities;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class DatabaseClass
    {
        #region DB
        protected DesignOfficeEntities db { get; set; }
        #endregion
        #region Constructor
        public DatabaseClass(DesignOfficeEntities db)
        {
            this.db = db;
        }
        #endregion
    }
}
