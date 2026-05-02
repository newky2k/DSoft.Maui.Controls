using DSoft.Maui.Controls.Events;

namespace MauiSampleApp;

public partial class SpinnerPickerPage : ContentPage
{
    public SpinnerPickerPage()
    {
        InitializeComponent();
        PopulatePickers();
    }

    private void PopulatePickers()
    {
        var months = new[]
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        var days = Enumerable.Range(1, 31).Select(d => d.ToString()).ToList();
        var years = Enumerable.Range(2020, 11).Select(y => y.ToString()).ToList();

        MonthPicker.ItemsSource = months;
        DayPicker.ItemsSource = days;
        YearPicker.ItemsSource = years;

        // Default to today
        var today = DateTime.Today;
        MonthPicker.SelectedIndex = today.Month - 1;
        DayPicker.SelectedIndex = today.Day - 1;
        YearPicker.SelectedIndex = Math.Clamp(today.Year - 2020, 0, 10);

        UpdateDateLabel();

        var countries = new[]
        {
            "Australia", "Brazil", "Canada", "Denmark", "Egypt",
            "France", "Germany", "Hungary", "India", "Japan",
            "Kenya", "Luxembourg", "Mexico", "Norway", "Oman",
            "Portugal", "Qatar", "Romania", "Spain", "Turkey",
            "Ukraine", "Vietnam", "Wales", "Xanadu", "Yemen", "Zimbabwe"
        };

        CountryPicker.ItemsSource = countries;
        CountryLabel.Text = $"Selected: {countries[0]}";
    }

    private void OnSelectionChanged(object? sender, SpinnerSelectedEventArgs e)
        => UpdateDateLabel();

    private void UpdateDateLabel()
    {
        var month = MonthPicker.SelectedItem as string ?? "—";
        var day = DayPicker.SelectedItem as string ?? "—";
        var year = YearPicker.SelectedItem as string ?? "—";
        SelectionLabel.Text = $"Selected: {month} {day}, {year}";
    }

    private void OnCountrySelectionChanged(object? sender, SpinnerSelectedEventArgs e)
        => CountryLabel.Text = $"Selected: {e.SelectedItem}";

    private async void OnCloseClicked(object? sender, EventArgs e)
        => await Navigation.PopModalAsync();
}
