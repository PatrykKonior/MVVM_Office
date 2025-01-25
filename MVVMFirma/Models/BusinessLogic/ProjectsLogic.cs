using System;
using System.Collections.Generic;
using System.Linq;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.Models.BusinessLogic
{
    public class ProjectsLogic : DatabaseClass
{
    public ProjectsLogic(DesignOfficeEntities db) : base(db) { }

    // Zwraca przychody projektów dla danego miesiąca i roku
    public List<KeyAndValue> GetProjectRevenuesByMonth(int month, int year)
    {
        // Log wszystkich projektów w bazie
        var allProjects = db.Projects.ToList();
        Console.WriteLine($"Wszystkie projekty w bazie: {allProjects.Count}");

        foreach (var project in allProjects)
        {
            Console.WriteLine($"Projekt: {project.ProjectName}, Data rozpoczęcia: {project.ProjectStartDate}, Budżet: {project.ProjectBudget}");
        }
        
        var rawData = db.Projects
            .Where(p => p.ProjectStartDate.HasValue &&
                        p.ProjectStartDate.Value.Month == month &&
                        p.ProjectStartDate.Value.Year == year)
            .Select(p => new
            {
                ProjectID = p.ProjectID,
                ProjectName = p.ProjectName,
                Revenue = p.ProjectBudget ?? 0
            })
            .ToList();
        
        Console.WriteLine($"Projekty dla miesiąca {month} i roku {year}: {rawData.Count}");
        foreach (var project in rawData)
        {
            Console.WriteLine($"Projekt: {project.ProjectName}, Budżet: {project.Revenue}");
        }

        if (!rawData.Any())
        {
            Console.WriteLine("Brak danych projektów dla podanego miesiąca i roku.");
            return new List<KeyAndValue>();
        }

        return rawData.Select(r => new KeyAndValue
        {
            Key = r.ProjectID,
            Value = $"{r.Revenue:F2}"
        }).ToList();
    }

    // Zwraca szczegóły projektów dla danego miesiąca i roku
    public List<ProjectsForAllView> GetProjectDetailsByMonth(int month, int year)
    {
        // Log wszystkich projektów w bazie
        var allProjects = db.Projects.ToList();
        Console.WriteLine($"Wszystkie projekty w bazie: {allProjects.Count}");

        foreach (var project in allProjects)
        {
            Console.WriteLine($"Projekt: {project.ProjectName}, Data rozpoczęcia: {project.ProjectStartDate}, Budżet: {project.ProjectBudget}");
        }

        // Filtrujemy projekty na podstawie miesiąca i roku
        var filteredProjects = db.Projects
            .Where(p => p.ProjectStartDate.HasValue &&
                        p.ProjectStartDate.Value.Month == month &&
                        p.ProjectStartDate.Value.Year == year)
            .ToList();

        Console.WriteLine($"Projekty dla miesiąca {month} i roku {year}: {filteredProjects.Count}");
        foreach (var project in filteredProjects)
        {
            Console.WriteLine($"Projekt: {project.ProjectName}, Data rozpoczęcia: {project.ProjectStartDate}, Budżet: {project.ProjectBudget}");
        }

        return filteredProjects
            .Select(p => new ProjectsForAllView
            {
                ProjectID = p.ProjectID,
                ClientsCompanyName = p.Clients != null ? p.Clients.CompanyName : "Brak klienta",
                ProjectName = p.ProjectName,
                ProjectType = p.ProjectType,
                ProjectStartDate = p.ProjectStartDate,
                ProjectEndDate = p.ProjectEndDate,
                ProjectBudget = p.ProjectBudget,
                VATRate = p.VATRate,
                ProjectStatus = p.ProjectStatus,
                EmployeesFirstName = p.Employees != null ? p.Employees.FirstName : "Brak danych",
                EmployeesLastName = p.Employees != null ? p.Employees.LastName : "Brak danych"
            })
            .ToList();
    }
}
}
