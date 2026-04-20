# DSoft.Maui.Controls

Controls and Views for .NET MAUI, mainly ported from DSoft.XamarinForms.Controls.

Features:

- `BubbleView`
    - Bubble View for notification counts etc
- `RepeaterView`
  - Bindable Vertical Stack layout 
- `ColorPickView`
  - Grid of selectable color dots
- `ColorWheelView`
  - Color selection wheel
- `GradientFrame`
- `SimpleRadialGaugeView`
  - Simple radial gauge chart with center view content
- `SimpleDonutGaugeView`
  - Simple donut gauge chart with center view content
- `RingChartView`
  - Bindable multi-ring chart with center view content
- `SingleRingChartView`
  - Simple single ring chart view with center view content
- `SelectableContentView`
    - Content View with bindable properties for use in a CollectionView allow for backgroup and text color updates
- `WizardView`
    - Bindable Wizard view for wizard dialogs
- `SegmentedControl`
    - MAUI Segmented Control
- `HeatmapChartView`
    - Grid-based heatmap chart with X/Y axis labels
- `SignaturePadView`
    - Freehand signature capture with configurable ink, stroke width and background; exports to PNG or JPEG at any size
- `TabView`
    - Tab container using `SegmentedControl` as the tab bar; supports top/bottom tab position, two-way `SelectedIndex` binding, external styling via `SegmentedControlStyle`, and hiding the tab bar via `IsTabBarVisible`
- `DataGridView`
    - `System.Data.DataTable`-backed data grid with column headers, alternating row colours, row selection, column sorting, and optional horizontal scrolling

This packages also contains `PanPinchContainer` based on `PanPinchContainer` by [CodingOctocat](https://github.com/CodingOctocat/MauiPanPinchContainer)

 - `PanPinchContainer`
    - Zoom in and out and pan around an image 

**All views are 100% MAUI and do not rely on platform implementations.**

 ###### This library has been tested on iOS and Android, but should also work on other platforms

 **NuGet**

|Name|Info|
| ------------------- | ------------------- | 
|DSoft.Maui.Controls|[![NuGet](https://img.shields.io/nuget/v/DSoft.Maui.Controls.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Maui.Controls)|

# Setup

## Core library
The core library with basic models and non-UI code for MVVM and dependency Injection

Add the package from Nuget, either from NuGet Package Manager or command line

        dotnet add package DSoft.Maui.Controls.Core

## Controls
The main UI controls library contains the Views, Behaviours and controls to use in your MAUI project.

Add the package from Nuget, either from NuGet Package Manager or command line

        dotnet add package DSoft.Maui.Controls

DSoft.Maui.Controls is a MAUI library so requires no dependency injection calls to initialise.  You can jump straight in.

## Usage

You will need to add a namespace reference to your xaml file

```xaml
  xmlns:mauic="http://dsoft.maui/schemas/controls"  
```
## BubbleView

![BubbleView](https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/BubbleView.png)

`BubbleView` is a simple control for showing notification counts.  The current API supports.

   - BubbleColor (background)
   - TextColor
   - Text
   - HasShadow
   - BorderColor

```xaml
  xmlns:mauic="http://dsoft.maui/schemas/controls" 
  
    <VerticalStackLayout>
        <mauic:BubbleView Margin="0,10,0,0" Text="20" BorderColor="White"  />
        <mauic:BubbleView Margin="0,10,0,0" Text="200" />
        <mauic:BubbleView Margin="0,10,0,20" Text="999999" HasShadow="False"  />
    <VerticalStackLayout> 
```

**Note: the API will be extended in upcoming versions**

## RepeaterView

![RepeaterView](https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/RepeaterView.png)

`RepeaterView` is bindable VerticalStackLayout subclass making it simple and quick to display repeated data.  Use `ItemSource` to  bind to the collection you wish to repeat

```xaml
  xmlns:mauic="http://dsoft.maui/schemas/controls" 
  
    <VerticalStackLayout>
        <mauic:RepeaterView x:Name="RepeaterView" Margin="10, 10,10,10" ItemsSource="{Binding Data}" BackgroundColor="Transparent">
            <mauic:RepeaterView.ItemTemplate>
                <DataTemplate>
                    <Grid HorizontalOptions="Fill" VerticalOptions="Start" Margin="10,0,10,5">
                        <Grid.RowDefinitions>
                            <RowDefinition Height="16"/>
                            <RowDefinition Height="22"/>
                        </Grid.RowDefinitions>
                        <Label Grid.Row="0" Text="{Binding Label}" TextColor="Black" FontAttributes="Bold" FontSize="12" />
                        <Label Grid.Row="1" Text="{Binding Percent}" TextColor="Grey" Margin="0,-5,0,3" FontSize="16"/>
                        <BoxView HeightRequest="1"
                                 BackgroundColor="LightGray"
                                 Grid.Row="1" Margin="0,0,0,0"
                                 VerticalOptions="End"/>
                    </Grid>
                </DataTemplate>
            </mauic:RepeaterView.ItemTemplate>
        </mauic:RepeaterView>
    <VerticalStackLayout> 
```

## SimpleRadialGaugeView

![SimpleRadialGuageView](https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/SimpleRadialGuageView.png)

`SimpleRadialGaugeView` is a simple radial guage chart with center view content

```xaml
  xmlns:mauic="http://dsoft.maui/schemas/controls" 
  
    <VerticalStackLayout>
        <controls:SimpleRadialGaugeView WidthRequest="200" Margin="0,10,0,0"
                                        HeightRequest="200"
                                        Percent="10"
                                        ScaleBackgroundColor="Pink"
                                        ScaleForegroundColor="Red"
                                        ScaleBackgroundLineWidth="5"
                                        ScaleForegroundLineWidth="5">
            <controls:SimpleRadialGaugeView.CenterView>
                <Grid BackgroundColor="Transparent" VerticalOptions="Center" HorizontalOptions="Center">
                    <Label Text="10%" FontSize="64"  />
                </Grid>
            </controls:SimpleRadialGaugeView.CenterView>
        </controls:SimpleRadialGaugeView>
    <VerticalStackLayout> 
```

## SimpleDonutGaugeView

![SimpleDonutGuageView](https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/SimpleDonutGuageView.png)

`SimpleDonutGaugeView`is a simple donut gauge chart with center view content

```xaml
  xmlns:mauic="http://dsoft.maui/schemas/controls" 
  
    <VerticalStackLayout>
        <controls:SimpleDonutGaugeView WidthRequest="200" Margin="0,10,0,0"
                                        HeightRequest="200"
                                        Percent="10"
                                        ScaleBackgroundColor="LightBlue"
                                        ScaleForegroundColor="DarkSlateBlue"
                                        ScaleBackgroundLineWidth="20"
                                        ScaleForegroundLineWidth="20">
            <controls:SimpleDonutGaugeView.CenterView>
                <Grid BackgroundColor="Transparent" VerticalOptions="Center" HorizontalOptions="Center">
                    <Label Text="10%" FontSize="64"  />
                </Grid>
            </controls:SimpleDonutGaugeView.CenterView>
        </controls:SimpleDonutGaugeView>
    <VerticalStackLayout> 
```

## RingChartView

![RingChartView](https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/RingChartView.png)

`RingChartView` is a bindable multi-ring chart with center view content. Use `ItemSource` to  bind to the collection of data.  The data must provide the following properties

```csharp
public double Percent { get; set; }

public double Value { get; set; }

public string Label { get; set; }

public Color? Color { get; set; }
```
**Note: The API will be updated to make the binding easier in a future version**

```xaml
  xmlns:mauic="http://dsoft.maui/schemas/controls" 
  
    <VerticalStackLayout>
        <controls:RingChartView ItemsSource="{Binding Data}" 
                                                    RingLineWidth="12" 
                                                    HasDropShadow="False" 
                                                    DropShadowDepth="4" 
                                                    WidthRequest="300" 
                                                    HeightRequest="300" 
                                                    Margin="0,10,0,0">
              <controls:RingChartView.CenterView>
                  <Grid BackgroundColor="Transparent" VerticalOptions="Center" HorizontalOptions="Center">
                      <VerticalStackLayout HorizontalOptions="Center" VerticalOptions="Center">
                          <Label Text="3.2/5.0" FontSize="64" />
                          <Label Text="Air Quality" FontSize="32" HorizontalTextAlignment="Center" />
                      </VerticalStackLayout>
                  </Grid>
              </controls:RingChartView.CenterView>
        </controls:RingChartView>
    <VerticalStackLayout> 
```

## SingleRingChartView

![SingleRingChartView](https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/SingleRingChartView.png)

`SingleRingChartView` is a simple single ring chart view with center view content

```xaml
  xmlns:mauic="http://dsoft.maui/schemas/controls" 
  
    <VerticalStackLayout>
        <controls:SimpleDonutGaugeView WidthRequest="200" Margin="0,10,0,0"
                                        HeightRequest="200"
                                        Percent="10"
                                        ScaleBackgroundColor="LightBlue"
                                        ScaleForegroundColor="DarkSlateBlue"
                                        ScaleBackgroundLineWidth="20"
                                        ScaleForegroundLineWidth="20">
            <controls:SimpleDonutGaugeView.CenterView>
                <Grid BackgroundColor="Transparent" VerticalOptions="Center" HorizontalOptions="Center">
                    <Label Text="10%" FontSize="64"  />
                </Grid>
            </controls:SimpleDonutGaugeView.CenterView>
        </controls:SimpleDonutGaugeView>
    <VerticalStackLayout> 
```

## PanPinchContainer (from Author)
I recently developed a MAUI app and needed a control that would allow the user to view an Image, like an Android/iOS photo album, I tried [Bertuzzi.MAUI.PinchZoomImage](https://github.com/TBertuzzi/Bertuzzi.MAUI.PinchZoomImage ), but it had some UX issues, then I tried reading the documentation [.NET MAUI Docs/Recognize a pangesture ](https://learn.microsoft.com/zh-cn/dotnet/maui/fundamentals/gestures/pan), [ .NET MAUI Docs/Recognize a pinch gesture](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/gestures/pinch ), After a few days of lots of attempts, I finally implemented the MauiPanPinchContainer!

Honestly, the code is all mathematical calculations, and I don't fully understand it, so if I could, I'd like to see in the code `Contnet.Anchor` to stay at the default value of 0.5 (I'm not sure if 0.5 is better), but I'm limited in my ability/time to do that for now.

### Usage
There are no plans to release a NuGet version for the time being, as PanPinchContainer is still very simple in its functionality, with hard-coded parameters, lack of flexibility, and no customizable dependency properties

```xaml
<mauic:PanPinchContainer>
    <Image Source="dotnet_bot.png" />
</mauic:PanPinchContainer> 
```

### Features
- 1x ~ 10x Scaling: Supports scaling from 1x to 10x.
- 0.5x Temporarily Scaling: Supports pinch and temporarily scale down the image below 1x. Upon release, the image restores to 1x.

### Supported
- Boundary Constraints: Limits scaling and panning within image boundaries.
- Double Tap to Zoom: Double tap to zoom in (2x) or zoom out (1x).
- Scaling Based on Pinch Position: Scale based on the position of the pinch gesture.
- Panning and Zooming Animation: Smooth panning and zooming animations.

### Not Supported
- Rotation
- Inertial panning
- Slightly more than 10x temporarily scaling

# Segmented Control

![Segmented Control](https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/SegmentedControl.png)

A fully native-free segmented control for .NET MAUI. Built entirely from MAUI primitives (`Border`, `Grid`, `Label`, `TapGestureRecognizer`) with no platform-specific views, handlers, or renderers — so it works identically on iOS, Android, Windows, and macOS without any per-platform code paths.

## Basic Usage

### String items (simplest form)

```xml
<controls:SegmentedControl
    SelectedIndex="0"
    SegmentSelected="OnSegmentSelected">
    <controls:SegmentedControl.ItemsSource>
        <x:Array Type="{x:Type x:String}">
            <x:String>Day</x:String>
            <x:String>Week</x:String>
            <x:String>Month</x:String>
        </x:Array>
    </controls:SegmentedControl.ItemsSource>
</controls:SegmentedControl>
```

### Code-behind event handler

```csharp
private void OnSegmentSelected(object sender, SegmentSelectedEventArgs e)
{
    Console.WriteLine($"Selected: {e.SelectedItem} (index {e.SelectedIndex})");
    Console.WriteLine($"Previously: {e.PreviousItem} (index {e.PreviousIndex})");
}
```

---

## MVVM / Data Binding

`SelectedIndex` and `SelectedItem` are both two-way bindable. You only need to bind one — they stay in sync automatically.

```xml
<controls:SegmentedControl
    ItemsSource="{Binding Tabs}"
    SelectedIndex="{Binding SelectedTabIndex}"
    SelectedCommand="{Binding TabChangedCommand}" />
```

```csharp
// ViewModel
public ObservableCollection<string> Tabs { get; } = ["Map", "Transit", "Satellite"];

[ObservableProperty]
private int _selectedTabIndex = 0;

[RelayCommand]
private void TabChanged(object selectedItem)
{
    // selectedItem is the string or SegmentedControlItem that was tapped
}
```

---

## SegmentedControlItem

For richer segments — an icon alongside the label, or a typed value payload separate from the display text — use `SegmentedControlItem` instead of plain strings.

```xml
<controls:SegmentedControl SelectedIndex="0">
    <controls:SegmentedControl.ItemsSource>
        <x:Array Type="{x:Type controls:SegmentedControlItem}">
            <controls:SegmentedControlItem Text="Map"     IconSource="icon_map.png"     Value="map" />
            <controls:SegmentedControlItem Text="Transit" IconSource="icon_transit.png" Value="transit" />
            <controls:SegmentedControlItem Text="Satellite"                             Value="satellite" />
        </x:Array>
    </controls:SegmentedControl.ItemsSource>
</controls:SegmentedControl>
```

The `Value` property lets you carry a domain object (an enum, a route key, a view model) independently of the display text. It is passed as `SelectedItem` and as the `ICommand` parameter when a segment is tapped.

---

## Styling

All visual properties are bindable and update live — no rebuild required.

### Dark theme

```xml
<controls:SegmentedControl
    TrackColor="#1C1C1E"
    ThumbColor="#2C2C2E"
    SelectedTextColor="White"
    UnselectedTextColor="#8E8E93"
    BorderColor="#38383A"
    BorderThickness="0"
    CornerRadius="10"
    ThumbCornerRadius="8" />
```

### Tinted accent (e.g. a blue filter bar)

```xml
<controls:SegmentedControl
    TrackColor="#E3EEFF"
    ThumbColor="#007AFF"
    SelectedTextColor="White"
    UnselectedTextColor="#007AFF"
    BorderColor="#C5D8FF"
    CornerRadius="10"
    ThumbCornerRadius="8" />
```

### Pill shape

```xml
<controls:SegmentedControl
    CornerRadius="20"
    ThumbCornerRadius="18"
    SegmentHeight="38"
    SegmentPadding="3" />
```

### Disabling the thumb shadow

```xml
<controls:SegmentedControl ThumbShadow="False" />
```

---

## Bindable Properties Reference

### Data

| Property | Type | Default | Description |
|---|---|---|---|
| `ItemsSource` | `IList` | `null` | Any list of `string` or `SegmentedControlItem`. Bound or set inline. |
| `SelectedIndex` | `int` | `-1` | Zero-based index of the selected segment. Two-way bindable. |
| `SelectedItem` | `object?` | `null` | The selected list item (or its `Value` if using `SegmentedControlItem`). Two-way bindable. Stays in sync with `SelectedIndex` automatically. |
| `SelectedCommand` | `ICommand?` | `null` | Executed on tap. The `SelectedItem` (or `Value`) is passed as the command parameter. |

### Layout

| Property | Type | Default | Description |
|---|---|---|---|
| `SegmentHeight` | `double` | `32` | Height of each segment (the thumb area). The overall control height is this value plus vertical `SegmentPadding`. |
| `SegmentPadding` | `Thickness` | `2` | Padding between the outer track border and the thumb. Increasing this creates the inset "pill inside a pill" look. |
| `CornerRadius` | `double` | `9` | Corner radius of the outer track. |
| `ThumbCornerRadius` | `double` | `7` | Corner radius of the selected thumb. Should be `CornerRadius − SegmentPadding` for a concentric look. |

### Colours

| Property | Type | Default | Description |
|---|---|---|---|
| `TrackColor` | `Color` | `#E5E5EA` | Background of the control track. |
| `ThumbColor` | `Color` | `White` | Fill colour of the selected segment thumb. |
| `SelectedTextColor` | `Color` | `Black` | Text colour for the selected segment. |
| `UnselectedTextColor` | `Color` | `#3C3C43` @ 60% | Text colour for unselected segments. |
| `BorderColor` | `Color` | `#C6C6C8` | Stroke colour of the outer track border. |
| `BorderThickness` | `double` | `0.5` | Stroke thickness of the outer track border. Set to `0` to remove. |

### Typography

| Property | Type | Default | Description |
|---|---|---|---|
| `FontSize` | `double` | `13` | Font size applied to all segment labels. Also scales icon size when using `SegmentedControlItem.IconSource`. |
| `FontAttributes` | `FontAttributes` | `Bold` | Font attributes applied to the **selected** segment label. Unselected segments always use `FontAttributes.None`. |

### Effects

| Property | Type | Default | Description |
|---|---|---|---|
| `ThumbShadow` | `bool` | `true` | When `true`, renders a subtle drop shadow beneath the selected thumb (black, 15% opacity, 2px Y offset, 4px radius). |

---

## Events

### `SegmentSelected`

Raised immediately after the selected segment changes, before `SelectedCommand` fires.

```csharp
public event EventHandler<SegmentSelectedEventArgs>? SegmentSelected;
```

`SegmentSelectedEventArgs` exposes:

| Property | Type | Description |
|---|---|---|
| `SelectedIndex` | `int` | Index of the newly selected segment. |
| `SelectedItem` | `object?` | The item at `SelectedIndex` (or its `Value` if using `SegmentedControlItem`). |
| `PreviousIndex` | `int` | Index of the previously selected segment. |
| `PreviousItem` | `object?` | The previously selected item. |

---

## How It Works

The control is a `ContentView` wrapping a single `Border` (the track). Segments are built at runtime as a column-equal `Grid` of `Border` + `Label` pairs — one for each item in `ItemsSource`. The selected segment's `Border` receives the `ThumbColor` background and an optional `Shadow`; unselected segments are transparent. A `TapGestureRecognizer` on each cell drives selection.

Because everything is a standard MAUI layout element, the control inherits all the usual benefits: it respects `AppThemeBinding`, works inside `ScrollView`, `CollectionView`, `Shell`, and any other container, and requires no custom handler registration in `MauiProgram`.

### `SelectedIndex` / `SelectedItem` sync

The two selection properties are kept in sync via a `_suppressCallbacks` guard that prevents re-entrancy. Setting either one from a binding or from code will automatically update the other and redraw the affected segments.

---

## Limitations

- **No animation.** The thumb swaps instantly on tap. A sliding animation would require measuring absolute positions and is left as a future enhancement.
- **Equal-width segments only.** All segments share a `GridLength.Star` column definition. Variable-width segments are not currently supported.
- **Single selection only.** Multi-select is not supported by design; use a set of `CheckBox` or `ToggleButton` controls for that pattern.
- **`IList` not `INotifyCollectionChanged`.** Replacing `ItemsSource` with a new list rebuilds the segments correctly, but mutating an existing `ObservableCollection` after binding will not auto-refresh. Reassign the property to trigger a rebuild.

---

# HeatmapChartView

![Heatmap Chart View](https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/heatmapview.png)

`HeatmapChartView` is a SkiaSharp-based heatmap grid. Each cell supplies its own colour directly — no colour-stop interpolation is performed by the control. Row and column counts are derived from the `YLabels` and `XLabels` collections respectively.

## Basic Usage

```xaml
xmlns:controls="http://dsoft.maui/schemas/controls"

<controls:HeatmapChartView
    XLabels="{Binding XLabels}"
    YLabels="{Binding YLabels}"
    Cells="{Binding Cells}"
    HeightRequest="300"
    HorizontalOptions="Fill"
    CellSpacing="4"
    LabelFontSize="12"
    LabelColor="Black"
    ShowGridLines="True"
    EmptyCellColor="Transparent"
    XLabelRotation="0" />
```

## HeatmapCell

Each cell in the `Cells` collection is a `HeatmapCell`:

```csharp
public class HeatmapCell
{
    /// <summary>Zero-based row index (top = 0).</summary>
    public int Row { get; set; }

    /// <summary>Zero-based column index (left = 0).</summary>
    public int Column { get; set; }

    /// <summary>Fill colour for this cell.</summary>
    public Color Color { get; set; }
}
```

Grid positions with no matching cell are filled with `EmptyCellColor` (transparent by default).

## Bindable Properties Reference

| Property | Type | Default | Description |
|---|---|---|---|
| `XLabels` | `IList<string>` | `null` | Labels shown along the bottom axis — one per column. The column count equals `XLabels.Count`. |
| `YLabels` | `IList<string>` | `null` | Labels shown along the left axis — one per row, top to bottom. The row count equals `YLabels.Count`. |
| `Cells` | `IList<HeatmapCell>` | `null` | The cells to render. Cells are looked up by `(Row, Column)` — missing positions use `EmptyCellColor`. |
| `CellSpacing` | `float` | `2` | Gap in pixels between adjacent cells. |
| `LabelFontSize` | `float` | `12` | Font size (sp) for axis labels. |
| `LabelColor` | `Color` | `Black` | Colour of the axis label text. |
| `ShowGridLines` | `bool` | `false` | When `true`, draws a thin border around every cell. |
| `GridLineColor` | `Color` | `LightGray` | Colour of the optional grid-line borders. |
| `EmptyCellColor` | `Color` | `Transparent` | Fill colour for grid positions that have no matching `HeatmapCell`. |
| `XLabelRotation` | `float` | `0` | Clockwise rotation in degrees for X-axis labels. Use `45` or `90` for long labels that would otherwise overlap. |

---

# SignaturePadView

`SignaturePadView` is a SkiaSharp-based freehand signature capture control. Users draw with a finger or stylus; the result can be exported as a PNG or JPEG at any requested output size with a choice of background colour.

## Basic Usage

```xaml
xmlns:controls="http://dsoft.maui/schemas/controls"

<controls:SignaturePadView
    x:Name="SignaturePad"
    HeightRequest="220"
    HorizontalOptions="Fill"
    ViewBackgroundColor="#F0F0F0"
    InkColor="Black"
    StrokeWidth="3" />
```

Add Clear and Export buttons in code-behind:

```csharp
private void OnClearClicked(object sender, EventArgs e)
{
    SignaturePad.Clear();
}

private async void OnExportClicked(object sender, EventArgs e)
{
    if (!SignaturePad.HasSignature)
        return;

    byte[] bytes = await SignaturePad.GetImageAsync(
        width: 600,
        height: 300,
        format: SignatureImageFormat.Png,
        backgroundColor: Colors.White);

    SignatureImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
}
```

## Exporting the Signature

`GetImageAsync` renders the captured strokes onto a fresh surface at the exact pixel dimensions you request, with the background colour of your choice. The `ViewBackgroundColor` of the live view is intentionally excluded from the export so you can always produce a clean image regardless of how the pad looks on screen.

```csharp
// PNG with a white background
byte[] png = await SignaturePad.GetImageAsync(
    width: 800,
    height: 400,
    format: SignatureImageFormat.Png,
    backgroundColor: Colors.White);

// PNG with a transparent background
byte[] pngTransparent = await SignaturePad.GetImageAsync(
    width: 800,
    height: 400,
    format: SignatureImageFormat.Png,
    backgroundColor: Colors.Transparent);

// JPEG with a light-yellow background
byte[] jpg = await SignaturePad.GetImageAsync(
    width: 800,
    height: 400,
    format: SignatureImageFormat.Jpeg,
    backgroundColor: Color.FromArgb("#FFF9C4"));
```

The strokes are scaled proportionally to the requested output size, so the signature always looks the same regardless of the export dimensions.

## Bindable Properties Reference

| Property | Type | Default | Description |
|---|---|---|---|
| `InkColor` | `Color` | `Black` | The colour of the ink strokes. |
| `StrokeWidth` | `float` | `3` | Thickness of the ink strokes in device-independent units. |
| `ViewBackgroundColor` | `Color` | `White` | Background colour shown behind the signature while drawing. **Not** included in exported images. |

## Public API

| Member | Description |
|---|---|
| `HasSignature` | `bool` — `true` if the user has drawn at least one point. |
| `Clear()` | Removes all strokes and resets the pad. |
| `GetImageAsync(width, height, format, backgroundColor)` | Returns the signature as a `byte[]` encoded at the requested size. `backgroundColor` defaults to `Colors.White` if `null`. |

### `SignatureImageFormat`

| Value | Description |
|---|---|
| `Png` | PNG — lossless, supports transparency. |
| `Jpeg` | JPEG — lossy, no transparency. |

## How It Works

The control hosts an `SKCanvasView` with `EnableTouchEvents = true`. Each finger or stylus contact is tracked independently by its touch id, so multiple simultaneous strokes are handled correctly. Points within a stroke are smoothed using quadratic Bézier curves through consecutive mid-points, giving the ink a natural, handwritten feel. Single taps (a press with no movement) are rendered as filled circles.

When `GetImageAsync` is called, a new off-screen `SKSurface` is created at the requested dimensions. The stored stroke points — which are in the canvas's native pixel space — are scaled proportionally to the output size. The stroke width scales by the same ratio, keeping the visual weight of the ink consistent with what the user saw on screen.

---

# ColorWheelView

`ColorWheelView` is a SkiaSharp-based interactive color wheel. The wheel renders a full HSL hue sweep with an optional white radial gradient overlay at the centre, allowing users to pick any hue at varying saturation levels. A circular indicator follows the touch point and reflects the selected color.

## Basic Usage

```xaml
xmlns:controls="http://dsoft.maui/schemas/controls"

<controls:ColorWheelView
    x:Name="ColorWheel"
    HeightRequest="300"
    WidthRequest="300"
    HorizontalOptions="Center"
    SelectedColor="{Binding PickedColor}"
    ColorChanged="OnColorChanged" />
```

## MVVM / Data Binding

`SelectedColor` is a two-way bindable property. Setting it from a view-model moves the indicator to the matching position on the wheel; touching the wheel updates the binding automatically.

```xaml
<controls:ColorWheelView
    SelectedColor="{Binding PickedColor, Mode=TwoWay}"
    HeightRequest="300"
    WidthRequest="300" />
```

```csharp
// ViewModel
[ObservableProperty]
private Color _pickedColor = Colors.Red;
```

When `SelectedColor` is set before the wheel has been laid out (e.g. on page load), the position is calculated and applied as soon as the first paint completes.

## Code-Behind Event

```csharp
private void OnColorChanged(object sender, ColorChangedEventArgs e)
{
    MyPreviewBox.BackgroundColor = e.Color;
}
```

## Bindable Properties Reference

| Property | Type | Default | Description |
|---|---|---|---|
| `SelectedColor` | `Color` | `Transparent` | The currently selected color. Two-way bindable — setting this moves the indicator to the corresponding wheel position. |
| `Colors` | `IEnumerable<Color>` | 8 evenly-spaced HSL hues | The hue gradient colors used to build the sweep. Replace to create a restricted palette wheel. |
| `ShowWhite` | `bool` | `true` | When `true`, overlays a radial white-to-transparent gradient at the centre, allowing desaturated/pastel colors to be picked. |

## Events

| Event | Args type | Description |
|---|---|---|
| `ColorChanged` | `ColorChangedEventArgs` | Raised when the user touches the wheel and the selected color changes. `e.Color` contains the new `Color` value. |

## How It Works

The wheel is drawn on an `SKCanvasView` using two overlaid shaders: a `SKShader.CreateSweepGradient` for the hue ring and a `SKShader.CreateRadialGradient` for the white centre fade. Touch is tracked via `TouchEffect`; when a touch point lands inside the wheel radius, the pixel at that location is sampled directly from the rendered surface to determine the exact color.

When `SelectedColor` is set programmatically the reverse mapping is applied: the color's HSL hue determines the sweep angle (`angle = (360 − H) % 360°`) and its lightness determines the radial distance from the center (`L = 50` → edge, `L = 100` → white center). The indicator circle is repositioned accordingly and the canvas is invalidated.

---

# TabView

`TabView` is a tab container that uses the built-in `SegmentedControl` as its tab bar. Add `TabItem` children in XAML — the tab bar is built automatically from their titles and selecting a segment instantly shows the matching content view.

## Basic Usage

```xaml
xmlns:controls="http://dsoft.maui/schemas/controls"

<controls:TabView HeightRequest="300">
    <controls:TabItem Title="Profile">
        <VerticalStackLayout Padding="8">
            <Label Text="Profile content here." />
        </VerticalStackLayout>
    </controls:TabItem>
    <controls:TabItem Title="Activity">
        <VerticalStackLayout Padding="8">
            <Label Text="Activity content here." />
        </VerticalStackLayout>
    </controls:TabItem>
    <controls:TabItem Title="Settings">
        <VerticalStackLayout Padding="8">
            <Label Text="Settings content here." />
        </VerticalStackLayout>
    </controls:TabItem>
</controls:TabView>
```

## Styling the Tab Bar

Pass a `Style` targeting `SegmentedControl` to `SegmentedControlStyle` to fully control the look of the tab bar without touching the control internals:

```xaml
<ContentPage.Resources>
    <Style x:Key="BlueTabBar" TargetType="controls:SegmentedControl">
        <Setter Property="TrackColor" Value="#1E88E5" />
        <Setter Property="ThumbColor" Value="White" />
        <Setter Property="SelectedTextColor" Value="#1E88E5" />
        <Setter Property="UnselectedTextColor" Value="White" />
        <Setter Property="CornerRadius" Value="6" />
        <Setter Property="ThumbCornerRadius" Value="4" />
        <Setter Property="SegmentHeight" Value="36" />
        <Setter Property="ThumbShadow" Value="False" />
    </Style>
</ContentPage.Resources>

<controls:TabView SegmentedControlStyle="{StaticResource BlueTabBar}"
                  TabPosition="Bottom">
    ...
</controls:TabView>
```

## Two-Way Binding

`SelectedIndex` supports two-way binding, so you can drive the active tab from a view-model or another control:

```xaml
<controls:TabView SelectedIndex="{Binding ActiveTab}">
    ...
</controls:TabView>
```

## Hiding the Tab Bar

Set `IsTabBarVisible` to `false` to hide the segmented control entirely. The row it occupied collapses so no gap is left behind. This is useful when you want to drive tab selection purely from a view-model or an external control without showing the built-in tab bar.

```xaml
<!-- Hide statically -->
<controls:TabView IsTabBarVisible="False" SelectedIndex="{Binding ActiveTab}">
    ...
</controls:TabView>
```

```xaml
<!-- Toggle from a view-model -->
<controls:TabView IsTabBarVisible="{Binding ShowTabs}" SelectedIndex="{Binding ActiveTab}">
    ...
</controls:TabView>
```

## Bindable Properties Reference

| Property | Type | Default | Description |
|---|---|---|---|
| `SelectedIndex` | `int` | `0` | Zero-based index of the active tab. Supports two-way binding. |
| `TabPosition` | `TabPosition` | `Top` | Places the tab bar above (`Top`) or below (`Bottom`) the content area. |
| `TabSpacing` | `double` | `8` | Gap in device-independent units between the tab bar and the content area. |
| `SegmentedControlStyle` | `Style` | `null` | A MAUI `Style` targeting `SegmentedControl` applied to the internal tab bar. |
| `IsTabBarVisible` | `bool` | `true` | When `false`, hides the segmented control and collapses its row so no space is reserved. |

## Events

| Event | Description |
|---|---|
| `TabSelected` | Raised when the user taps a tab. The `EventArgs` value is the new selected index (`int`). |

---

# DataGridView

`DataGridView` is a pure-MAUI data grid backed by a `System.Data.DataTable`. It renders column headers, alternating row backgrounds, row selection, column sorting, and optional horizontal scrolling — all without any platform-specific code.

## Basic Usage

Create a `DataTable`, set column metadata via `ExtendedProperties`, populate rows, then assign it to `DataSource`:

```csharp
var table = new DataTable("Orders");

var idCol = table.Columns.Add("ID", typeof(int));
idCol.ExtendedProperties["Width"]     = 60.0;   // fixed width in dip
idCol.ExtendedProperties["AllowSort"] = true;

var nameCol = table.Columns.Add("Product", typeof(string));
// No Width → Star (fills remaining space)

var priceCol = table.Columns.Add("Price", typeof(string));
priceCol.ExtendedProperties["Width"]     = 90.0;
priceCol.ExtendedProperties["AllowSort"] = false; // header not tappable

table.Rows.Add(1, "Widget A", "$9.99");
table.Rows.Add(2, "Widget B", "$14.99");

MyGrid.DataSource = table;
```

```xaml
xmlns:controls="http://dsoft.maui/schemas/controls"

<controls:DataGridView x:Name="MyGrid"
                       RowSelected="OnRowSelected" />
```

```csharp
private void OnRowSelected(object sender, DataGridRowSelectedEventArgs e)
{
    Console.WriteLine($"Row {e.RowIndex}: {e.Row["Product"]}");
}
```

## Column Metadata

Column display behaviour is controlled via `DataColumn.ExtendedProperties`:

| Key | Type | Description |
|---|---|---|
| `"Width"` | `double` | Fixed column width in device-independent units. Omit (or set no value) for proportional Star sizing. |
| `"AllowSort"` | `bool` | Whether tapping the column header triggers a sort. Defaults to `true` when not set. |

Columns without a `"Width"` entry share the remaining space equally using Star sizing (or use `DefaultColumnWidth` when `HorizontalScrollEnabled` is `true`).

## Sorting

Tapping a sortable column header sorts the `DataTable.DefaultView` ascending. Tapping the same header again reverses to descending. Tapping a different header resets the sort direction to ascending for that column.

The `ColumnSorted` event is raised after each sort so you can react in code:

```csharp
MyGrid.ColumnSorted += (s, e) =>
    Console.WriteLine($"Sorted by column index {e.ColumnIndex}");
```

## Row Selection

Tapping a row highlights it with `SelectionColor` and raises `RowSelected`. Only one row can be selected at a time.

```csharp
MyGrid.RowSelected += (s, e) =>
{
    // e.Row    — the selected DataRow
    // e.RowIndex — zero-based position in the current sort order
};
```

Clear the selection programmatically without firing the event:

```csharp
MyGrid.ClearSelection();
```

## Horizontal Scrolling

Set `HorizontalScrollEnabled="True"` to enable horizontal scrolling. The header and rows scroll together. When enabled, Star-width columns use `DefaultColumnWidth` (default `120`) as their absolute width so the total content width can be computed.

```xaml
<controls:DataGridView DataSource="{Binding Table}"
                       HorizontalScrollEnabled="True"
                       DefaultColumnWidth="150" />
```

## Runtime Updates

Call `Reload()` after modifying the `DataTable`'s rows or columns at runtime:

```csharp
table.Rows.Add(3, "Widget C", "$19.99");
MyGrid.Reload();
```

Assigning a new `DataTable` to `DataSource` triggers a full rebuild automatically.

## Bindable Properties Reference

### Data

| Property | Type | Default | Description |
|---|---|---|---|
| `DataSource` | `DataTable` | `null` | The table that provides columns and rows. Assigning a new value rebuilds the grid and resets sort state. |

### Layout

| Property | Type | Default | Description |
|---|---|---|---|
| `RowHeight` | `double` | `44` | Height of each data row in device-independent units. |
| `HeaderHeight` | `double` | `44` | Height of the column header row. |
| `HorizontalScrollEnabled` | `bool` | `false` | When `true`, wraps the grid in a horizontal `ScrollView` so wide tables can be scrolled. Header and rows scroll together. |
| `DefaultColumnWidth` | `double` | `120` | Width applied to Star columns when `HorizontalScrollEnabled` is `true`. Has no effect when horizontal scroll is disabled. |

### Colours

| Property | Type | Default | Description |
|---|---|---|---|
| `HeaderBackgroundColor` | `Color` | `#CCCCCC` | Background colour of the header row. |
| `HeaderTextColor` | `Color` | `Black` | Text colour of the column header labels. |
| `RowBackgroundColor` | `Color` | `White` | Background colour for even-indexed data rows. |
| `AlternateRowBackgroundColor` | `Color` | `#F2F2F2` | Background colour for odd-indexed data rows. |
| `SelectionColor` | `Color` | `#ADD8E6` | Background colour of the currently selected row. |
| `CellTextColor` | `Color` | `Black` | Text colour of data cells. |
| `SeparatorColor` | `Color` | `#DDDDDD` | Colour of row separators and optional column separators. |
| `ShowColumnSeparators` | `bool` | `true` | When `true`, draws a 1px vertical separator between columns in both the header and data rows. |

## Events

| Event | Args type | Description |
|---|---|---|
| `RowSelected` | `DataGridRowSelectedEventArgs` | Raised when the user taps a data row. `e.Row` is the `DataRow`; `e.RowIndex` is its zero-based position. |
| `ColumnSorted` | `DataGridColumnSortedEventArgs` | Raised after the grid re-sorts following a header tap. `e.ColumnIndex` is the zero-based column that was tapped. |

## How It Works

`DataGridView` is a `ContentView` whose content is a two-row `Grid`: the top row holds a `Grid`-based header (rebuilt whenever the `DataSource` changes) and the bottom row holds a `CollectionView` for virtualised row rendering. Each row is a `ContentView` wrapping an inner column `Grid` that mirrors the header's column definitions exactly.

Selection state is tracked through a lightweight `DataRowViewModel` wrapper (one per row) that implements `INotifyPropertyChanged`. When a row is tapped the wrapper's `IsSelected` flag is toggled and the row's background updates immediately without a full rebuild.

Sorting is applied to `DataTable.DefaultView.Sort` and `BuildRows` is called to refresh the `CollectionView` items source in the new order.

When `HorizontalScrollEnabled` is `true`, the inner `_rootGrid` is placed inside a `ScrollView` with `Orientation = Horizontal` and given a `WidthRequest` equal to the sum of all column widths. Star columns are converted to Absolute widths using `DefaultColumnWidth` so the total content width is well-defined in the unconstrained horizontal measurement pass.
