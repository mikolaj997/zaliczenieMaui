using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;


namespace zaliczenieMaui.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Nierozpoczęty";
        public DateTime? DueDate { get; set; } // Nowe pole na termin wykonania projektu
        public ObservableCollection<TaskModel> Tasks { get; set; }
        public ICommand AddTaskCommand { get; set; }

        private readonly DatabaseService _databaseService;

        public Project(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            Tasks = new ObservableCollection<TaskModel>();
            AddTaskCommand = new Command(async () => await AddTask());
        }

        private async Task AddTask()
        {
            var newTask = new TaskModel
            {
                Title = "New Task",
                Description = "Description of the new task",
                ProjectId = Id
            };

            // Dodaj zadanie do bazy danych
            await _databaseService.AddTaskAsync(newTask);

            // Dodaj zadanie do listy Tasks w pamięci
            Tasks.Add(newTask);
        }
    }
}
