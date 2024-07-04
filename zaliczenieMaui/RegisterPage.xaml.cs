using Microsoft.Maui.Controls;
using zaliczenieMaui.Models;
using zaliczenieMaui.Services;
using System;

namespace zaliczenieMaui
{
    public partial class RegisterPage : ContentPage
    {
        private DatabaseService _databaseService;

        public RegisterPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;
            var confirmPassword = ConfirmPasswordEntry.Text;

            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password must be at least 6 characters long.", "OK");
                return;
            }
            if (password != confirmPassword)
            {
                ErrorMessage.Text = "Passwords do not match.";
                ErrorMessage.IsVisible = true;
                return;
            }

            var existingUser = await _databaseService.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                ErrorMessage.Text = "Email is already in use.";
                ErrorMessage.IsVisible = true;
                return;
            }

            var user = new User { Email = email, Password = password };
            await _databaseService.SaveUserAsync(user);

            await DisplayAlert("Success", "Account created successfully", "OK");
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        private void OnBackToLoginTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}
