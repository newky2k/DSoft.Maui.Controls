namespace DSoft.Maui.Controls;

/// <summary>
/// Represents a single tab inside a <see cref="TabView"/>. Set <see cref="Title"/> for the tab
/// label and place any <see cref="View"/> as the direct child (content property) for the body.
/// </summary>
[ContentProperty(nameof(Content))]
public class TabItem : BindableObject
{
    #region Title

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(TabItem), string.Empty);

    /// <summary>Text shown in the segmented-control tab button.</summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    #endregion

    #region Content

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content), typeof(View), typeof(TabItem), null);

    /// <summary>The view displayed when this tab is selected.</summary>
    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    #endregion
}
