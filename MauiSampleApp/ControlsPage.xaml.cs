namespace MauiSampleApp;

public partial class ControlsPage : ContentPage
{
	private ControlsPageViewModel _viewModel;

	internal ControlsPageViewModel ViewModel
	{
		get { return _viewModel; }
		set { _viewModel = value; BindingContext = _viewModel; }
	}

	public ControlsPage()
	{
		InitializeComponent();

		ViewModel = new ControlsPageViewModel();
	}
}