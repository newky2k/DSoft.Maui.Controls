using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace DSoft.Maui.Controls;

/// <summary>
/// Event args for row selection in DataGridView
/// </summary>
public class DataGridRowSelectedEventArgs : EventArgs
{
    /// <summary>The selected DataRow</summary>
    public DataRow Row { get; }

    /// <summary>Zero-based index of the selected row</summary>
    public int RowIndex { get; }

    public DataGridRowSelectedEventArgs(DataRow row, int rowIndex)
    {
        Row = row;
        RowIndex = rowIndex;
    }
}

/// <summary>
/// Event args raised when a column header is tapped to sort
/// </summary>
public class DataGridColumnSortedEventArgs : EventArgs
{
    /// <summary>Zero-based index of the sorted column</summary>
    public int ColumnIndex { get; }

    public DataGridColumnSortedEventArgs(int columnIndex)
    {
        ColumnIndex = columnIndex;
    }
}

/// <summary>
/// A MAUI DataGrid view backed by a System.Data.DataTable data source.
/// Supports column headers, alternating row colours, row selection and column sorting.
///
/// Column display metadata is stored in DataColumn.ExtendedProperties:
///   "Width"     – double: fixed column width in device-independent units (omit for proportional/Star)
///   "AllowSort" – bool:   whether the column header triggers sorting (defaults to true)
/// </summary>
public class DataGridView : ContentView
{
    #region Bindable Properties

    public static readonly BindableProperty DataSourceProperty =
        BindableProperty.Create(nameof(DataSource), typeof(DataTable), typeof(DataGridView), null,
            propertyChanged: OnDataSourceChanged);

    public static readonly BindableProperty RowHeightProperty =
        BindableProperty.Create(nameof(RowHeight), typeof(double), typeof(DataGridView), 44.0,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty HeaderHeightProperty =
        BindableProperty.Create(nameof(HeaderHeight), typeof(double), typeof(DataGridView), 44.0,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty HeaderBackgroundColorProperty =
        BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(DataGridView),
            Color.FromArgb("#CCCCCC"),
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty RowBackgroundColorProperty =
        BindableProperty.Create(nameof(RowBackgroundColor), typeof(Color), typeof(DataGridView), Colors.White,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty AlternateRowBackgroundColorProperty =
        BindableProperty.Create(nameof(AlternateRowBackgroundColor), typeof(Color), typeof(DataGridView),
            Color.FromArgb("#F2F2F2"),
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty SelectionColorProperty =
        BindableProperty.Create(nameof(SelectionColor), typeof(Color), typeof(DataGridView),
            Color.FromArgb("#ADD8E6"),
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty HeaderTextColorProperty =
        BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(DataGridView), Colors.Black,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty CellTextColorProperty =
        BindableProperty.Create(nameof(CellTextColor), typeof(Color), typeof(DataGridView), Colors.Black,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty SeparatorColorProperty =
        BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(DataGridView),
            Color.FromArgb("#DDDDDD"));

    public static readonly BindableProperty ShowColumnSeparatorsProperty =
        BindableProperty.Create(nameof(ShowColumnSeparators), typeof(bool), typeof(DataGridView), true);

    public static readonly BindableProperty HorizontalScrollEnabledProperty =
        BindableProperty.Create(nameof(HorizontalScrollEnabled), typeof(bool), typeof(DataGridView), false,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty DefaultColumnWidthProperty =
        BindableProperty.Create(nameof(DefaultColumnWidth), typeof(double), typeof(DataGridView), 120.0,
            propertyChanged: OnLayoutPropertyChanged);

    #endregion

    #region Public Properties

    /// <summary>The DataTable that provides columns and rows to the grid</summary>
    public DataTable DataSource
    {
        get => (DataTable)GetValue(DataSourceProperty);
        set => SetValue(DataSourceProperty, value);
    }

    /// <summary>Height of each data row in device-independent units</summary>
    public double RowHeight
    {
        get => (double)GetValue(RowHeightProperty);
        set => SetValue(RowHeightProperty, value);
    }

    /// <summary>Height of the column header row</summary>
    public double HeaderHeight
    {
        get => (double)GetValue(HeaderHeightProperty);
        set => SetValue(HeaderHeightProperty, value);
    }

    public Color HeaderBackgroundColor
    {
        get => (Color)GetValue(HeaderBackgroundColorProperty);
        set => SetValue(HeaderBackgroundColorProperty, value);
    }

    public Color RowBackgroundColor
    {
        get => (Color)GetValue(RowBackgroundColorProperty);
        set => SetValue(RowBackgroundColorProperty, value);
    }

    public Color AlternateRowBackgroundColor
    {
        get => (Color)GetValue(AlternateRowBackgroundColorProperty);
        set => SetValue(AlternateRowBackgroundColorProperty, value);
    }

    public Color SelectionColor
    {
        get => (Color)GetValue(SelectionColorProperty);
        set => SetValue(SelectionColorProperty, value);
    }

    public Color HeaderTextColor
    {
        get => (Color)GetValue(HeaderTextColorProperty);
        set => SetValue(HeaderTextColorProperty, value);
    }

    public Color CellTextColor
    {
        get => (Color)GetValue(CellTextColorProperty);
        set => SetValue(CellTextColorProperty, value);
    }

    /// <summary>Colour of the row separator line</summary>
    public Color SeparatorColor
    {
        get => (Color)GetValue(SeparatorColorProperty);
        set => SetValue(SeparatorColorProperty, value);
    }

    /// <summary>Whether to draw vertical separators between columns</summary>
    public bool ShowColumnSeparators
    {
        get => (bool)GetValue(ShowColumnSeparatorsProperty);
        set => SetValue(ShowColumnSeparatorsProperty, value);
    }

    /// <summary>
    /// When true the grid scrolls horizontally as well as vertically.
    /// The header and rows scroll together.
    /// </summary>
    public bool HorizontalScrollEnabled
    {
        get => (bool)GetValue(HorizontalScrollEnabledProperty);
        set => SetValue(HorizontalScrollEnabledProperty, value);
    }

    /// <summary>
    /// Width (in device-independent units) used for columns that have no explicit Width set
    /// when HorizontalScrollEnabled is true. Has no effect when horizontal scroll is disabled.
    /// </summary>
    public double DefaultColumnWidth
    {
        get => (double)GetValue(DefaultColumnWidthProperty);
        set => SetValue(DefaultColumnWidthProperty, value);
    }

    /// <summary>The currently selected DataRow, or null if nothing is selected</summary>
    public DataRow? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            RefreshSelection();
        }
    }

    #endregion

    #region Events

    /// <summary>Raised when the user taps a data row</summary>
    public event EventHandler<DataGridRowSelectedEventArgs>? RowSelected;

    /// <summary>Raised after the data has been sorted by a column header tap</summary>
    public event EventHandler<DataGridColumnSortedEventArgs>? ColumnSorted;

    #endregion

    #region Private Fields

    private readonly Grid _rootGrid;
    private readonly Grid _headerGrid;
    private readonly CollectionView _collectionView;
    private readonly ScrollView _horizontalScroll;

    private DataTable? _currentTable;
    private DataRow? _selectedItem;

    private ObservableCollection<DataRowViewModel>? _rows;

    private string? _sortColumnName;
    private bool _sortDescending;

    #endregion

    #region Constructor

    public DataGridView()
    {
        _rootGrid = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star }
            },
            RowSpacing = 0
        };

        _headerGrid = new Grid { ColumnSpacing = 0 };

        _collectionView = new CollectionView
        {
            SelectionMode = SelectionMode.None,
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = 0
            }
        };

        _rootGrid.Add(_headerGrid, 0, 0);
        _rootGrid.Add(_collectionView, 0, 1);

        _horizontalScroll = new ScrollView
        {
            Orientation = ScrollOrientation.Horizontal,
            Content = _rootGrid
        };

        Content = _rootGrid;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Clears the current selection without raising RowSelected.
    /// </summary>
    public void ClearSelection()
    {
        _selectedItem = null;
        RefreshSelection();
    }

    /// <summary>
    /// Rebuilds the grid from the current DataSource.
    /// Call this after modifying the DataSource's rows or columns at runtime.
    /// </summary>
    public void Reload()
    {
        _currentTable = DataSource;

        if (_currentTable == null)
        {
            _headerGrid.Children.Clear();
            _headerGrid.ColumnDefinitions.Clear();
            _collectionView.ItemTemplate = null;
            _collectionView.ItemsSource = null;
            _rows = null;
            return;
        }

        UpdateScrollWrapper();
        BuildHeader(_currentTable);
        BuildRows(_currentTable);
    }

    #endregion

    #region Private Methods

    private void UpdateScrollWrapper()
    {
        if (HorizontalScrollEnabled)
        {
            if (!ReferenceEquals(Content, _horizontalScroll))
            {
                _horizontalScroll.Content = _rootGrid;
                Content = _horizontalScroll;
            }
        }
        else
        {
            if (!ReferenceEquals(Content, _rootGrid))
                Content = _rootGrid;

            _rootGrid.WidthRequest = -1;
        }
    }

    private double ComputeContentWidth(DataTable table)
    {
        double total = 0;
        foreach (DataColumn col in table.Columns)
        {
            var w = GetColumnWidth(col);
            total += w > 0 ? w : DefaultColumnWidth;
        }
        return total;
    }

    /// <summary>
    /// Returns the GridLength for a column, taking HorizontalScrollEnabled into account.
    /// When horizontal scroll is active every column must have an absolute width so the
    /// total content width is well-defined in an unconstrained horizontal pass.
    /// </summary>
    private GridLength GetColumnGridLength(DataColumn col)
    {
        var w = GetColumnWidth(col);
        if (w > 0)
            return new GridLength(w, GridUnitType.Absolute);

        return HorizontalScrollEnabled
            ? new GridLength(DefaultColumnWidth, GridUnitType.Absolute)
            : GridLength.Star;
    }

    private static void OnDataSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (DataGridView)bindable;
        // Reset sort state when a new table is assigned
        view._sortColumnName = null;
        view._sortDescending = false;
        view.Reload();
    }

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((DataGridView)bindable).Reload();
    }

    private void BuildHeader(DataTable table)
    {
        _headerGrid.Children.Clear();
        _headerGrid.ColumnDefinitions.Clear();
        _headerGrid.BackgroundColor = HeaderBackgroundColor;
        _headerGrid.HeightRequest = HeaderHeight;
        _headerGrid.ColumnSpacing = 0;

        if (HorizontalScrollEnabled)
            _rootGrid.WidthRequest = ComputeContentWidth(table);
        else
            _rootGrid.WidthRequest = -1;

        for (int i = 0; i < table.Columns.Count; i++)
        {
            var col = table.Columns[i];

            _headerGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = GetColumnGridLength(col)
            });

            var headerLabel = new Label
            {
                Text = col.Caption ?? col.ColumnName,
                TextColor = HeaderTextColor,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,
                HorizontalTextAlignment = TextAlignment.Start,
                Padding = new Thickness(8, 0)
            };

            if (GetColumnAllowSort(col))
            {
                int colIndex = i;
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) => SortByColumn(colIndex);
                headerLabel.GestureRecognizers.Add(tap);
            }

            _headerGrid.Add(headerLabel, i, 0);

            // Column separator (skip for last column)
            if (ShowColumnSeparators && i < table.Columns.Count - 1)
            {
                var sep = new BoxView
                {
                    Color = SeparatorColor,
                    WidthRequest = 1,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Fill
                };
                _headerGrid.Add(sep, i, 0);
            }
        }
    }

    private void BuildRows(DataTable table)
    {
        _rows = new ObservableCollection<DataRowViewModel>();

        var view = table.DefaultView;
        for (int i = 0; i < view.Count; i++)
        {
            var dataRow = view[i].Row;
            var vm = new DataRowViewModel(dataRow, i);

            if (_selectedItem != null && dataRow == _selectedItem)
                vm.IsSelected = true;

            _rows.Add(vm);
        }

        _collectionView.ItemTemplate = CreateRowTemplate(table);
        _collectionView.ItemsSource = _rows;
    }

    private DataTemplate CreateRowTemplate(DataTable table)
    {
        var columnCount = table.Columns.Count;

        return new DataTemplate(() =>
        {
            // Build the inner column grid for cell content
            var rowGrid = new Grid { ColumnSpacing = 0, RowSpacing = 0 };
            var labels = new Label[columnCount];

            for (int i = 0; i < columnCount; i++)
            {
                var col = table.Columns[i];

                rowGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = GetColumnGridLength(col)
                });

                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Fill,
                    HorizontalTextAlignment = TextAlignment.Start,
                    Padding = new Thickness(8, 0),
                    TextColor = CellTextColor,
                    LineBreakMode = LineBreakMode.TailTruncation
                };

                rowGrid.Add(label, i, 0);
                labels[i] = label;

                // Column separator (skip for last column)
                if (ShowColumnSeparators && i < columnCount - 1)
                {
                    var sep = new BoxView
                    {
                        Color = SeparatorColor,
                        WidthRequest = 1,
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.Fill
                    };
                    rowGrid.Add(sep, i, 0);
                }
            }

            // Row separator at the bottom
            var rowSeparator = new BoxView
            {
                Color = SeparatorColor,
                HeightRequest = 1,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Fill
            };

            // Outer wrapper stacks content + separator vertically
            var outerGrid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Auto }
                },
                RowSpacing = 0
            };
            outerGrid.Add(rowGrid, 0, 0);
            outerGrid.Add(rowSeparator, 0, 1);

            var wrapper = new ContentView
            {
                Content = outerGrid,
                HeightRequest = RowHeight
            };

            var tapGesture = new TapGestureRecognizer();
            wrapper.GestureRecognizers.Add(tapGesture);

            // Track the currently bound view model so we can unsubscribe
            DataRowViewModel? currentVm = null;

            void UpdateBackground()
            {
                if (currentVm == null) return;
                wrapper.BackgroundColor = currentVm.IsSelected
                    ? SelectionColor
                    : (currentVm.RowIndex % 2 == 0 ? RowBackgroundColor : AlternateRowBackgroundColor);
            }

            void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == nameof(DataRowViewModel.IsSelected))
                    UpdateBackground();
            }

            wrapper.BindingContextChanged += (s, e) =>
            {
                // Unsubscribe previous vm
                if (currentVm != null)
                    currentVm.PropertyChanged -= OnVmPropertyChanged;

                currentVm = wrapper.BindingContext as DataRowViewModel;

                if (currentVm == null) return;

                // Populate cell text
                for (int i = 0; i < columnCount; i++)
                    labels[i].Text = currentVm.GetValue(table.Columns[i].ColumnName);

                // Update background
                UpdateBackground();

                // Wire selection tracking
                currentVm.PropertyChanged += OnVmPropertyChanged;

                // Wire tap
                tapGesture.Command = new Command(() =>
                {
                    if (currentVm != null)
                        SelectRow(currentVm);
                });
            };

            return wrapper;
        });
    }

    private void SelectRow(DataRowViewModel tappedVm)
    {
        _selectedItem = tappedVm.Row;

        if (_rows != null)
        {
            foreach (var vm in _rows)
                vm.IsSelected = vm == tappedVm;
        }

        RowSelected?.Invoke(this, new DataGridRowSelectedEventArgs(tappedVm.Row, tappedVm.RowIndex));
    }

    private void RefreshSelection()
    {
        if (_rows == null) return;

        foreach (var vm in _rows)
            vm.IsSelected = _selectedItem != null && vm.Row == _selectedItem;
    }

    private void SortByColumn(int columnIndex)
    {
        if (_currentTable == null) return;

        var col = _currentTable.Columns[columnIndex];

        if (col.ColumnName == _sortColumnName)
            _sortDescending = !_sortDescending;
        else
        {
            _sortColumnName = col.ColumnName;
            _sortDescending = false;
        }

        _currentTable.DefaultView.Sort = $"[{_sortColumnName}] {(_sortDescending ? "DESC" : "ASC")}";

        ColumnSorted?.Invoke(this, new DataGridColumnSortedEventArgs(columnIndex));

        BuildRows(_currentTable);
    }

    /// <summary>
    /// Returns the fixed column width stored in DataColumn.ExtendedProperties["Width"],
    /// or -1 if not set (caller should use Star sizing).
    /// </summary>
    private static double GetColumnWidth(DataColumn col)
    {
        if (col.ExtendedProperties.Contains("Width") && col.ExtendedProperties["Width"] is double w)
            return w;
        return -1;
    }

    /// <summary>
    /// Returns whether the column allows sorting. Reads DataColumn.ExtendedProperties["AllowSort"].
    /// Defaults to true if not set.
    /// </summary>
    private static bool GetColumnAllowSort(DataColumn col)
    {
        if (col.ExtendedProperties.Contains("AllowSort") && col.ExtendedProperties["AllowSort"] is bool b)
            return b;
        return true;
    }

    #endregion
}

/// <summary>
/// Internal view model wrapping a DataRow for use in the CollectionView
/// </summary>
internal sealed class DataRowViewModel : INotifyPropertyChanged
{
    private bool _isSelected;

    public DataRow Row { get; }
    public int RowIndex { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public DataRowViewModel(DataRow row, int rowIndex)
    {
        Row = row;
        RowIndex = rowIndex;
    }

    public string GetValue(string columnName) => Row[columnName]?.ToString() ?? string.Empty;
}