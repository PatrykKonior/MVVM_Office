using System;
using System.Collections.ObjectModel;
using System.Linq;
using LiveCharts;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;

namespace MVVMFirma.ViewModels
{
    public class AnalizaTerminowViewModel : JedenViewModel<object>
    {
        // Dane dla wykresu kołowego - zadania
        public ChartValues<int> TaskStatusCounts { get; private set; }
        public string[] TaskStatuses { get; private set; }

        // Dane dla wykresu kołowego - projekty
        public ChartValues<int> ProjectStatusCounts { get; private set; }
        public string[] ProjectStatuses { get; private set; }

        // Listy zadań i projektów
        public ObservableCollection<ZadaniaForAllView> OrderedTasks { get; private set; }
        public ObservableCollection<ProjectsForAllView> OrderedProjects { get; private set; }

        // Widoczność danych
        public bool HasTaskData => TaskStatusCounts != null && TaskStatusCounts.Any();
        public bool HasProjectData => ProjectStatusCounts != null && ProjectStatusCounts.Any();

        public AnalizaTerminowViewModel() : base("Analiza Terminów Zadań i Projektów")
        {
            // Inicjalizacja wykresów i list
            TaskStatusCounts = new ChartValues<int>();
            ProjectStatusCounts = new ChartValues<int>();
            OrderedTasks = new ObservableCollection<ZadaniaForAllView>();
            OrderedProjects = new ObservableCollection<ProjectsForAllView>();

            // Wstępne ładowanie danych
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new DesignOfficeEntities())
            {
                // Inicjalizacja modeli biznesowych
                var tasksLogic = new TasksLogic(db);
                var projectsLogic = new ProjectsLogicB(db);

                // Pobieranie danych do wykresu kołowego zadań
                var taskStatusData = tasksLogic.GetTaskStatusCounts();
                TaskStatuses = taskStatusData.Keys.ToArray();
                TaskStatusCounts = new ChartValues<int>(taskStatusData.Values);

                // Pobieranie danych do wykresu kołowego projektów
                var projectStatusData = projectsLogic.GetProjectStatusCounts();
                ProjectStatuses = projectStatusData.Keys.ToArray();
                ProjectStatusCounts = new ChartValues<int>(projectStatusData.Values);

                // Pobieranie danych dla list zadań i projektów
                OrderedTasks = new ObservableCollection<ZadaniaForAllView>(
                    tasksLogic.GetTasksOrderedByDeadline()
                );
                OrderedProjects = new ObservableCollection<ProjectsForAllView>(
                    projectsLogic.GetProjectsOrderedByDeadline()
                );

                // Obsługa pustych danych
                if (!HasTaskData) Console.WriteLine("Brak danych dla zadań.");
                if (!HasProjectData) Console.WriteLine("Brak danych dla projektów.");
            }

            // Aktualizacja widoku
            OnPropertyChanged(() => TaskStatusCounts);
            OnPropertyChanged(() => TaskStatuses);
            OnPropertyChanged(() => ProjectStatusCounts);
            OnPropertyChanged(() => ProjectStatuses);
            OnPropertyChanged(() => OrderedTasks);
            OnPropertyChanged(() => OrderedProjects);
            OnPropertyChanged(() => HasTaskData);
            OnPropertyChanged(() => HasProjectData);
        }

        // Wymagana implementacja abstrakcyjnej metody Save
        public override void Save()
        {
            Console.WriteLine("Metoda Save nie jest używana w tym widoku biznesowym.");
        }
    }
}
