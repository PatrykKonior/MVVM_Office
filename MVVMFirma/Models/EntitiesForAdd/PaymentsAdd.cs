using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class PaymentsAdd : DatabaseClass
    {
        #region Constructor
        public PaymentsAdd(DesignOfficeEntities db) : base(db) { }
        #endregion

        #region Functions
        public IQueryable<KeyAndValue> GetInvoicesKeyAndValueItems()
        {
            return db.Invoices.Select(i => new KeyAndValue
            {
                Key = i.InvoiceID,
                Value = i.InvoiceID + " | " + i.InvoiceDate.ToString() + " | " + i.TotalAmount.ToString()
            }).AsQueryable();
        }
        #endregion
    }
}
