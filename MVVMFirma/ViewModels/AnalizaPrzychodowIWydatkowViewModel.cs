using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LiveCharts;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;

namespace MVVMFirma.ViewModels
{
    public class AnalizaPrzychodowIWydatkowViewModel : JedenViewModel<object>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ChartValues<double> Revenues { get; private set; }
        public ChartValues<double> Expenses { get; private set; }
        public string[] Months { get; private set; }
        public ObservableCollection<DetailedRecord> DetailedRecords { get; private set; }

        public bool HasData => Revenues.Any() || Expenses.Any();

        public ICommand LoadDataCommand { get; }

        public AnalizaPrzychodowIWydatkowViewModel()
            : base("Analiza Przychodów i Wydatków")
        {
            StartDate = DateTime.Now.AddMonths(-6);
            EndDate = DateTime.Now;

            DetailedRecords = new ObservableCollection<DetailedRecord>();
            LoadDataCommand = new RelayCommand(LoadData);

            LoadData(); // Załaduj dane przy inicjalizacji
        }

        private void LoadData()
        {
            using (var db = new DesignOfficeEntities())
            {
                var revenueLogic = new RevenueLogic(db);
                var expenseLogic = new ExpenseLogic(db);

                // Pobranie danych z logiki biznesowej
                var revenueData = revenueLogic.GetRevenueByDateRange(StartDate, EndDate);
                var expenseData = expenseLogic.GetExpensesByDateRange(StartDate, EndDate);

                Revenues = new ChartValues<double>(
                    revenueData.Select(r => double.Parse(r.Value, System.Globalization.CultureInfo.InvariantCulture))
                );
                Expenses = new ChartValues<double>(
                    expenseData.Select(e => double.Parse(e.Value, System.Globalization.CultureInfo.InvariantCulture))
                );
                Months = revenueData.Select(r => $"{GetMonthName(r.Key)} {StartDate.Year}").ToArray();

                // Wypełnianie szczegółowych rekordów
                DetailedRecords.Clear();
                foreach (var record in revenueData.Select(r => new DetailedRecord
                         {
                             Date = $"{r.Key:00}.{StartDate.Year}",
                             Description = "Przychody",
                             Amount = double.Parse(r.Value, System.Globalization.CultureInfo.InvariantCulture)
                         }))
                {
                    DetailedRecords.Add(record);
                }

                foreach (var record in expenseData.Select(e => new DetailedRecord
                         {
                             Date = $"{e.Key:00}.{StartDate.Year}",
                             Description = "Wydatki",
                             Amount = double.Parse(e.Value, System.Globalization.CultureInfo.InvariantCulture)
                         }))
                {
                    DetailedRecords.Add(record);
                }
            }

            // Aktualizacja widoku
            OnPropertyChanged(() => Revenues);
            OnPropertyChanged(() => Expenses);
            OnPropertyChanged(() => Months);
            OnPropertyChanged(() => DetailedRecords);
            OnPropertyChanged(() => HasData);
        }

        private string GetMonthName(int month)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.DateTimeFormat.GetMonthName(month);
        }

        public override void Save() { }
    }

    public class DetailedRecord
    {
        public string Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
