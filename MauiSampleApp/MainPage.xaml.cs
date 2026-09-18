namespace MauiSampleApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private IEnumerable<DataEntry> _data = new List<DataEntry>();

        public IEnumerable<DataEntry> Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged(nameof(Data)); }
        }

        public MainPage()
        {
            InitializeComponent();
            ThemeToolbarItem.Text = ThemeService.Label;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // The theme may have been changed from a modal sample page.
            ThemeToolbarItem.Text = ThemeService.Label;
        }

        private void OnThemeClicked(object sender, EventArgs e)
        {
            ThemeService.Cycle();
            ThemeToolbarItem.Text = ThemeService.Label;
        }

        private async void OnChartsButtonClicked(object sender, EventArgs e)
        {
            var dlg = new ChartsPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));
        }

        private async void OnControlsClicked(object sender, EventArgs e)
        {
            var dlg = new ControlsPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));
        }

        private async void OnPanZoomClicked(object sender, EventArgs e)
        {
            var dlg = new PinchZoomPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));

        }
        
        private async void OnColorPickerClicked(object sender, EventArgs e)
        {
            var dlg = new ColorPickerPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));

        }

        private async void OnWizardPageClicked(object sender, EventArgs e)
        {
            var dlg = new WizardPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));

        }
        
        private async void OnSegmentedControlPageClicked(object sender, EventArgs e)
        {
            var dlg = new SegmentedControlPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));

        }
        
        private async void OnHeatmapPageClicked(object sender, EventArgs e)
        {
            var dlg = new HeatMapPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));

        }

        private async void OnSignaturePadPageClicked(object sender, EventArgs e)
        {
            var dlg = new SignaturePadPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));
        }

        private async void OnTabViewPageClicked(object sender, EventArgs e)
        {
            var dlg = new TabViewPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));
        }

        private async void OnDataGridPageClicked(object sender, EventArgs e)
        {
            var dlg = new DataGridPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));
        }

        private async void OnSpinnerPickerPageClicked(object sender, EventArgs e)
        {
            var dlg = new SpinnerPickerPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));
        }

        private async void OnDatePickerPageClicked(object sender, EventArgs e)
        {
            var dlg = new DatePickerPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));
        }

        private async void OnGradientBorderPageClicked(object sender, EventArgs e)
        {
            var dlg = new GradientBorderPage();

            await Navigation.PushModalAsync(new NavigationPage(dlg));
        }
    }

}
