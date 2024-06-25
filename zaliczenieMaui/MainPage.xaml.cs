using System.Collections.ObjectModel;
using zaliczenieMaui.Data;
using zaliczenieMaui.Models;

namespace zaliczenieMaui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            

        }

        private async void GoToPageAClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageA());
        }

        private async void GoToPageBClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageB());
        }
        private void ShowProjectsClicked(object sender, EventArgs e)
        {
            // Pokaż lub ukryj szczegóły projektów
            ProjectDetails.IsVisible = !ProjectDetails.IsVisible;
            AllProjects.IsVisible = !AllProjects.IsVisible;
        }
    }

}
