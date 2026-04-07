using System.Mvvm;
using DSoft.Maui.Controls.Models;

namespace MauiSampleApp;

public class HeatMapPageViewModel : ViewModel
{
    private List<string> _xLabels;
    private List<string> _yLabels;
    private List<HeatmapCell>  _cells;

    public List<string> XLabels
    {
        get => _xLabels;
        set
        {
            _xLabels = value;
            NotifyPropertyChanged(nameof(XLabels));
        }
    }
    
    public List<string> YLabels
    {
        get => _yLabels;
        set
        {
            _yLabels = value;
            NotifyPropertyChanged(nameof(YLabels));
        }
    }
    
    public List<HeatmapCell>  Cells
    {
        get => _cells;
        set
        {
            _cells = value;
            NotifyPropertyChanged(nameof(Cells));
        }
    }
    
    public HeatMapPageViewModel()
    {
        XLabels = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];

        YLabels = ["6am", "9am", "12pm", "3pm", "6pm", "9pm"];
        
        Cells =
        [
            new HeatmapCell { Row = 0, Column = 0, Color = Color.FromArgb("#C8E6C9") },
            new HeatmapCell { Row = 1, Column = 0, Color = Color.FromArgb("#388E3C") },
            new HeatmapCell { Row = 2, Column = 0, Color = Color.FromArgb("#1B5E20") },
            new HeatmapCell { Row = 3, Column = 0, Color = Color.FromArgb("#388E3C") },
            new HeatmapCell { Row = 4, Column = 0, Color = Color.FromArgb("#C8E6C9") },
            new HeatmapCell { Row = 5, Column = 0, Color = Color.FromArgb("#FFFFFF") },

            // Tuesday
            new HeatmapCell { Row = 0, Column = 1, Color = Color.FromArgb("#FFFFFF") },
            new HeatmapCell { Row = 1, Column = 1, Color = Color.FromArgb("#C8E6C9") },
            new HeatmapCell { Row = 2, Column = 1, Color = Color.FromArgb("#388E3C") },
            new HeatmapCell { Row = 3, Column = 1, Color = Color.FromArgb("#1B5E20") },
            new HeatmapCell { Row = 4, Column = 1, Color = Color.FromArgb("#388E3C") },
            new HeatmapCell { Row = 5, Column = 1, Color = Color.FromArgb("#C8E6C9") }

            // ... remaining days follow the same pattern
        ];
    }
}