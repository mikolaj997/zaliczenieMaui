using Microsoft.Maui.Controls;
using zaliczenieMaui.Models;
using zaliczenieMaui.Services;
using System;

namespace zaliczenieMaui
{
    public partial class AddProjectPage : ContentPage
    {
        private DatabaseService _databaseService;
        private User _user;

        public AddProjectPage(DatabaseService databaseService, User user)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _user = user;
        }

        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TitleEntry.Text) || string.IsNullOrEmpty(DescriptionEditor.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }
            if (DeadlinePicker.Date <= DateTime.Today)
            {
                await DisplayAlert("Error", "The deadline must be in the future.", "OK");
                return;
            }

            var project = new Project
            {
                Title = TitleEntry.Text,
                Description = DescriptionEditor.Text,
                Deadline = DeadlinePicker.Date,
                Status = "Not Started",
                OwnerEmail = _user.Email
            };

            await _databaseService.SaveProjectAsync(project);
            await DisplayAlert("Success", "Project added successfully", "OK");
            MessagingCenter.Send(this, "RefreshProjects");
            TitleEntry.Text = string.Empty;
            DescriptionEditor.Text = string.Empty;
            DeadlinePicker.Date = DateTime.Now;
        }
    }
}