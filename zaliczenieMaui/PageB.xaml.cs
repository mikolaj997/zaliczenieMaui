namespace zaliczenieMaui;

public partial class PageB : ContentPage
{
	public PageB()
	{
		InitializeComponent();
	}
    private async void GoBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}