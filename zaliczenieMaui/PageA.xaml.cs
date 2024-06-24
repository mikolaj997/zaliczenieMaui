namespace zaliczenieMaui;

public partial class PageA : ContentPage
{
	public PageA()
	{
		InitializeComponent();
	}
    private async void GoBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}