using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace MauiSampleApp;

public partial class ColorPickerPage : ContentPage
{
    private ColorPickerPageViewModel _viewModel;

    internal ColorPickerPageViewModel ViewModel
    {
        get => _viewModel;
        set { _viewModel = value; BindingContext = _viewModel; }
    }
    
    public ColorPickerPage()
    {
        InitializeComponent();

        ViewModel = new();
    }
    
    private async void OnCloseClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}

public class ColorPickerPageViewModel : ViewModel
{

    private Color _selectedColor;

    public Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            _selectedColor = value;
            NotifyPropertyChanged(nameof(SelectedColor));
        }
    }
    
    public ColorPickerPageViewModel()
    {
        SelectedColor = Colors.Red;
    }
}