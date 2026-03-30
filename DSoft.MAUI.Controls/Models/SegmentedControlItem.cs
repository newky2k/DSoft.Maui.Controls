namespace DSoft.Maui.Controls.Models;

public class SegmentedControlItem
{
    public string Text { get; set; } = string.Empty;
    
    public string IconSource { get; set; }
    public object Value { get; set; }

    public SegmentedControlItem() { }

    public SegmentedControlItem(string text, object value = null)
    {
        Text = text;
        Value = value ?? text;
    }
}