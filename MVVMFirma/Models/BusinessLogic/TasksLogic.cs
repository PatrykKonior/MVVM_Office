using System;
using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.Models.BusinessLogic
{
    public class TasksLogic : DatabaseClass
    {
        public TasksLogic(DesignOfficeEntities db) : base(db) { }

        // Zwraca liczbę zadań w podziale na "Zakończono" i "W trakcie"
        public Dictionary<string, int> GetTaskStatusCounts()
        {
            var tasks = db.Tasks.ToList();
            if (!tasks.Any())
            {
                Console.WriteLine("Brak zadań w bazie danych.");
                return new Dictionary<string, int>
                {
                    { "Zakończono", 0 },
                    { "W trakcie", 0 }
                };
            }

            return tasks
                .GroupBy(t => t.TaskStatus)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Zwraca zadania w kolejności: najbardziej opóźnione, zbliżające się, odległe terminy
        public List<ZadaniaForAllView> GetTasksOrderedByDeadline()
        {
            var tasks = db.Tasks
                .Where(t => t.TaskStatus == "W trakcie" && t.TaskEndDate.HasValue)
                .ToList();

            if (!tasks.Any())
            {
                Console.WriteLine("Brak zadań w trakcie realizacji lub bez daty zakończenia.");
                return new List<ZadaniaForAllView>();
            }

            return tasks
                .OrderByDescending(t => t.TaskEndDate.Value < DateTime.UtcNow)  // Opóźnione najpierw
                .ThenBy(t => t.TaskEndDate.Value)                               // Następnie zbliżające się
                .Select(t => new ZadaniaForAllView
                {
                    TaskID = t.TaskID,
                    ProjectName = t.Projects?.ProjectName ?? "Brak projektu",
                    ProjectType = t.Projects?.ProjectType ?? "Brak typu projektu",
                    TaskName = t.TaskName,
                    TaskDescription = t.TaskDescription,
                    TaskStartDate = t.TaskStartDate,
                    TaskEndDate = t.TaskEndDate.Value,
                    TaskStatus = t.TaskStatus,
                    EmployeesFirstName = t.Employees?.FirstName ?? "Nieprzypisany",
                    EmployeesLastName = t.Employees?.LastName ?? string.Empty,
                    EstimatedHours = t.EstimatedHours
                })
                .ToList();
        }
    }
}
