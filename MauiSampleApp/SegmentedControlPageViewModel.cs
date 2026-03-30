using System.Mvvm;
using System.Windows.Input;
using DSoft.Maui.Controls.Models;

namespace MauiSampleApp;

public class SegmentedControlPageViewModel : ViewModel
{
    private IList<SegmentedControlItem> _items;
    private int _selectedIndex;
    private SegmentedControlItem _selectedItem;

    public SegmentedControlItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            NotifyPropertyChanged(nameof(SelectedItem));
        }
    }
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            NotifyPropertyChanged(nameof(SelectedIndex));
        }
    }
    
    public IList<SegmentedControlItem> Items
    {
        get => _items;
        set
        {
            _items = value;
            NotifyPropertyChanged(nameof(Items));
        }
    }

    public ICommand ItemSelectedCommand { get; set; }
    
    public SegmentedControlPageViewModel()
    {
        Items = new List<SegmentedControlItem>()
        {
            new SegmentedControlItem()
            {
                Text = "Item 1",
            },
            new SegmentedControlItem()
            {
                Text = "Item 2",
            },
            new SegmentedControlItem()
            {
                Text = "Item 3",
            }
        };

        ItemSelectedCommand = new DelegateCommand(() =>
        {
            Console.WriteLine("Selected item");
        });
    }
}