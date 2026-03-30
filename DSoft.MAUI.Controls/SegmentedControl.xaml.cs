using System.Collections;
using System.Windows.Input;
using DSoft.Maui.Controls.Events;
using DSoft.Maui.Controls.Models;
using Microsoft.Maui.Controls.Shapes;

namespace DSoft.Maui.Controls;

public partial class SegmentedControl
{
    // -------------------------------------------------------------------------
    // Bindable Properties
    // -------------------------------------------------------------------------

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(SegmentedControl),
            propertyChanged: OnItemsSourceChanged);

    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(SegmentedControl), -1,
            BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SegmentedControl), null,
            BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

    public static readonly BindableProperty SelectedCommandProperty =
        BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(SegmentedControl));

    public static readonly BindableProperty TrackColorProperty =
        BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(SegmentedControl),
            Color.FromArgb("#E5E5EA"));

    public static readonly BindableProperty ThumbColorProperty =
        BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(SegmentedControl),
            Colors.White, propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    public static readonly BindableProperty SelectedTextColorProperty =
        BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(SegmentedControl),
            Colors.Black, propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    public static readonly BindableProperty UnselectedTextColorProperty =
        BindableProperty.Create(nameof(UnselectedTextColor), typeof(Color), typeof(SegmentedControl),
            Color.FromArgb("#3C3C43").WithAlpha(0.6f),
            propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SegmentedControl), 13.0,
            propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(SegmentedControl),
            FontAttributes.Bold, propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(SegmentedControl), 9.0);

    public static readonly BindableProperty ThumbCornerRadiusProperty =
        BindableProperty.Create(nameof(ThumbCornerRadius), typeof(double), typeof(SegmentedControl), 7.0,
            propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    public static readonly BindableProperty SegmentHeightProperty =
        BindableProperty.Create(nameof(SegmentHeight), typeof(double), typeof(SegmentedControl), 32.0,
            propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    public static readonly BindableProperty SegmentPaddingProperty =
        BindableProperty.Create(nameof(SegmentPadding), typeof(Thickness), typeof(SegmentedControl),
            new Thickness(2), propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SegmentedControl),
            Color.FromArgb("#C6C6C8"));

    public static readonly BindableProperty BorderThicknessProperty =
        BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(SegmentedControl), 0.5);

    public static readonly BindableProperty ThumbShadowProperty =
        BindableProperty.Create(nameof(ThumbShadow), typeof(bool), typeof(SegmentedControl), true,
            propertyChanged: (b, _, _) => ((SegmentedControl)b).RefreshSegments());

    // -------------------------------------------------------------------------
    // CLR wrappers
    // -------------------------------------------------------------------------

    public IList? ItemsSource
    {
        get => (IList?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public ICommand? SelectedCommand
    {
        get => (ICommand?)GetValue(SelectedCommandProperty);
        set => SetValue(SelectedCommandProperty, value);
    }

    public Color TrackColor
    {
        get => (Color)GetValue(TrackColorProperty);
        set => SetValue(TrackColorProperty, value);
    }

    public Color ThumbColor
    {
        get => (Color)GetValue(ThumbColorProperty);
        set => SetValue(ThumbColorProperty, value);
    }

    public Color SelectedTextColor
    {
        get => (Color)GetValue(SelectedTextColorProperty);
        set => SetValue(SelectedTextColorProperty, value);
    }

    public Color UnselectedTextColor
    {
        get => (Color)GetValue(UnselectedTextColorProperty);
        set => SetValue(UnselectedTextColorProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public double ThumbCornerRadius
    {
        get => (double)GetValue(ThumbCornerRadiusProperty);
        set => SetValue(ThumbCornerRadiusProperty, value);
    }

    public double SegmentHeight
    {
        get => (double)GetValue(SegmentHeightProperty);
        set => SetValue(SegmentHeightProperty, value);
    }

    public Thickness SegmentPadding
    {
        get => (Thickness)GetValue(SegmentPaddingProperty);
        set => SetValue(SegmentPaddingProperty, value);
    }

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public double BorderThickness
    {
        get => (double)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public bool ThumbShadow
    {
        get => (bool)GetValue(ThumbShadowProperty);
        set => SetValue(ThumbShadowProperty, value);
    }

    // -------------------------------------------------------------------------
    // Events
    // -------------------------------------------------------------------------

    public event EventHandler<SegmentSelectedEventArgs>? SegmentSelected;

    // -------------------------------------------------------------------------
    // Internal state
    // -------------------------------------------------------------------------

    private bool _suppressCallbacks;

    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------

    public SegmentedControl()
    {
        InitializeComponent();
    }

    // -------------------------------------------------------------------------
    // Property-changed handlers
    // -------------------------------------------------------------------------

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        => ((SegmentedControl)bindable).BuildSegments();

    private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ctrl = (SegmentedControl)bindable;
        if (ctrl._suppressCallbacks) return;

        var idx = (int)newValue;
        ctrl._suppressCallbacks = true;

        if (ctrl.ItemsSource != null && idx >= 0 && idx < ctrl.ItemsSource.Count)
            ctrl.SelectedItem = ctrl.ItemsSource[idx];

        ctrl._suppressCallbacks = false;
        ctrl.RefreshSegments();
    }

    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ctrl = (SegmentedControl)bindable;
        if (ctrl._suppressCallbacks) return;

        ctrl._suppressCallbacks = true;

        if (ctrl.ItemsSource != null && newValue != null)
        {
            int idx = ctrl.ItemsSource.IndexOf(newValue);
            if (idx >= 0) ctrl.SelectedIndex = idx;
        }

        ctrl._suppressCallbacks = false;
        ctrl.RefreshSegments();
    }

    // -------------------------------------------------------------------------
    // Build / refresh
    // -------------------------------------------------------------------------

    private void BuildSegments()
    {
        SegmentsGrid.Children.Clear();
        SegmentsGrid.ColumnDefinitions.Clear();

        if (ItemsSource == null || ItemsSource.Count == 0) return;

        int count = ItemsSource.Count;

        for (int i = 0; i < count; i++)
            SegmentsGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        for (int i = 0; i < count; i++)
        {
            var segment = BuildSegmentView(ItemsSource[i]!, i, i == SelectedIndex);
            Grid.SetColumn(segment, i);
            SegmentsGrid.Children.Add(segment);
        }
    }

    private void RefreshSegments() => BuildSegments();

    // -------------------------------------------------------------------------
    // Segment view factory
    // -------------------------------------------------------------------------

    private View BuildSegmentView(object item, int index, bool isSelected)
    {
        string text = item is SegmentedControlItem sci ? sci.Text : item.ToString() ?? string.Empty;
        string? icon = item is SegmentedControlItem sci2 ? sci2.IconSource : null;

        View content;

        if (!string.IsNullOrEmpty(icon))
        {
            content = new HorizontalStackLayout
            {
                Spacing = 4,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Image
                    {
                        Source = icon,
                        HeightRequest = FontSize + 2,
                        WidthRequest = FontSize + 2,
                        VerticalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = text,
                        TextColor = isSelected ? SelectedTextColor : UnselectedTextColor,
                        FontSize = FontSize,
                        FontAttributes = isSelected ? FontAttributes : FontAttributes.None,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    }
                }
            };
        }
        else
        {
            content = new Label
            {
                Text = text,
                TextColor = isSelected ? SelectedTextColor : UnselectedTextColor,
                FontSize = FontSize,
                FontAttributes = isSelected ? FontAttributes : FontAttributes.None,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
        }

        Shadow? shadow = null;
        if (isSelected && ThumbShadow)
        {
            shadow = new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Offset = new Point(0, 2),
                Radius = 4,
                Opacity = 0.15f
            };
        }

        var thumbBorder = new Border
        {
            BackgroundColor = isSelected ? ThumbColor : Colors.Transparent,
            StrokeThickness = 0,
            Padding = new Thickness(0),
            HeightRequest = SegmentHeight,
            Shadow = shadow,
            Content = content,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(ThumbCornerRadius) }
        };

        var wrapper = new Grid
        {
            Padding = SegmentPadding,
            Children = { thumbBorder }
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => OnSegmentTapped(index);
        wrapper.GestureRecognizers.Add(tap);

        return wrapper;
    }

    // -------------------------------------------------------------------------
    // Selection
    // -------------------------------------------------------------------------

    private void OnSegmentTapped(int index)
    {
        if (index == SelectedIndex) return;

        var previousIndex = SelectedIndex;
        var previousItem = SelectedItem;

        _suppressCallbacks = true;
        SelectedIndex = index;
        SelectedItem = ItemsSource?[index];
        _suppressCallbacks = false;

        RefreshSegments();

        SegmentSelected?.Invoke(this, new SegmentSelectedEventArgs(index, SelectedItem, previousIndex, previousItem));

        if (SelectedCommand?.CanExecute(SelectedItem) == true)
            SelectedCommand.Execute(SelectedItem);
    }
}
