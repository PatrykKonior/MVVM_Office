using System;
using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.Models.BusinessLogic
{
    public class ProjectsLogicB : DatabaseClass
    {
        public ProjectsLogicB(DesignOfficeEntities db) : base(db) { }

        // Zwraca liczbę projektów w podziale na "Zakończony" i "W trakcie"
        public Dictionary<string, int> GetProjectStatusCounts()
        {
            var projects = db.Projects.ToList();
            if (!projects.Any())
            {
                Console.WriteLine("Brak projektów w bazie danych.");
                return new Dictionary<string, int>
                {
                    { "Zakończony", 0 },
                    { "W trakcie", 0 }
                };
            }

            return projects
                .GroupBy(p => p.ProjectStatus)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Zwraca projekty w kolejności: najbardziej opóźnione, zbliżające się, odległe terminy
        public List<ProjectsForAllView> GetProjectsOrderedByDeadline()
        {
            var projects = db.Projects
                .Where(p => p.ProjectStatus == "W trakcie" && p.ProjectEndDate.HasValue)
                .ToList();

            if (!projects.Any())
            {
                Console.WriteLine("Brak projektów w trakcie realizacji lub bez daty zakończenia.");
                return new List<ProjectsForAllView>();
            }

            return projects
                .OrderByDescending(p => p.ProjectEndDate.Value < DateTime.UtcNow)  // Opóźnione najpierw
                .ThenBy(p => p.ProjectEndDate.Value)                               // Następnie zbliżające się
                .Select(p => new ProjectsForAllView
                {
                    ProjectID = p.ProjectID,
                    ProjectName = p.ProjectName,
                    ProjectType = p.ProjectType,
                    ProjectEndDate = p.ProjectEndDate.Value,
                    ProjectStartDate = p.ProjectStartDate,
                    ProjectStatus = p.ProjectStatus,
                    ClientsCompanyName = p.Clients?.CompanyName ?? "Brak klienta",
                    EmployeesFirstName = p.Employees?.FirstName ?? "Nieprzypisany",
                    EmployeesLastName = p.Employees?.LastName ?? string.Empty,
                    ProjectBudget = p.ProjectBudget,
                    VATRate = p.VATRate
                })
                .ToList();
        }
    }
}
