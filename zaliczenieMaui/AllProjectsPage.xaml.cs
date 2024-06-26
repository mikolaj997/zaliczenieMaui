using zaliczenieMaui.Models;

namespace zaliczenieMaui;
public partial class AllProjectsPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public AllProjectsPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        LoadProjects();
    }

    private async void NavigateToAddProject(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void NavigateToAllProjects(object sender, EventArgs e)
    {
        // Jeœli ju¿ jesteœ na stronie AllProjects, nie rób nic
        if (Navigation.NavigationStack.LastOrDefault() is AllProjectsPage)
            return;

        await Navigation.PushAsync(new AllProjectsPage());
    }

    private async void LoadProjects()
    {
        var projects = await _databaseService.GetProjectsAsync();
        projectsCollection.ItemsSource = projects;
    }

    private async void OnDelete(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var projectId = (int)button.CommandParameter;

        var isConfirmed = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this project?", "Yes", "No");
        if (isConfirmed)
        {
            await _databaseService.DeleteProjectAsync(projectId);
            LoadProjects();
        }
    }
}
