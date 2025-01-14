using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MVVMFirma.Models.Entities;
using MVVMFirma.Helper;
using System.Windows.Input;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class WszystkieWydatkiViewModel : WszystkieViewModel<WydatkiForAllView>
    {

        #region Constructor
        public WszystkieWydatkiViewModel()
            :base("Wydatki")
        {
        }
        #endregion

        #region Sort And Find
        // tu decydujemy po czym sortowac 
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Data wydatku",
                "Kwota netto",
                "Kwota brutto",
                "Nazwa projektu"
            };
        }
        // tu decydujemy jak sortowac
        public override void Sort()
        {
            switch (SortField)
            {
                case "Data wydatku":
                    List = new ObservableCollection<WydatkiForAllView>(List.OrderBy(w => w.ExpenseDate));
                    break;
                case "Kwota netto":
                    List = new ObservableCollection<WydatkiForAllView>(List.OrderBy(w => w.NetAmount));
                    break;
                case "Kwota brutto":
                    List = new ObservableCollection<WydatkiForAllView>(List.OrderBy(w => w.GrossAmount));
                    break;
                case "Nazwa projektu":
                    List = new ObservableCollection<WydatkiForAllView>(List.OrderBy(w => w.ProjectsName));
                    break;
                default:
                    break;
            }
        }
        // tu decydujemy po czym filtrowac
        public override List<string> GetComboboxFindList()
        {
            return new List<string>
            {
                "Nazwa projektu",
                "Opis wydatku",
                "Data wydatku"
            };
        }
        // tu decydujemy jak filtrowac
        public override void Find()
        {
            if (string.IsNullOrWhiteSpace(FindTextBox)) return;

            switch (FindField)
            {
                case "Nazwa projektu":
                    List = new ObservableCollection<WydatkiForAllView>(List.Where(w => w.ProjectsName != null &&
                        w.ProjectsName.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;
                case "Opis wydatku":
                    List = new ObservableCollection<WydatkiForAllView>(List.Where(w => w.ExpenseDescription != null &&
                        w.ExpenseDescription.IndexOf(FindTextBox, StringComparison.OrdinalIgnoreCase) >= 0));
                    break;
                case "Data wydatku":
                    // Rozdzielenie logiki filtrowania na podstawie długości wprowadzonego tekstu
                    List = new ObservableCollection<WydatkiForAllView>(List.Where(w =>
                    {
                        if (w.ExpenseDate == null) return false;

                        // Parsowanie roku, miesiąca, dnia
                        if (int.TryParse(FindTextBox, out var year))
                        {
                            return w.ExpenseDate.Value.Year == year;
                        }
                        else if (DateTime.TryParseExact(FindTextBox, "MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDay)) // MM-dd (miesiąc-dzień)
                        {
                            return w.ExpenseDate.Value.Month == monthDay.Month && w.ExpenseDate.Value.Day == monthDay.Day;
                        }
                        else if (DateTime.TryParse(FindTextBox, out var exactDate)) // Pełna data
                        {
                            return w.ExpenseDate == exactDate;
                        }

                        return false; // Jeśli nic nie pasuje
                    }));
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<WydatkiForAllView>
            (
                List = new ObservableCollection<WydatkiForAllView>
                (
                    from expense in designOfficeEntities.Expenses
                    select new WydatkiForAllView
                    {
                        ExpenseID = expense.ExpenseID,
                        ProjectsName = expense.Projects != null ? expense.Projects.ProjectName : "Brak projektu",
                        ProjectsType = expense.Projects != null ? expense.Projects.ProjectType : "Brak typu projektu",
                        ExpenseDate = expense.ExpenseDate,
                        ExpenseDescription = expense.ExpenseDescription,
                        NetAmount = expense.NetAmount,
                        VATAmount = expense.VATAmount,
                        GrossAmount = expense.GrossAmount
                    }
                )
            );
        }
        #endregion
    }


}