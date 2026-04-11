using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DSoft.Maui.Controls.Core.Enums;
using DSoft.Maui.Controls.Events;

namespace DSoft.Maui.Controls;

/// <summary>
/// A tab container that uses a <see cref="SegmentedControl"/> as the tab bar and displays
/// the content of the selected <see cref="TabItem"/>. Add <see cref="TabItem"/> children
/// in XAML or code; the tab bar is built automatically from their <see cref="TabItem.Title"/> values.
/// </summary>
[ContentProperty(nameof(TabItems))]
public class TabView : ContentView
{
    #region Fields

    private readonly SegmentedControl _segmentedControl = new();
    private readonly Grid _contentGrid = new()
    {
        HorizontalOptions = LayoutOptions.Fill,
        VerticalOptions = LayoutOptions.Fill,
    };
    private readonly Grid _rootGrid = new();
    private bool _suppressSync;

    #endregion

    #region TabItems

    /// <summary>The collection of <see cref="TabItem"/> objects that make up this tab view.</summary>
    public ObservableCollection<TabItem> TabItems { get; } = new();

    #endregion

    #region Bindable Properties

    #region SelectedIndex

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
        nameof(SelectedIndex), typeof(int), typeof(TabView), 0, BindingMode.TwoWay,
        propertyChanged: OnSelectedIndexChanged);

    private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        => ((TabView)bindable).SyncSelection((int)newValue);

    /// <summary>The zero-based index of the currently selected tab. Supports two-way binding.</summary>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    #endregion

    #region SegmentedControlStyle

    public static readonly BindableProperty SegmentedControlStyleProperty = BindableProperty.Create(
        nameof(SegmentedControlStyle), typeof(Style), typeof(TabView), null,
        propertyChanged: OnSegmentedControlStyleChanged);

    private static void OnSegmentedControlStyleChanged(BindableObject bindable, object oldValue, object newValue)
        => ((TabView)bindable)._segmentedControl.Style = (Style)newValue;

    /// <summary>
    /// A MAUI <see cref="Style"/> targeting <see cref="SegmentedControl"/> applied to the internal
    /// tab bar. Use this to customise colours, font, height, etc. from outside the control.
    /// </summary>
    public Style SegmentedControlStyle
    {
        get => (Style)GetValue(SegmentedControlStyleProperty);
        set => SetValue(SegmentedControlStyleProperty, value);
    }

    #endregion

    #region TabPosition

    public static readonly BindableProperty TabPositionProperty = BindableProperty.Create(
        nameof(TabPosition), typeof(TabPosition), typeof(TabView), TabPosition.Top,
        propertyChanged: (b, _, _) => ((TabView)b).BuildLayout());

    /// <summary>Whether the tab bar appears above (<see cref="TabPosition.Top"/>, default) or below the content.</summary>
    public TabPosition TabPosition
    {
        get => (TabPosition)GetValue(TabPositionProperty);
        set => SetValue(TabPositionProperty, value);
    }

    #endregion

    #region TabSpacing

    public static readonly BindableProperty TabSpacingProperty = BindableProperty.Create(
        nameof(TabSpacing), typeof(double), typeof(TabView), 8.0,
        propertyChanged: (b, _, _) => ((TabView)b).BuildLayout());

    /// <summary>Gap in device-independent units between the tab bar and the content area. Default is 8.</summary>
    public double TabSpacing
    {
        get => (double)GetValue(TabSpacingProperty);
        set => SetValue(TabSpacingProperty, value);
    }

    #endregion

    #region IsTabBarVisible

    public static readonly BindableProperty IsTabBarVisibleProperty = BindableProperty.Create(
        nameof(IsTabBarVisible), typeof(bool), typeof(TabView), true,
        propertyChanged: (b, _, _) => ((TabView)b).UpdateTabBarVisibility());

    /// <summary>Gets or sets whether the segmented control tab bar is visible. Default is <c>true</c>.</summary>
    public bool IsTabBarVisible
    {
        get => (bool)GetValue(IsTabBarVisibleProperty);
        set => SetValue(IsTabBarVisibleProperty, value);
    }

    #endregion

    #endregion

    #region Events

    /// <summary>Raised when the user selects a different tab. The argument is the new selected index.</summary>
    public event EventHandler<int>? TabSelected;

    #endregion

    #region Constructor

    public TabView()
    {
        TabItems.CollectionChanged += OnTabItemsChanged;
        _segmentedControl.SegmentSelected += OnSegmentSelected;

        BuildLayout();
        Content = _rootGrid;
    }

    #endregion

    #region Layout

    private void BuildLayout()
    {
        _rootGrid.RowSpacing = TabSpacing;
        _rootGrid.RowDefinitions.Clear();
        _rootGrid.Children.Clear();

        if (TabPosition == TabPosition.Top)
        {
            _rootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            _rootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            Grid.SetRow(_segmentedControl, 0);
            Grid.SetRow(_contentGrid, 1);
        }
        else
        {
            _rootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            _rootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            Grid.SetRow(_contentGrid, 0);
            Grid.SetRow(_segmentedControl, 1);
        }

        _rootGrid.Children.Add(_segmentedControl);
        _rootGrid.Children.Add(_contentGrid);

        UpdateTabBarVisibility();
    }

    private void UpdateTabBarVisibility()
    {
        var visible = IsTabBarVisible;
        _segmentedControl.IsVisible = visible;

        // Collapse or restore the row that holds the segmented control
        var tabRow = TabPosition == TabPosition.Top ? 0 : 1;
        _rootGrid.RowDefinitions[tabRow] = new RowDefinition(visible ? GridLength.Auto : new GridLength(0));
        _rootGrid.RowSpacing = visible ? TabSpacing : 0;
    }

    #endregion

    #region Tab management

    private void OnTabItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => Rebuild();

    private void Rebuild()
    {
        _contentGrid.Children.Clear();

        _segmentedControl.ItemsSource = TabItems.Select(t => t.Title).ToList();

        var clampedIndex = TabItems.Count == 0 ? 0 : Math.Max(0, Math.Min(SelectedIndex, TabItems.Count - 1));

        for (int i = 0; i < TabItems.Count; i++)
        {
            var view = TabItems[i].Content;
            if (view == null) continue;
            view.IsVisible = i == clampedIndex;
            _contentGrid.Children.Add(view);
        }

        _suppressSync = true;
        _segmentedControl.SelectedIndex = clampedIndex;
        _suppressSync = false;
    }

    private void SyncSelection(int index)
    {
        if (_suppressSync) return;
        if (TabItems.Count == 0) return;

        index = Math.Max(0, Math.Min(index, TabItems.Count - 1));

        for (int i = 0; i < _contentGrid.Children.Count; i++)
            if (_contentGrid.Children[i] is VisualElement ve)
                ve.IsVisible = i == index;

        _suppressSync = true;
        _segmentedControl.SelectedIndex = index;
        _suppressSync = false;
    }

    private void OnSegmentSelected(object? sender, SegmentSelectedEventArgs e)
    {
        if (_suppressSync) return;

        _suppressSync = true;
        SelectedIndex = e.SelectedIndex;
        _suppressSync = false;

        SyncSelection(e.SelectedIndex);
        TabSelected?.Invoke(this, e.SelectedIndex);
    }

    #endregion
}
