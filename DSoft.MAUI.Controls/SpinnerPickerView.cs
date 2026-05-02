using System.Collections;
using System.Collections.Specialized;
using DSoft.Maui.Controls.Events;

namespace DSoft.Maui.Controls;

/// <summary>
/// A drum-roll / wheel-style picker. Scroll vertically to select a value.
/// The selected item is always centred; surrounding items fade and scale down.
/// Uses only MAUI primitives — no native elements.
/// </summary>
public class SpinnerPickerView : ContentView
{
    #region Fields

    private readonly VerticalStackLayout _itemsLayout;
    private readonly RowDefinition _centerRowDef;
    private readonly BoxView _topLine;
    private readonly BoxView _bottomLine;
    private readonly List<Label> _labels = new();
    private double _startTranslation;
    private bool _suppressCallbacks;

    #endregion

    #region ItemsSource

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(IList), typeof(SpinnerPickerView),
        null, propertyChanged: OnItemsSourceChanged);

    public IList? ItemsSource
    {
        get => (IList?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    #endregion

    #region SelectedIndex

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
        nameof(SelectedIndex), typeof(int), typeof(SpinnerPickerView),
        0, BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    #endregion

    #region SelectedItem

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem), typeof(object), typeof(SpinnerPickerView),
        null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    #endregion

    #region ItemHeight

    public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(
        nameof(ItemHeight), typeof(double), typeof(SpinnerPickerView),
        44.0, propertyChanged: OnLayoutChanged);

    public double ItemHeight
    {
        get => (double)GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    #endregion

    #region VisibleItemCount

    public static readonly BindableProperty VisibleItemCountProperty = BindableProperty.Create(
        nameof(VisibleItemCount), typeof(int), typeof(SpinnerPickerView),
        5, propertyChanged: OnLayoutChanged);

    /// <summary>
    /// Number of rows to show. Use an odd number for a clear centre selection.
    /// </summary>
    public int VisibleItemCount
    {
        get => (int)GetValue(VisibleItemCountProperty);
        set => SetValue(VisibleItemCountProperty, value);
    }

    #endregion

    #region TextColor

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor), typeof(Color), typeof(SpinnerPickerView),
        Colors.Gray, propertyChanged: OnStyleChanged);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    #endregion

    #region SelectedTextColor

    public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create(
        nameof(SelectedTextColor), typeof(Color), typeof(SpinnerPickerView),
        Colors.Black, propertyChanged: OnStyleChanged);

    public Color SelectedTextColor
    {
        get => (Color)GetValue(SelectedTextColorProperty);
        set => SetValue(SelectedTextColorProperty, value);
    }

    #endregion

    #region FontSize

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize), typeof(double), typeof(SpinnerPickerView),
        16.0, propertyChanged: OnStyleChanged);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    #endregion

    #region SelectorColor

    public static readonly BindableProperty SelectorColorProperty = BindableProperty.Create(
        nameof(SelectorColor), typeof(Color), typeof(SpinnerPickerView),
        Colors.LightGray, propertyChanged: OnSelectorColorChanged);

    public Color SelectorColor
    {
        get => (Color)GetValue(SelectorColorProperty);
        set => SetValue(SelectorColorProperty, value);
    }

    #endregion

    #region Events

    public event EventHandler<SpinnerSelectedEventArgs>? SelectionChanged;

    #endregion

    #region Constructor

    public SpinnerPickerView()
    {
        _itemsLayout = new VerticalStackLayout
        {
            Spacing = 0,
            VerticalOptions = LayoutOptions.Start,
        };

        _centerRowDef = new RowDefinition { Height = new GridLength(ItemHeight, GridUnitType.Absolute) };

        _topLine = new BoxView
        {
            Color = SelectorColor,
            HeightRequest = 1,
            VerticalOptions = LayoutOptions.End,
            InputTransparent = true,
        };

        _bottomLine = new BoxView
        {
            Color = SelectorColor,
            HeightRequest = 1,
            VerticalOptions = LayoutOptions.Start,
            InputTransparent = true,
        };

        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },
                _centerRowDef,
                new RowDefinition { Height = GridLength.Star },
            }
        };

        Grid.SetRow(_itemsLayout, 0);
        Grid.SetRowSpan(_itemsLayout, 3);
        grid.Add(_itemsLayout);

        Grid.SetRow(_topLine, 0);
        grid.Add(_topLine);

        Grid.SetRow(_bottomLine, 2);
        grid.Add(_bottomLine);

        var pan = new PanGestureRecognizer();
        pan.PanUpdated += OnPanUpdated;
        grid.GestureRecognizers.Add(pan);

        grid.IsClippedToBounds = true;
        Content = grid;
        HeightRequest = ItemHeight * VisibleItemCount;
    }

    #endregion

    #region Methods

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _startTranslation = _itemsLayout.TranslationY;
                this.AbortAnimation("Snap");
                break;

            case GestureStatus.Running:
                _itemsLayout.TranslationY = ClampTranslation(_startTranslation + e.TotalY);
                RefreshItemTransforms();
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                SnapToNearestItem();
                break;
        }
    }

    private double ClampTranslation(double y)
    {
        if (_labels.Count == 0) return y;
        return Math.Clamp(y, CenterOffsetForIndex(_labels.Count - 1), CenterOffsetForIndex(0));
    }

    // TranslationY value that places item[index] in the vertical centre of the control.
    // Derivation: item centre = (index + 0.5) * ItemHeight + TranslationY == ControlHeight / 2
    private double CenterOffsetForIndex(int index)
        => ItemHeight * ((VisibleItemCount - 1) / 2.0 - index);

    private async void SnapToNearestItem()
    {
        if (_labels.Count == 0) return;

        var rawIndex = (VisibleItemCount - 1) / 2.0 - _itemsLayout.TranslationY / ItemHeight;
        var index = Math.Clamp((int)Math.Round(rawIndex), 0, _labels.Count - 1);

        await AnimateToIndex(index);

        if (index == SelectedIndex && Equals(ItemsSource?[index], SelectedItem))
            return;

        var previousIndex = SelectedIndex;
        var previousItem = SelectedItem;

        _suppressCallbacks = true;
        SelectedIndex = index;
        SelectedItem = ItemsSource?[index];
        _suppressCallbacks = false;

        SelectionChanged?.Invoke(this, new SpinnerSelectedEventArgs(index, SelectedItem, previousIndex, previousItem));
    }

    private Task AnimateToIndex(int index)
    {
        var startY = _itemsLayout.TranslationY;
        var targetY = CenterOffsetForIndex(index);

        this.AbortAnimation("Snap");

        var tcs = new TaskCompletionSource<bool>();

        new Animation(v =>
        {
            _itemsLayout.TranslationY = v;
            RefreshItemTransforms();
        }, startY, targetY, Easing.SpringOut)
        .Commit(this, "Snap", 16, 300, finished: (_, cancelled) =>
        {
            if (!cancelled)
            {
                _itemsLayout.TranslationY = targetY;
                RefreshItemTransforms();
            }
            tcs.TrySetResult(true);
        });

        return tcs.Task;
    }

    private void RefreshItemTransforms()
    {
        if (_labels.Count == 0) return;

        var controlCenterY = ItemHeight * VisibleItemCount / 2.0;

        for (var i = 0; i < _labels.Count; i++)
        {
            var label = _labels[i];
            var itemCenterY = (i + 0.5) * ItemHeight + _itemsLayout.TranslationY;
            var distance = Math.Abs(itemCenterY - controlCenterY) / ItemHeight;

            label.Opacity = Math.Max(0.2, 1.0 - distance * 0.5);
            label.Scale = Math.Max(0.7, 1.0 - distance * 0.15);

            if (distance < 0.5)
            {
                label.TextColor = SelectedTextColor;
                label.FontAttributes = FontAttributes.Bold;
            }
            else
            {
                label.TextColor = TextColor;
                label.FontAttributes = FontAttributes.None;
            }
        }
    }

    private void BuildItems()
    {
        _labels.Clear();
        _itemsLayout.Children.Clear();

        if (ItemsSource == null) return;

        foreach (var item in ItemsSource)
        {
            var label = new Label
            {
                Text = item?.ToString() ?? string.Empty,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = ItemHeight,
                FontSize = FontSize,
                TextColor = TextColor,
            };
            _labels.Add(label);
            _itemsLayout.Children.Add(label);
        }

        var safeIndex = Math.Clamp(SelectedIndex, 0, Math.Max(0, _labels.Count - 1));
        _itemsLayout.TranslationY = CenterOffsetForIndex(safeIndex);
        RefreshItemTransforms();
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SpinnerPickerView)bindable;

        if (oldValue is INotifyCollectionChanged oldCollection)
            oldCollection.CollectionChanged -= control.OnCollectionChanged;

        if (newValue is INotifyCollectionChanged newCollection)
            newCollection.CollectionChanged += control.OnCollectionChanged;

        control.BuildItems();
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => MainThread.BeginInvokeOnMainThread(BuildItems);

    private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SpinnerPickerView)bindable;
        if (control._suppressCallbacks || control._labels.Count == 0) return;

        var index = Math.Clamp((int)newValue, 0, control._labels.Count - 1);

        control._suppressCallbacks = true;
        control.SelectedItem = control.ItemsSource?[index];
        control._suppressCallbacks = false;

        _ = control.AnimateToIndex(index);
    }

    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SpinnerPickerView)bindable;
        if (control._suppressCallbacks || control.ItemsSource == null) return;

        for (var i = 0; i < control.ItemsSource.Count; i++)
        {
            if (!Equals(control.ItemsSource[i], newValue)) continue;

            control._suppressCallbacks = true;
            control.SelectedIndex = i;
            control._suppressCallbacks = false;

            _ = control.AnimateToIndex(i);
            return;
        }
    }

    private static void OnLayoutChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SpinnerPickerView)bindable;
        control._centerRowDef.Height = new GridLength(control.ItemHeight, GridUnitType.Absolute);
        control.HeightRequest = control.ItemHeight * control.VisibleItemCount;
        control.BuildItems();
    }

    private static void OnStyleChanged(BindableObject bindable, object oldValue, object newValue)
        => ((SpinnerPickerView)bindable).RefreshItemTransforms();

    private static void OnSelectorColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SpinnerPickerView)bindable;
        var color = (Color)newValue;
        control._topLine.Color = color;
        control._bottomLine.Color = color;
    }

    #endregion
}
