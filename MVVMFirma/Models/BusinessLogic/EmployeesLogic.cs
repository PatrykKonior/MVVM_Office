using System;
using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.Models.BusinessLogic
{
    public class EmployeesLogic : DatabaseClass
    {
        public EmployeesLogic(DesignOfficeEntities db) : base(db) { }

        // Zwraca liczbę projektów przypisanych do pracownika w danym miesiącu
        public List<KeyAndValue> GetProjectsCountByEmployee(int month, int year)
        {
            var rawData = db.ProjectAssignments
                .Where(pa => pa.AssignmentDate.HasValue &&
                             pa.AssignmentDate.Value.Month == month &&
                             pa.AssignmentDate.Value.Year == year)
                .GroupBy(pa => pa.EmployeeID)
                .Select(g => new
                {
                    EmployeeID = g.Key,
                    ProjectsCount = g.Count(),
                    ProjectNames = g.Select(pa => pa.Projects.ProjectName).ToList()
                })
                .ToList();

            if (!rawData.Any())
            {
                Console.WriteLine("Brak danych projektów przypisanych pracownikom dla podanego miesiąca i roku.");
                return new List<KeyAndValue>();
            }

            return rawData.Select(r => new KeyAndValue
            {
                Key = r.EmployeeID ?? -1,
                Value = $"{r.ProjectsCount}"
            }).ToList();
        }
    }
}