using Microsoft.Maui.Controls;
using zaliczenieMaui.Models;
using zaliczenieMaui.Services;

namespace zaliczenieMaui
{
    public partial class HomePage : TabbedPage
    {
        private DatabaseService _databaseService;
        private User _user;

        public HomePage(DatabaseService databaseService, User user)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _user = user;

            Children.Add(new AllProjectsPage(_databaseService) { Title = "All Projects" });
            Children.Add(new MyProjectsPage(_databaseService, _user) { Title = "My Projects" });
            Children.Add(new AddProjectPage(_databaseService, _user) { Title = "Add Project" });
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Logout", "Cancel");
            if (confirm)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
                await DisplayAlert("Logged Out", "You have been successfully logged out.", "OK");
            }
        }
    }
}