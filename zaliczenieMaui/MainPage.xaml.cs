﻿using zaliczenieMaui.Models;
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


        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            await _databaseService.AddProjectAsync(projectNameEntry.Text, projectDescriptionEntry.Text);
            LoadProjects();
            projectNameEntry.Text = string.Empty;
            projectDescriptionEntry.Text = string.Empty;
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
