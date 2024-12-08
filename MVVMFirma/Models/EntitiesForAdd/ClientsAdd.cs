using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class ClientsAdd:DatabaseClass
    {
        #region Constructor
            public ClientsAdd(DesignOfficeEntities db)
            : base(db)
        {
        }
        #endregion
        #region Function
        public IQueryable<KeyAndValue> GetClientsKeyAndValueItems()
        { 
            return (
                from client in db.Clients
                select new KeyAndValue
                {
                    Key = client.ClientID,
                    Value = client.CompanyName
                }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
