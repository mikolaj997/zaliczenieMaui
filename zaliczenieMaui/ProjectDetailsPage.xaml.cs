using zaliczenieMaui.Models;

namespace zaliczenieMaui;

public partial class ProjectDetailsPage : ContentPage
{
    public Project Project { get; set; }

    public ProjectDetailsPage(Project project)
    {
        InitializeComponent();
        Project = project;
        BindingContext = Project;
       
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

   

   
}



