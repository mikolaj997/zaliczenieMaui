using Microsoft.Maui.Controls;
using zaliczenieMaui.Models;
using zaliczenieMaui.Services;
using System;
using System.IO;

namespace zaliczenieMaui
{
    public partial class MainPage : ContentPage
    {
        private DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "projects.db3"));
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            var user = await _databaseService.GetUserAsync(email, password);
            if (user != null)
            {
                Application.Current.MainPage = new NavigationPage(new HomePage(_databaseService, user));
            }
            else
            {
                ErrorMessage.Text = "Invalid email or password";
                ErrorMessage.IsVisible = true;
            }
        }
        private void OnRegisterTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new RegisterPage(_databaseService));
        }

    }

}
