# AGENTS.md

Guidelines for AI agents working in this repository.

## Project Overview

**DSoft.Maui.Controls** is a .NET MAUI controls library providing 100% MAUI-based (no platform-specific implementations) reusable UI controls. It is distributed as the `DSoft.Maui.Controls` NuGet package.

- **Target Frameworks:** .NET 9 and .NET 10
- **Platforms:** Android 33+, iOS 15+, macOS Catalyst 15+, Windows 10 1803+, Tizen 6.5+
- **License:** MIT

## Repository Structure

```
DSoft.MAUI.Controls/       # Main library (the NuGet package)
MauiSampleApp/             # Sample/demo application
DSoft.MAUI.Controls.sln    # Solution file
Directory.Build.props      # Shared MSBuild properties
azure-pipelines-*.yml      # CI/CD pipelines (Azure DevOps)
```

### Library Layout

```
DSoft.MAUI.Controls/
├── *.cs / *.xaml          # Top-level controls (BubbleView, RepeaterView, etc.)
├── ColorPicker/           # Color picker and wheel views
├── TouchTracking/         # Cross-platform touch effect + event args
├── Events/                # Custom EventArgs classes
├── Models/                # Data model classes (e.g. SegmentedControlItem)
├── Extensions/            # MauiBuilderExtensions, SKPointExtensions
└── Platforms/             # Platform-specific handler implementations
    ├── Android/
    ├── iOS/
    ├── MacCatalyst/
    ├── Windows/
    └── Tizen/
```

## Building

```bash
# Restore dependencies
dotnet restore

# Build (Debug)
dotnet build DSoft.MAUI.Controls.sln

# Build (Release — also generates the NuGet package)
dotnet build --configuration Release /p:Platform="Any CPU"
```

**Required workloads:**
```bash
dotnet workload install maui android ios maccatalyst macos wasm-tools
```

## Testing

There is no automated test project. Verification is done manually by running the **MauiSampleApp** on iOS or Android and exercising each demo page:

| Page | Controls demonstrated |
|------|-----------------------|
| ChartsPage | SimpleRadialGaugeView, SimpleDonutGaugeView, RingChartView, SingleRingChartView |
| ControlsPage | BubbleView, RepeaterView |
| PinchZoomPage | PanPinchContainer |
| ColorPickerPage | ColorPickerView, ColorWheelView |
| WizardPage | WizardView |
| SegmentedControlPage | SegmentedControl |

When adding or modifying a control, add or update the corresponding demo page in `MauiSampleApp`.

## Controls Reference

| Control | Base Class | Description |
|---------|-----------|-------------|
| `BubbleView` | `ContentView` | Notification bubble with customisable color, text, shadow, and border |
| `RepeaterView` | `VerticalStackLayout` | Data-bound stack layout (ItemsSource + DataTemplate) |
| `SimpleRadialGaugeView` | `ContentView` | SkiaSharp radial progress gauge |
| `SimpleDonutGaugeView` | `ContentView` | SkiaSharp donut progress gauge |
| `RingChartView` | `ContentView` | Multi-ring chart with bindable ItemsSource |
| `SingleRingChartView` | `ContentView` | Single-ring chart with min/max values |
| `SegmentedControl` | `Grid` | Native-free segmented tab control with icon support |
| `WizardView` | `Grid` | Step-by-step wizard with position binding and looping |
| `SelectableContentView` | `ContentView` | Selectable content item for use in CollectionView |
| `ColorPickerView` | `ContentView` | Grid of selectable color dots |
| `ColorWheelView` | `ContentView` | Circular color-wheel picker |
| `GradientFrame` | `ContentView` | Decorative gradient border frame |
| `PanPinchContainer` | `ContentView` | Pan/pinch/zoom image container (1×–10×) |

## Coding Conventions

### BindableProperty pattern

Every public property on a control must be a `BindableProperty`:

```csharp
public static readonly BindableProperty FooProperty = BindableProperty.Create(
    nameof(Foo), typeof(string), typeof(MyControl), default(string),
    BindingMode.TwoWay, propertyChanged: OnFooChanged);

public string Foo
{
    get => (string)GetValue(FooProperty);
    set => SetValue(FooProperty, value);
}

private static void OnFooChanged(BindableObject bindable, object oldValue, object newValue)
{
    ((MyControl)bindable).RefreshFoo();
}
```

### General rules

- **Namespace:** `DSoft.Maui.Controls` (or `DSoft.Maui.Controls.<SubFolder>` for sub-namespaces)
- **No platform code in controls** — keep all rendering in pure MAUI / SkiaSharp; platform-specific code belongs in `Platforms/`
- Use `#region` blocks (`#Fields`, `#Properties`, `#Events`, `#Methods`) to organise larger files
- Naming callbacks `OnXxxChanged`; redraw helpers `RedrawCanvas()` or `RefreshXxx()`
- Touch/gesture handling uses the `TouchEffect` abstraction; do not add raw platform touch code to controls
- `UseDSoftControls()` in `Extensions/MauiBuilderExtensions.cs` must register any new platform effects

### Initialization

New controls that require platform handler registration must be wired up in `MauiBuilderExtensions.cs`:

```csharp
public static MauiAppBuilder UseDSoftControls(this MauiAppBuilder builder)
{
    builder.ConfigureMauiHandlers(handlers =>
    {
        // register your effect/handler here
    });
    return builder;
}
```

## CI/CD

| Pipeline | Trigger | Purpose |
|----------|---------|---------|
| `azure-pipelines-mergetest.yml` | Manual | Build verification before merging a PR |
| `azure-pipelines-release.yml` | Push to `master` | Produces and publishes the NuGet artifact |

Version format used by the release pipeline: `2.0.{YYMM}.{DD}{rev}`

Both pipelines use `windows-latest` and .NET 10.x SDK with full MAUI workloads.

## Pull Requests

- Target the `development` branch for day-to-day work; `main`/`master` is the release branch.
- Run the merge-test pipeline (or build locally in Release) before marking a PR ready for review.
- Include a demo page update in `MauiSampleApp` for any new or changed control.
- Keep controls 100% MAUI — no Xamarin.Forms compatibility shims or platform renderers.
