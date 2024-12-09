using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class TimeLogsAdd : DatabaseClass
    {
        #region Constructor
        public TimeLogsAdd(DesignOfficeEntities db) : base(db) { }
        #endregion

        #region Functions
        public IQueryable<KeyAndValue> GetEmployeesKeyAndValueItems()
        {
            return db.Employees
                .Select(e => new KeyAndValue
                {
                    Key = e.EmployeeID,
                    Value = e.FirstName + " " + e.LastName + " (" + e.Position + ")" 
                });
        }

        public IQueryable<KeyAndValue> GetProjectsKeyAndValueItems()
        {
            return db.Projects
                .Select(p => new KeyAndValue
                {
                    Key = p.ProjectID,
                    Value = p.ProjectName + " (" + p.ProjectType + ")"
                });
        }
        #endregion
    }
}
