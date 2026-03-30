using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSoft.Maui.Controls.Events;

namespace MauiSampleApp;

public partial class SegmentedControlPage : ContentPage
{
    private SegmentedControlPageViewModel _viewModel;
    
    public SegmentedControlPageViewModel ViewModel
    {
        get { return _viewModel; }
        set { _viewModel = value; BindingContext = _viewModel; }
    }
    
    public SegmentedControlPage()
    {
        InitializeComponent();

        this.ViewModel = new();
    }
    
    private async void OnCloseClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    
    private void OnSegmentSelected(object sender, SegmentSelectedEventArgs e)
    {
        ResultLabel.Text = $"{e.SelectedItem}  (index {e.SelectedIndex}, previously {e.PreviousItem})";
    }
}