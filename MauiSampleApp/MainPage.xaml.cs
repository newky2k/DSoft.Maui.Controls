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
    }

}
