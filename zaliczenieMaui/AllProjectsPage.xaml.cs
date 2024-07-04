using Microsoft.Maui.Controls;
using zaliczenieMaui.Models;
using zaliczenieMaui.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zaliczenieMaui
{
    public partial class AllProjectsPage : ContentPage
    {
        private DatabaseService _databaseService;

        public AllProjectsPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            LoadProjects();

            MessagingCenter.Subscribe<AddProjectPage>(this, "RefreshProjects", (sender) => {
                LoadProjects();
            });
        }

        private async void LoadProjects()
        {
            var projects = await _databaseService.GetAllProjectsAsync();
            ProjectsCollectionView.ItemsSource = projects;
        }
        private void RefreshProjects(object sender, EventArgs e)
        {
            LoadProjects();
        }

    }
}