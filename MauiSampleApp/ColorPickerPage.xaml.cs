using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSampleApp;

public partial class ColorPickerPage : ContentPage
{
    public ColorPickerPage()
    {
        InitializeComponent();
    }
    
    private async void OnCloseClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}