namespace DSoft.Maui.Controls.Events;

// -------------------------------------------------------------------------
// Event args
// -------------------------------------------------------------------------

public class SegmentSelectedEventArgs(int selectedIndex, object selectedItem, int previousIndex, object previousItem)
    : EventArgs
{
    public int SelectedIndex { get; } = selectedIndex;
    public object SelectedItem { get; } = selectedItem;
    public int PreviousIndex { get; } = previousIndex;
    public object PreviousItem { get; } = previousItem;
}