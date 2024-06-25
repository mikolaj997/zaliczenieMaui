using zaliczenieMaui.Models;

namespace zaliczenieMaui;

public partial class ProjectDetailsPage : ContentPage
{
    public ProjectDetailsPage(string name, string description)
    {
        InitializeComponent();
        projectNameLabel.Text = name;
        projectDescriptionLabel.Text = description;
    }
}

