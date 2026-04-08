namespace MauiSampleApp;

public partial class TabViewPage : ContentPage
{
    public TabViewPage()
    {
        InitializeComponent();
    }

    private async void OnCloseClicked(object sender, EventArgs e)
        => await Navigation.PopModalAsync();
}
