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

This packages also just contains `PanPinchContainer` based on `PanPinchContainer` by [CodingOctocat](https://github.com/CodingOctocat/MauiPanPinchContainer)

 - `PanPinchContainer`
    - Zoom in and out and pan around an image 

**All views are 100% MAUI and do not rely on platform implementations.**

 ###### This library has been tested on iOS and Android, but should also work on other platforms

 **NuGet**

|Name|Info|
| ------------------- | ------------------- | 
|DSoft.Maui.Controls|[![NuGet](https://img.shields.io/nuget/v/DSoft.Maui.Controls.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.Maui.Controls)|

# Setup

Add the package from Nuget, either from NuGet Package Manager or command line

        dotnet add package DSoft.Maui.Controls

DSoft.Maui.Controls is a MAUI library so requires no dependency injection calls to initialise.  You can jump straight in.

## Usage

You will need to add a namespace reference to your xaml file

```xaml
  xmlns:mauic="http://dsoft.maui/schemas/controls"  
```
## BubbleView

<img src="https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/BubbleView.png" width="250"/>

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
<img src="https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/RepeaterView.png" width="250"/>

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

<img src="https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/SimpleRadialGuageView.png" width="250"/>

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
<img src="https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/SimpleDonutGuageView.png" width="250"/>

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
<img src="https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/RingChartView.png" width="250"/>

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
<img src="https://raw.githubusercontent.com/newky2k/DSoft.Maui.Controls/refs/heads/main/images/SingleRingChartView.png" width="250"/>


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

