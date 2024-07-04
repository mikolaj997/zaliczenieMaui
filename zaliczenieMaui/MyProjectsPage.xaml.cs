using Microsoft.Maui.Controls;
using zaliczenieMaui.Models;
using zaliczenieMaui.Services;
using System.Collections.Generic;

namespace zaliczenieMaui
{
    public partial class MyProjectsPage : ContentPage
    {
        private DatabaseService _databaseService;
        private User _user;

        public MyProjectsPage(DatabaseService databaseService, User user)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _user = user;
            LoadProjects();

            MessagingCenter.Subscribe<AddProjectPage>(this, "RefreshProjects", (sender) => {
                LoadProjects();
            });
        }

        private async void LoadProjects()
        {
            var ownerProjects = await _databaseService.GetProjectsByOwnerAsync(_user.Email);
            var memberProjects = await _databaseService.GetProjectsByMemberAsync(_user.Email);
            var allProjects = new List<Project>(ownerProjects);
            allProjects.AddRange(memberProjects);
            ProjectsCollectionView.ItemsSource = allProjects;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AddProjectPage>(this, "RefreshProjects");
        }
        private async void OnProjectTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Project selectedProject)
            {
                await Navigation.PushAsync(new ProjectDetailsPage(_databaseService, _user, selectedProject));
            }
        }
    }
}
