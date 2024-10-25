namespace MauiSampleApp;

public partial class ChartsPage : ContentPage
{
	private ChartsPageViewModel _viewModel;

	internal ChartsPageViewModel ViewModel
	{
		get { return _viewModel; }
		set { _viewModel = value; BindingContext = _viewModel; }
	}
	
	public ChartsPage()
	{
		InitializeComponent();
		
		ViewModel = new ChartsPageViewModel();
	}
	
	private async void OnCloseClicked(object? sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}