using System.Collections.ObjectModel;
using System.Windows.Input;
using MVVMFirma.Helper;

namespace MVVMFirma.ViewModels
{
    public class StartPageViewModel : WorkspaceViewModel
    {
        public ObservableCollection<CommandViewModel> NewItemsCommands { get; }
        public ObservableCollection<CommandViewModel> AllItemsCommands { get; }
        public ObservableCollection<CommandViewModel> BusinessViewsCommands { get; }

        public StartPageViewModel(MainWindowViewModel mainViewModel)
        {
            // NOWE ELEMENTY
            NewItemsCommands = new ObservableCollection<CommandViewModel>
            {
                new CommandViewModel("Nowy Projekt", new BaseCommand(() => mainViewModel.CreateView(new NowyProjektViewModel()))),
                new CommandViewModel("Nowy Przydział", new BaseCommand(() => mainViewModel.CreateView(new NowyProjectAssignmentsViewModel()))),
                new CommandViewModel("Nowy Kontrakt", new BaseCommand(() => mainViewModel.CreateView(new NowyKontraktViewModel()))),
                new CommandViewModel("Nowe Zadanie", new BaseCommand(() => mainViewModel.CreateView(new NoweZadanieViewModel()))),
                new CommandViewModel("Nowy Towar", new BaseCommand(() => mainViewModel.CreateView(new NowyTowarViewModel()))),
                new CommandViewModel("Nowy Materiał", new BaseCommand(() => mainViewModel.CreateView(new NowyProjectMaterialViewModel()))),
                new CommandViewModel("Nowa Faktura", new BaseCommand(() => mainViewModel.CreateView(new NowaFakturaViewModel()))),
                new CommandViewModel("Nowa Sprzedaż", new BaseCommand(() => mainViewModel.CreateView(new NowaSprzedazViewModel()))),
                new CommandViewModel("Nowy Sprzedany Produkt", new BaseCommand(() => mainViewModel.CreateView(new NowySaleItemViewModel()))),
                new CommandViewModel("Nowy Wydatek", new BaseCommand(() => mainViewModel.CreateView(new NowyWydatekViewModel()))),
                new CommandViewModel("Nowa Płatność", new BaseCommand(() => mainViewModel.CreateView(new NowaPlatnoscViewModel()))),
                new CommandViewModel("Nowy Klient", new BaseCommand(() => mainViewModel.CreateView(new NowyKlientViewModel()))),
                new CommandViewModel("Nowy Pracownik", new BaseCommand(() => mainViewModel.CreateView(new NowyPracownikViewModel()))),
                new CommandViewModel("Nowy Dział", new BaseCommand(() => mainViewModel.CreateView(new NowyDzialViewModel()))),
                new CommandViewModel("Nowy Czas Pracy", new BaseCommand(() => mainViewModel.CreateView(new NowyCzasPracyViewModel()))),
            };

            // WSZYSTKIE ELEMENTY
            AllItemsCommands = new ObservableCollection<CommandViewModel>
            {
                new CommandViewModel("Lista Projektów", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieProjektyViewModel()))),
                new CommandViewModel("Lista Przydziałów", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieProjectAssignmentsViewModel()))),
                new CommandViewModel("Lista Kontraktów", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieKontraktyViewModel()))),
                new CommandViewModel("Lista Zadań", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieZadaniaViewModel()))),
                new CommandViewModel("Lista Towarów", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieTowaryViewModel()))),
                new CommandViewModel("Lista Materiałów", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieProjectMaterialsViewModel()))),
                new CommandViewModel("Lista Faktur", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieFakturyViewModel()))),
                new CommandViewModel("Lista Sprzedaży", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieSprzedazeViewModel()))),
                new CommandViewModel("Lista Sprzedanych Produktów", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieSaleItemsViewModel()))),
                new CommandViewModel("Lista Wydatków", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieWydatkiViewModel()))),
                new CommandViewModel("Lista Płatności", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkiePlatnosciViewModel()))),
                new CommandViewModel("Lista Klientów", new BaseCommand(() => mainViewModel.CreateShowAll(new WszyscyKlienciViewModel()))),
                new CommandViewModel("Lista Pracowników", new BaseCommand(() => mainViewModel.CreateShowAll(new WszyscyPracownicyViewModel()))),
                new CommandViewModel("Lista Działów", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieDzialyViewModel()))),
                new CommandViewModel("Lista Czasów Pracy", new BaseCommand(() => mainViewModel.CreateShowAll(new WszystkieCzasyPracyViewModel()))),
            };

            // WIDOKI BIZNESOWE
            BusinessViewsCommands = new ObservableCollection<CommandViewModel>
            {
                new CommandViewModel("Analiza Przychodów", new BaseCommand(() => mainViewModel.CreateView(new AnalizaPrzychodowIWydatkowViewModel()))),
                new CommandViewModel("Analiza Projektów", new BaseCommand(() => mainViewModel.CreateView(new AnalizaPrzychodowProjektowViewModel()))),
                new CommandViewModel("Analiza Deadlinów", new BaseCommand(() => mainViewModel.CreateView(new AnalizaTerminowViewModel()))),
            };
        }
    }
}
