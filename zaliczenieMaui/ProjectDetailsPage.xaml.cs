using Microsoft.Maui.Controls;
using zaliczenieMaui.Models;
using zaliczenieMaui.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zaliczenieMaui
{
    public partial class ProjectDetailsPage : ContentPage
    {
        private DatabaseService _databaseService;
        private User _user;
        private Project _project;
        private bool _isOwner;
        private string _attachedFilePath;

        public ProjectDetailsPage(DatabaseService databaseService, User user, Project project)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _user = user;
            _project = project;
            _isOwner = project.OwnerEmail == user.Email;

            TitleLabel.Text = _project.Title;
            OwnerLabel.Text = $"Owner: {_project.OwnerEmail}";
            DescriptionLabel.Text = _project.Description;
            DeadlineLabel.Text = $"Deadline: {_project.Deadline.ToShortDateString()}";
            StatusLabel.Text = $"Status: {_project.Status}";

            BindingContext = new { IsOwner = _isOwner };
            LoadTasks();
            LoadComments();
            LoadMembers();

            MessagingCenter.Subscribe<EditProjectPage, Project>(this, "ProjectUpdated", (sender, updatedProject) =>
            {
                _project = updatedProject;
                TitleLabel.Text = _project.Title;
                OwnerLabel.Text = $"Owner: {_project.OwnerEmail}";
                DescriptionLabel.Text = _project.Description;
                DeadlineLabel.Text = $"Deadline: {_project.Deadline.ToShortDateString()}";
                StatusLabel.Text = $"Status: {_project.Status}";
            });
        }

        private async void LoadTasks()
        {
            var tasks = await _databaseService.GetTasksByProjectAsync(_project.Id);
            TasksCollectionView.ItemsSource = tasks;
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            string title = await DisplayPromptAsync("New Task", "Enter task title:");
            if (string.IsNullOrEmpty(title)) return;

            string description = await DisplayPromptAsync("New Task", "Enter task description:");
            if (string.IsNullOrEmpty(description)) return;

            var task = new ProjectTask
            {
                ProjectId = _project.Id,
                Title = title,
                Description = description,
                IsCompleted = false
            };
            await _databaseService.SaveTaskAsync(task);
            LoadTasks();
        }
        private async void OnTaskTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is ProjectTask task)
            {
                bool confirm = await DisplayAlert("Update Task", $"Mark task '{task.Description}' as completed?", "Yes", "No");
                if (confirm)
                {
                    task.IsCompleted = true;
                    await _databaseService.UpdateTaskAsync(task);
                    LoadTasks();
                }
            }
        }

        private async void OnEditProjectClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProjectPage(_databaseService, _project));
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<EditProjectPage, Project>(this, "ProjectUpdated");
        }
        /*private async void OnAttachFileClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {
                    _attachedFilePath = result.FullPath; // Zapisanie œcie¿ki do za³¹cznika
                    await DisplayAlert("File Attached", $"File: {result.FileName} has been attached.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error attaching file: {ex.Message}", "OK");
            }
        }*/

        private async void OnSendCommentClicked(object sender, EventArgs e)
        {
            var comment = new Comment
            {
                ProjectId = _project.Id,
                AuthorEmail = _user.Email,
                Text = CommentEntry.Text,
                Timestamp = DateTime.UtcNow,
                //FilePath = _attachedFilePath
            };

            await _databaseService.SaveCommentAsync(comment);
            CommentEntry.Text = "";
            //_attachedFilePath = null;
            LoadComments();
        }

        private async void LoadComments()
        {
            var comments = await _databaseService.GetCommentsByProjectIdAsync(_project.Id);
            CommentsCollectionView.ItemsSource = comments;
        }
        private async void OnAddMemberClicked(object sender, EventArgs e)
        {
            string email = await DisplayPromptAsync("Add Member", "Enter member's email:");
            if (!string.IsNullOrEmpty(email))
            {
                var member = new ProjectMember { ProjectId = _project.Id, MemberEmail = email };
                await _databaseService.AddMemberToProjectAsync(member);
                LoadMembers();
            }
        }

        private async void LoadMembers()
        {
            var members = await _databaseService.GetMembersByProjectAsync(_project.Id);
            MembersCollectionView.ItemsSource = members;
        }
    }
}
