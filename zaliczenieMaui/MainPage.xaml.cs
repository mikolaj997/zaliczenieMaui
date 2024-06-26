using zaliczenieMaui.Models;
using Microsoft.Data.Sqlite;

namespace zaliczenieMaui
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LoadProjects();
        }

        private async void LoadProjects()
        {
            var projects = await _databaseService.GetProjectsAsync();
            projectsCollection.ItemsSource = projects;
        }

        private async void NavigateToAddProject(object sender, EventArgs e)
        {
            // Jeśli już jesteś na stronie MainPage, nie rób nic
            if (Navigation.NavigationStack.LastOrDefault() is MainPage)
                return;

            await Navigation.PushAsync(new MainPage());
        }
        private async void NavigateToAllProjects(object sender, EventArgs e)
        {
            // Jeśli już jesteś na stronie AllProjects, nie rób nic
            if (Navigation.NavigationStack.LastOrDefault() is AllProjectsPage)
                return;

            await Navigation.PushAsync(new AllProjectsPage());
        }

        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            var projectName = projectNameEntry.Text;
            var projectDescription = projectDescriptionEntry.Text;
            var projectDueDate = projectDueDatePicker.Date;

            await _databaseService.AddProjectAsync(projectName, projectDescription, projectDueDate);
            LoadProjects();
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
        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedProject = (Project)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new ProjectDetailsPage(selectedProject.Name, selectedProject.Description));
                // Deselect item
                ((CollectionView)sender).SelectedItem = null;
            }
        }
        private async void GoToEditPageClicked(object sender, EventArgs e)
        {
            if (projectsCollection.SelectedItem is Project selectedProject)
            {
                string action = await DisplayActionSheet("Zmień status projektu", "Cancel", null, "Nierozpoczęty", "W trakcie", "Zakończony");
                if (action != "Cancel")
                {
                    await _databaseService.UpdateProjectStatusAsync(selectedProject.Id, action);
                    LoadProjects(); // Odśwież listę projektów
                }
            }
        }
        private void RefreshProjects(object sender, EventArgs e)
        {
            LoadProjects();
        }
    }
}
