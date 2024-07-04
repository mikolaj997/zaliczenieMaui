using Microsoft.Maui.Controls;
using zaliczenieMaui.Models;
using zaliczenieMaui.Services;
using System;

namespace zaliczenieMaui
{
    public partial class EditProjectPage : ContentPage
    {
        private DatabaseService _databaseService;
        private Project _project;

        public EditProjectPage(DatabaseService databaseService, Project project)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _project = project;

            TitleEntry.Text = _project.Title;
            DescriptionEditor.Text = _project.Description;
            DeadlinePicker.Date = _project.Deadline;
            StatusPicker.SelectedItem = _project.Status;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _project.Title = TitleEntry.Text;
            _project.Description = DescriptionEditor.Text;
            _project.Deadline = DeadlinePicker.Date;
            _project.Status = (string)StatusPicker.SelectedItem;

            await _databaseService.UpdateProjectAsync(_project);
            MessagingCenter.Send(this, "ProjectUpdated", _project);
            await DisplayAlert("Success", "Project updated successfully", "OK");
            await Navigation.PopAsync();
        }
    }
}
