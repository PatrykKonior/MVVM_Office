using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class SalesAdd:DatabaseClass
    {
        #region Constructor
        public SalesAdd(DesignOfficeEntities db)
        : base(db)
        {
        }
        #endregion
        #region Function
        public IQueryable<KeyAndValue> GetSalesKeyAndValueItems()
        {
            return (
                from sale in db.Sales
                select new KeyAndValue
                {
                    Key = sale.SaleID,
                    Value = sale.SaleDate.ToString()
                }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
