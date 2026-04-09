#nullable enable
using DSoft.Maui.Controls.Extensions;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp;
using MColor = Microsoft.Maui.Graphics.Colors;
using DSoft.Maui.Controls.TouchTracking;

namespace DSoft.Maui.Controls.ColorPicker
{
	public class ColorWheelView : Border
	{
		#region Defaults

		private static IEnumerable<Color> DefaultColors
		{
			get
			{
				var results = new List<Color>();

				for (int i = 0; i < 8; i++)
				{
					results.Add(SKColor.FromHsl(i * 360f / 7, 100, 50).ToMauiColor());
				}

				results.Reverse();

				return results;
			}
		}
		
		#endregion
		
		#region Bindable Properties

		#region ColorsProperty

		public static readonly BindableProperty ColorsProperty = BindableProperty.Create(nameof(Colors), typeof(IEnumerable<Color>), typeof(ColorWheelView), DefaultColors, propertyChanged: RedrawCanvas);

		/// <summary>
		/// The Colors to be used by the color gradient
		/// </summary>
		public IEnumerable<Color> Colors
		{
			get => (IEnumerable<Color>)GetValue(ColorsProperty);
			set => SetValue(ColorsProperty, value);
		}


		#endregion

		#region ShowWhite

		public static readonly BindableProperty ShowWhiteProperty = BindableProperty.Create(nameof(Colors), typeof(bool), typeof(ColorWheelView), true, propertyChanged: RedrawCanvas);

		/// <summary>
		/// Show the white color gradient overlay at the center of the Gradient
		/// </summary>
		public bool ShowWhite
		{
			get => (bool)GetValue(ShowWhiteProperty);
			set => SetValue(ShowWhiteProperty, value);
		}


		#endregion

		#region SelectedColor

		public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(
			nameof(SelectedColor),
			typeof(Color),
			typeof(ColorWheelView),
			MColor.Transparent,
			BindingMode.TwoWay,
			propertyChanged: OnSelectedColorChanged);

		/// <summary>
		/// Gets or sets the currently selected color. Setting this from a binding will move the indicator to the matching position on the wheel.
		/// </summary>
		public Color SelectedColor
		{
			get => (Color)GetValue(SelectedColorProperty);
			set => SetValue(SelectedColorProperty, value);
		}

		private static void OnSelectedColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var view = (ColorWheelView)bindable;
			if (view._updatingFromTouch) return;

			if (newValue is Color color && color != MColor.Transparent)
				view.SetTouchLocationFromColor(color);
		}

		#endregion

		#endregion

		#region Fields

		private readonly SKCanvasView _canvasView = new SKCanvasView();

		private const int _shrinkage = 50;

		private float _radius;
		private bool _colorChanged;
		private float _touchCircleRadius = 30;

		#region Paint Objects
		private readonly SKPaint _touchCircleOutline = new SKPaint
		{
			Style = SKPaintStyle.Stroke,
			Color = MColor.Gray.ToSKColor(),
			StrokeWidth = 4,
			IsAntialias = true
		};

		private readonly SKPaint _touchCircleFill = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};

		private readonly SKPaint _circlePalette = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};


		private readonly SKPaint _centerGradient = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};

		private readonly SKPaint _rectanglePalette = new SKPaint
		{
			Style = SKPaintStyle.Fill,
			IsAntialias = true
		};
		#endregion

		#region Points
		private SKPoint _touchLocation;
		private SKPoint _center;
		#endregion

		#region Colors

		private SKColor _selectedColor = MColor.Transparent.ToSKColor();
		#endregion

		#region State

		private bool _updatingFromTouch;
		private Color? _pendingSelectedColor;

		#endregion

		#region Events

		public event EventHandler<ColorChangedEventArgs> ColorChanged;

		#endregion

		#endregion
		
		public ColorWheelView()
		{

			Padding = new Thickness(0);
			BackgroundColor = MColor.Transparent;

			Content = _canvasView;
			_canvasView.PaintSurface += OnPaintSurface;

			var touchEffect = new TouchEffect()
			{
				Capture = true,

			};

			touchEffect.TouchAction += OnTouchEffectAction;

			this.Effects.Add(touchEffect);
		}

		private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
		{
			ColorWheelView colorWheel = bindable as ColorWheelView;
			colorWheel?._canvasView.InvalidateSurface();
		}

		void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{

			var colorRange = Colors.ToSKColors();

			var info = e.Info;
			var surface = e.Surface;
			var canvas = surface.Canvas;
			canvas.Clear();

			_center = new SKPoint(info.Rect.MidX, info.Rect.MidY);
			_radius = (Math.Min(info.Width, info.Height) - _shrinkage) / 2;

			if (_pendingSelectedColor is not null)
				SetTouchLocationFromColor(_pendingSelectedColor);

			_circlePalette.Shader = SKShader.CreateSweepGradient(_center, colorRange, null);
			canvas.DrawCircle(_center, _radius, _circlePalette);

			if (ShowWhite)
			{
				var innerradius = _radius * 0.75f;
				_centerGradient.Shader = SKShader.CreateRadialGradient(
					_center,
					innerradius,
					new SKColor[] { SKColors.White,
												SKColors.Transparent },
					new float[] { 0.05f, 1 },
					SKShaderTileMode.Clamp);

				canvas.DrawCircle(_center, innerradius, _centerGradient);
			}


			var rectLeft = info.Rect.MidX - _radius;
			var rectTop = 0;
			var rectRight = rectLeft + _radius * 2;
			var rectBottom = rectTop + _shrinkage;
			var rect = new SKRect(rectLeft, rectTop, rectRight, rectBottom);

			//insure touch circle in the center of color wheel
			if (_touchLocation == SKPoint.Empty)
			{
				_touchLocation = _center;
				_colorChanged = true;
			}

			if (_colorChanged)
			{
				using (var bmp = new SKBitmap(info))
				{
					IntPtr dstpixels = bmp.GetPixels();

					var succeed = surface.ReadPixels(info, dstpixels, info.RowBytes, (int)_touchLocation.X, (int)_touchLocation.Y);
					if (succeed)
					{
						_selectedColor = bmp.GetPixel(0, 0);
						_touchCircleFill.Color = _selectedColor;

						var mauiColor = _selectedColor.ToMauiColor();
						ColorChanged?.Invoke(this, new ColorChangedEventArgs(mauiColor));

						_updatingFromTouch = true;
						SelectedColor = mauiColor;
						_updatingFromTouch = false;
					}
				}
			}

			canvas.DrawCircle(_touchLocation, _touchCircleRadius, _touchCircleOutline);
			canvas.DrawCircle(_touchLocation, _touchCircleRadius, _touchCircleFill);
		}

		private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
		{
			var skPoint = args.Location.ToPixelSKPoint(_canvasView);
			if (skPoint.IsInsideCircle(_center, _radius))
			{
				_touchLocation = skPoint;

				if (args.Type == TouchActionType.Entered ||
					args.Type == TouchActionType.Pressed ||
					args.Type == TouchActionType.Moved ||
					args.Type == TouchActionType.Released)
				{
					_colorChanged = true;
					_canvasView.InvalidateSurface();
				}
			}
			else
			{
				_colorChanged = false;
			}
		}

		/// <summary>
		/// Converts a color back to a touch position on the wheel using hue (angle) and lightness (radius).
		/// The default color wheel sweeps hues in reverse, so angle = (360 - hue) % 360.
		/// Lightness maps from 50 (fully saturated edge) to 100 (white center).
		/// </summary>
		private void SetTouchLocationFromColor(Color color)
		{
			if (_radius == 0 || _center == SKPoint.Empty)
			{
				_pendingSelectedColor = color;
				return;
			}

			_pendingSelectedColor = null;

			var skColor = color.ToSKColor();
			skColor.ToHsl(out float h, out _, out float l);

			// The sweep gradient reverses hues (H=360..0 maps to angle 0°..360°)
			var angleRad = (360f - h) % 360f * (float)(Math.PI / 180.0);

			// L=50 → pure hue at edge, L=100 → white at center
			var radius = Math.Min(_radius, Math.Max(0f, (100f - l) / 50f * _radius));

			_touchLocation = new SKPoint(
				_center.X + (float)Math.Cos(angleRad) * radius,
				_center.Y + (float)Math.Sin(angleRad) * radius);

			_selectedColor = skColor;
			_touchCircleFill.Color = _selectedColor;
			_colorChanged = false;
			_canvasView.InvalidateSurface();
		}

	}
}
