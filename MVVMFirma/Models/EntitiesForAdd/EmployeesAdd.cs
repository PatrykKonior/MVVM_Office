using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class EmployeesAdd : DatabaseClass
    {
        #region Constructor
        public EmployeesAdd(DesignOfficeEntities db) : base(db)
        {
        }
        #endregion

        #region Functions
        public IQueryable<KeyAndValue> GetEmployeesKeyAndValueItems()
        {
            return (
                from employee in db.Employees
                select new KeyAndValue
                {
                    Key = employee.EmployeeID,
                    Value = employee.FirstName + " " + employee.LastName
                }
            ).ToList().AsQueryable();
        }
        #endregion
    }
}
