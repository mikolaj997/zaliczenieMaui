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
            _project.Title = TitleEntry.Text;
            _project.Description = DescriptionEditor.Text;
            _project.Deadline = DeadlinePicker.Date;
            _project.Status = (string)StatusPicker.SelectedItem;

            await _databaseService.UpdateProjectAsync(_project);
            MessagingCenter.Send(this, "ProjectUpdated", _project);
            await DisplayAlert("Success", "Project updated successfully", "OK");
            await Navigation.PopAsync();
        }
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool isConfirmed = await DisplayAlert("Confirm Delete",
                                                  "Are you sure you want to delete this project?",
                                                  "Yes", "No");
            if (isConfirmed)
            {
                await _databaseService.DeleteProjectAsync(_project);
                MessagingCenter.Send(this, "ProjectDeleted", _project);
                await DisplayAlert("Success", "Project deleted successfully.", "OK");
                await Navigation.PopAsync();  // Powrót do poprzedniej strony po usuniêciu
            }
        }
    }
}
