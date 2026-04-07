using Microsoft.Maui.Graphics;

namespace DSoft.Maui.Controls.Core.Models;

/// <summary>
/// Represents a single cell in a HeatmapChartView.
/// The colour to render is supplied directly rather than derived from a value scale.
/// </summary>
public class HeatmapCell
{
    /// <summary>Zero-based row index (top = 0).</summary>
    public int Row { get; set; }

    /// <summary>Zero-based column index (left = 0).</summary>
    public int Column { get; set; }

    /// <summary>Fill colour for this cell.</summary>
    public Color Color { get; set; }
}

