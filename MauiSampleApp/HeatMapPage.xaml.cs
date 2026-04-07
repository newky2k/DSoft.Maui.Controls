using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSampleApp;

public partial class HeatMapPage : ContentPage
{
    private HeatMapPageViewModel _viewModel;
    
    public HeatMapPageViewModel ViewModel
    {
        get { return _viewModel; }
        set { _viewModel = value; BindingContext = _viewModel; }
    }
    
    public HeatMapPage()
    {
        InitializeComponent();
        
        ViewModel = new ();
   
    }
    
    private async void OnCloseClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}