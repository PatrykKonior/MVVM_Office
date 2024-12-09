using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class ExpensesAdd : DatabaseClass
    {
        #region Constructor
        public ExpensesAdd(DesignOfficeEntities db) : base(db) { }
        #endregion

        #region Functions
        public IQueryable<KeyAndValue> GetProjectsKeyAndValueItems()
        {
            return db.Projects.Select(p => new KeyAndValue
            {
                Key = p.ProjectID,
                Value = p.ProjectName + " (" + p.ProjectType + ")" 
            });
        }
        #endregion
    }
}
