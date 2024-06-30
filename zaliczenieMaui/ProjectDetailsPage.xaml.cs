using zaliczenieMaui.Models;

namespace zaliczenieMaui;

public partial class ProjectDetailsPage : ContentPage
{
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

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        var task = new TaskModel
        {
            Title = taskTitleEntry.Text,
            Description = taskDescriptionEditor.Text,
            ProjectId = Project.Id
        };
        // Dodaj zadanie do bazy danych
        await DatabaseService.AddTaskAsync(task);
        LoadTasks();
    }

    private async void LoadTasks()
    {
        var tasks = await DatabaseService.GetTasksAsync(Project.Id);
        tasksCollection.ItemsSource = tasks;
    }
}



