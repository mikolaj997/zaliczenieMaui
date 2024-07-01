using zaliczenieMaui.Models;

namespace zaliczenieMaui;

public partial class ProjectDetailsPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public Project Project { get; set; }

    public ProjectDetailsPage(Project project)
    {
        InitializeComponent();
        Project = project;
        BindingContext = Project;
        LoadTasks();
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async Task OnAddTaskClicked(object sender, EventArgs e)
    {
        var task = new TaskModel
        {
            Title = taskTitleEntry.Text,
            Description = taskDescriptionEditor.Text,
            ProjectId = Project.Id
        };

        // Dodaj zadanie do bazy danych
        await _databaseService.AddTaskAsync(task);
        await LoadTasks();
    }

    private async Task LoadTasks()
    {
        var tasks = await _databaseService.GetTasksAsync(Project.Id);
        tasksCollection.ItemsSource = tasks;
    }
}



