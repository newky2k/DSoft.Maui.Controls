namespace MauiSampleApp;

public partial class PinchZoomPage : ContentPage
{
	public PinchZoomPage()
	{
		InitializeComponent();
	}

	private async void OnCloseClicked(object? sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}