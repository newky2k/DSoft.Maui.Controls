namespace MauiSampleApp;

public partial class ControlsPage : ContentPage
{
	public ControlsPage()
	{
		InitializeComponent();
	}
	
	private async void OnCloseClicked(object? sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}