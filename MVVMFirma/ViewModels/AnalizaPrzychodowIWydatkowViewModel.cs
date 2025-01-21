using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using MVVMFirma.Models.Entities;

namespace MVVMFirma.ViewModels
{
    public class AnalizaPrzychodowIWydatkowViewModel : JedenViewModel<object>
    {
        #region Properties

        public ChartValues<double> Revenues { get; set; }
        public ChartValues<double> Expenses { get; set; }
        public string[] Months { get; set; }
        public ChartValues<double> ClientARevenue { get; set; }
        public ChartValues<double> ClientBRevenue { get; set; }
        public ChartValues<double> OtherClientsRevenue { get; set; }
        public ObservableCollection<DetailedRecord> DetailedRecords { get; set; }

        #endregion

        #region Constructor

        public AnalizaPrzychodowIWydatkowViewModel()
            : base("Analiza Przychodów i Wydatków")
        {
            InitializeMockData();
        }

        #endregion

        #region Methods

        private void InitializeMockData()
        {
            // Dane do wykresów
            Revenues = new ChartValues<double> { 10000, 15000, 20000, 25000 };
            Expenses = new ChartValues<double> { 5000, 7000, 9000, 11000 };
            Months = new[] { "Styczeń", "Luty", "Marzec", "Kwiecień" };

            ClientARevenue = new ChartValues<double> { 15000 };
            ClientBRevenue = new ChartValues<double> { 10000 };
            OtherClientsRevenue = new ChartValues<double> { 5000 };

            // Dane szczegółowe
            DetailedRecords = new ObservableCollection<DetailedRecord>
            {
                new DetailedRecord { Date = "01-01-2025", Description = "Sprzedaż A", Amount = 10000 },
                new DetailedRecord { Date = "01-02-2025", Description = "Sprzedaż B", Amount = 15000 },
                new DetailedRecord { Date = "01-03-2025", Description = "Sprzedaż C", Amount = 20000 },
            };
        }

        public override void Save()
        {
            // Funkcja zapisu. Jeśli później będą dane do zapisania w bazie danych, 
            // można tutaj je zapisać, np. designOfficeEntities.SaveChanges().
            // Na razie logika zapisu może być pusta:
        }

        #endregion
    }

    #region Helper Class

    public class DetailedRecord
    {
        public string Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }

    #endregion
}