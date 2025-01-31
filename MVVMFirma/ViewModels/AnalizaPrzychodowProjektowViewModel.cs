﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LiveCharts;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using LiveCharts.Defaults;

namespace MVVMFirma.ViewModels
{
    public class AnalizaPrzychodowProjektowViewModel : JedenViewModel<object>
    {
        // Wybrane miesiąc i rok
        private int _selectedMonth;
        public int SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged(() => SelectedMonth);
                Console.WriteLine($"SelectedMonth updated: {SelectedMonth}");
            }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(() => SelectedYear);
                Console.WriteLine($"SelectedYear updated: {SelectedYear}");
            }
        }
        
        // Źródła dla ComboBox
        public ObservableCollection<int> Months { get; set; }
        public ObservableCollection<int> Years { get; set; }
        
        // Dane do wykresów
        public ChartValues<double> ProjectRevenues { get; private set; }
        public string[] ProjectNames { get; private set; }
        public string[] ProjectTooltips { get; private set; }

        public ChartValues<int> EmployeeProjectCounts { get; private set; }
        public string[] EmployeeNames { get; private set; }

        // Podsumowanie danych
        public string SummaryText { get; private set; }

        // Widoczność danych
        public bool HasData =>
            (ProjectRevenues != null && ProjectRevenues.Any()) ||
            (EmployeeProjectCounts != null && EmployeeProjectCounts.Any());

        // Komenda do załadowania danych
        public ICommand LoadDataCommand { get; }

        public ObservableCollection<ProjectsForAllView> ProjectDetails { get; private set; }
        
        public Func<ChartPoint, string> PieLabelFormatter { get; private set; }

        public AnalizaPrzychodowProjektowViewModel()
            : base("Analiza Przychodów wg Projektów")
        {
            // Inicjalizacja miesięcy i lat
            Months = new ObservableCollection<int>(Enumerable.Range(1, 12));
            Years = new ObservableCollection<int>(new[] { 2022, 2023, 2024, 2025 });
            
            SelectedMonth = DateTime.Now.Month;
            SelectedYear = DateTime.Now.Year;

            ProjectRevenues = new ChartValues<double>();
            EmployeeProjectCounts = new ChartValues<int>();
            ProjectDetails = new ObservableCollection<ProjectsForAllView>();
            
            PieLabelFormatter = point =>
            {
                var index = (int)point.X;
                if (index >= 0 && index < ProjectTooltips.Length)
                {
                    return ProjectTooltips[index];
                }
                return "Nieznany projekt";
            };
            
            LoadDataCommand = new RelayCommand(LoadData);

            LoadData(); // Wstępne ładowanie danych
        }

        private void LoadData()
        {
            using (var db = new DesignOfficeEntities())
            {
                var projectsLogic = new ProjectsLogic(db);
                var employeesLogic = new EmployeesLogic(db);

                // Pobierz dane dla wykresu kołowego (przychody projektów)
                var projectData = projectsLogic.GetProjectRevenuesByMonth(SelectedMonth, SelectedYear);
                var totalRevenue = projectData.Sum(p => double.Parse(p.Value, System.Globalization.CultureInfo.InvariantCulture));

                // Ustaw dane dla przychodów
                ProjectRevenues = new ChartValues<double>(
                    projectData.Select(p => double.Parse(p.Value, System.Globalization.CultureInfo.InvariantCulture))
                );
                
                // Nazwy projektów — konwersja klucza na string
                ProjectNames = projectData.Select(p => $"Projekt {p.Key}").ToArray();

                // Tooltipy dla wykresu kołowego
                ProjectTooltips = projectData.Select(p =>
                {
                    var projectName = $"Projekt {p.Key}";
                    var revenue = double.Parse(p.Value, System.Globalization.CultureInfo.InvariantCulture);
                    var percentage = totalRevenue > 0 ? (revenue / totalRevenue) * 100 : 0;
                    return $"{projectName}\nKwota: {revenue:F2} PLN";
                }).ToArray();
                
                // Pobierz dane dla wykresu kolumnowego (liczba projektów przypisanych pracownikom)
                var employeeData = employeesLogic.GetProjectsCountByEmployee(SelectedMonth, SelectedYear);
                EmployeeProjectCounts = new ChartValues<int>(
                    employeeData.Select(e => int.Parse(e.Value))
                );
                EmployeeNames = employeeData.Select(e =>
                    $"{e.Key}: {db.Employees.Where(emp => emp.EmployeeID == e.Key).Select(emp => emp.FirstName + " " + emp.LastName).FirstOrDefault() ?? "Nieznany"}"
                ).ToArray();

                // Pobierz szczegółowe dane projektów
                var projectDetails = projectsLogic.GetProjectDetailsByMonth(SelectedMonth, SelectedYear);
                ProjectDetails.Clear();
                foreach (var detail in projectDetails)
                {
                    ProjectDetails.Add(detail);
                }

                // Generowanie podsumowania
                SummaryText = GenerateSummary(projectData, employeeData);
            }

            // Aktualizacja widoku
            OnPropertyChanged(() => ProjectRevenues);
            OnPropertyChanged(() => ProjectNames);
            OnPropertyChanged(() => ProjectTooltips);
            OnPropertyChanged(() => EmployeeProjectCounts);
            OnPropertyChanged(() => EmployeeNames);
            OnPropertyChanged(() => ProjectDetails);
            OnPropertyChanged(() => HasData);
            OnPropertyChanged(() => SummaryText);
        }

        private string GenerateSummary(IEnumerable<KeyAndValue> projectData, IEnumerable<KeyAndValue> employeeData)
        {
            if (!projectData.Any() && !employeeData.Any())
                return "Brak danych dla wybranego miesiąca i roku.";

            var totalRevenue = projectData.Sum(p => double.Parse(p.Value.Split(':').Last().Replace("zł", "").Trim(), System.Globalization.CultureInfo.InvariantCulture));
            var totalProjects = employeeData.Sum(e => int.Parse(e.Value.Split(' ').First()));

            return $"Podsumowanie:\n" +
                   $"- Łączny dochód z projektów: {totalRevenue:C2}\n" +
                   $"- Łączna liczba projektów w miesiącu: {totalProjects}\n" +
                   $"- Liczba pracowników zaangażowanych w projekty: {employeeData.Count()}";
        }

        // Wymagana implementacja abstrakcyjnej metody Save
        public override void Save()
        {
            // Nie wymaga implementacji w tym widoku biznesowym
            Console.WriteLine("Metoda Save nie jest używana w tym widoku biznesowym.");
        }
    }
}
