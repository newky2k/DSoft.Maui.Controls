namespace MauiSampleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ThemeService.ApplySaved();

            MainPage = new AppShell();
        }
    }
}
