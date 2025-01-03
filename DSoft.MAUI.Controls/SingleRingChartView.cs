using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp;

namespace DSoft.Maui.Controls
{
	public class SingleRingChartView : ContentView
	{
		#region fields and Properties
		private const double StartAngle = 270;
		private const double EndAngle = 360;

		private readonly SKCanvasView _canvasView = new SKCanvasView()
		{
			BackgroundColor = Colors.Transparent,
		};

		private readonly Grid _container = new Grid()
		{
			Padding = new Thickness(0, 0, 0, 0),
		};

		//public double CurrentSweep => EndAngle * (Percent / 100);

		#endregion

		#region Bindable Properties

		#region ScaleBackgroundColor

		public static readonly BindableProperty RingBackgroundColorProperty = BindableProperty.Create(nameof(RingBackgroundColor), typeof(Color), typeof(SingleRingChartView), Color.FromRgba("#ebeced"), propertyChanged: RedrawCanvas);

		public Color RingBackgroundColor
		{
			get => (Color)GetValue(RingBackgroundColorProperty);
			set => SetValue(RingBackgroundColorProperty, value);
		}

		#endregion

		#region ScaleColor

		public static readonly BindableProperty RingColorProperty = BindableProperty.Create(nameof(RingColor), typeof(Color), typeof(SingleRingChartView), Color.FromRgba("#ebeced"), propertyChanged: RedrawCanvas);

		public Color RingColor
		{
			get => (Color)GetValue(RingColorProperty);
			set => SetValue(RingColorProperty, value);
		}

		public static readonly BindableProperty UseShadedRingColorProperty = BindableProperty.Create(nameof(UseShadedRingColor), typeof(bool), typeof(SingleRingChartView), false, propertyChanged: RedrawCanvas);

		public bool UseShadedRingColor
		{
			get => (bool)GetValue(UseShadedRingColorProperty);
			set => SetValue(UseShadedRingColorProperty, value);
		}

		#endregion

		#region CenterView

		public static readonly BindableProperty CenterViewProperty = BindableProperty.Create(nameof(CenterView), typeof(View), typeof(SingleRingChartView), null, propertyChanged: null);

		public View CenterView
		{
			get => (View)GetValue(CenterViewProperty);
			set => SetValue(CenterViewProperty, value);
		}

		#endregion

		#region ItemsSource

		public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value),
																						typeof(double),
																						typeof(SingleRingChartView),
																						0d,
																						BindingMode.OneWay,
																						propertyChanged: RedrawCanvas);

		public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue),
																				typeof(double),
																				typeof(SingleRingChartView),
																				0d,
																				BindingMode.OneWay,
																				propertyChanged: RedrawCanvas);

		public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue),
																				typeof(double),
																				typeof(SingleRingChartView),
																				100d,
																				BindingMode.OneWay,
																				propertyChanged: RedrawCanvas);


		public double Value
		{
			get => (double)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}


		public double MinValue
		{
			get => (double)GetValue(MinValueProperty);
			set => SetValue(MinValueProperty, value);
		}

		public double MaxValue
		{
			get => (double)GetValue(MaxValueProperty);
			set => SetValue(MaxValueProperty, value);
		}


		#endregion

		#region Max Line Size

		public static readonly BindableProperty RingLineWidthProperty = BindableProperty.Create(
							nameof(RingLineWidth), typeof(double), typeof(SingleRingChartView), 0d, propertyChanged: RedrawCanvas);

		public double RingLineWidth
		{
			get => (double)GetValue(RingLineWidthProperty);
			set => SetValue(RingLineWidthProperty, value);
		}

		#endregion

		#region Drop Shadow

		public static readonly BindableProperty HasDropShadowProperty = BindableProperty.Create(
			nameof(HasDropShadow), typeof(bool), typeof(SingleRingChartView), true, propertyChanged: RedrawCanvas);

		public bool HasDropShadow
		{
			get => (bool)GetValue(HasDropShadowProperty);
			set => SetValue(HasDropShadowProperty, value);
		}

		public static readonly BindableProperty DropShadowDepthProperty = BindableProperty.Create(
			nameof(DropShadowDepth), typeof(int), typeof(SingleRingChartView), 2, propertyChanged: RedrawCanvas);

		public int DropShadowDepth
		{
			get => (int)GetValue(DropShadowDepthProperty);
			set => SetValue(DropShadowDepthProperty, value);
		}



		#endregion
		#endregion

		#region Constructors

		public SingleRingChartView()
		{
			HorizontalOptions = LayoutOptions.Fill;
			VerticalOptions = LayoutOptions.Fill;

			_canvasView.PaintSurface += OnPaintSurface;

			var grid = new Grid()
			{
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill,
			};


			grid.Children.Add(_container);
			grid.Children.Add(_canvasView);

			this.Content = grid;

		}

		#endregion

		#region Methods

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			var currentPosX = this.Width / 2;
			var currentPosy = this.Height / 2;
			var newSize = this.Width - (this.Width / 3);

			if (CenterView != null)
			{
				_container.Children.Add(CenterView);
			}
			
			_container.LayoutTo(new Rect(currentPosX - (newSize / 2) + 1, currentPosy - (newSize / 2), newSize, newSize), 0, Easing.Linear);
		}

		private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
		{
			var self = bindable as SingleRingChartView;
			self?._canvasView.InvalidateSurface();
		}

		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
		{
			//get the canvas & info
			var canvas = args.Surface.Canvas;
			int surfaceWidth = args.Info.Width;
			int surfaceHeight = args.Info.Height;

			if (HasDropShadow == true)
			{
				surfaceWidth -= (DropShadowDepth * 2);
				surfaceHeight -= (DropShadowDepth * 2);
			}

			var scale = args.Info.Width / this.Width;

			//clear the canvas
			canvas.Clear();

			var innerWidth = 0d;
			var innerHeight = 0d;

			if (this.Content != null)
			{
				innerWidth = this.Content.Width * scale;
				innerHeight = this.Content.Height * scale;
			}

			var centerX = surfaceWidth / 2;
			var centerY = surfaceHeight / 2;

			var initialRadius = Math.Min(surfaceHeight, surfaceWidth);
			var innerRadius = Math.Min(innerHeight, innerWidth);

			if (HasDropShadow == true)
			{
				initialRadius -= (DropShadowDepth * 2);
				innerRadius -= (DropShadowDepth * 2);
			}


			var size = initialRadius - (innerRadius * 1.33);
			
			var maxLineWidth = (RingLineWidth > 0) ? (float)RingLineWidth : (float)(size / 2);
			

			var backRadius = initialRadius - maxLineWidth;
			var backLeft = centerX - (backRadius * 0.5f);
			var backTop = centerY - (backRadius * 0.5f);

			if (HasDropShadow == true)
			{
				backLeft += DropShadowDepth;
				backTop += DropShadowDepth;
			}

			var backRight = backLeft + backRadius;
			var backBottom = backTop + backRadius;

			//Main rect
			var rect = new SKRect(backLeft, backTop, backRight, backBottom);

			var foreColor = RingColor;

			var backColor = (UseShadedRingColor == true) ? Color.FromRgba(foreColor.Red, foreColor.Green, foreColor.Blue, 0.4f) : RingBackgroundColor;

			var perc = (Value - MinValue) / (MaxValue - MinValue) * 100;

			DrawRing(canvas, rect, backColor.ToSKColor(), foreColor.ToSKColor(), maxLineWidth, perc);

			rect.Inflate(new SKSize(-(maxLineWidth * 2), -(maxLineWidth * 2)));


		}

		private void DrawRing(SKCanvas canvas, SKRect rect, SKColor backcolor, SKColor foreColor, float lineWidth, double percent)
		{
			var currentSweep = EndAngle * (percent / 100);
			
			var arcPaintBack = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				StrokeWidth = lineWidth,
				Color = backcolor,
				IsAntialias = true,

			};

			var arcPaintBackRound = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = backcolor,
				IsAntialias = true,
			};

			var arcPaintRound = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = foreColor,
				IsAntialias = true,
			};

			var arcPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				StrokeWidth = lineWidth,
				Color = foreColor,
				IsAntialias = true,
			};

			if (HasDropShadow)
			{
				var rectangleStyleFillShadow = SKImageFilter.CreateDropShadow(0f, 0f, DropShadowDepth, DropShadowDepth, foreColor, null);
				arcPaintRound.ImageFilter = rectangleStyleFillShadow;
				arcPaint.ImageFilter = rectangleStyleFillShadow;

			}

			var angle = Math.PI * (StartAngle + 0) / 180.0;
			var endangle = Math.PI * (StartAngle + EndAngle) / 180.0;

			//calculate the radius and the center point of the circle
			var radius = (rect.Right - rect.Left) / 2;
			var middlePoint = new SKPoint();
			middlePoint.X = (rect.Left + radius);
			middlePoint.Y = rect.Top + radius; //top of current circle plus radius

			canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(angle)), middlePoint.Y + (float)(radius * Math.Sin(angle)), lineWidth / 2, arcPaintBackRound);
			canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(endangle)), middlePoint.Y + (float)(radius * Math.Sin(endangle)), lineWidth / 2, arcPaintBackRound);

			using (var path = new SKPath())
			{
				path.AddArc(rect, (float)StartAngle, (float)EndAngle);
				canvas.DrawPath(path, arcPaintBack);
			}

			angle = Math.PI * (StartAngle) / 180.0;
			endangle = Math.PI * ((StartAngle + currentSweep)) / 180.0;


			canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(angle)), middlePoint.Y + (float)(radius * Math.Sin(angle)), lineWidth / 2, arcPaintRound);
			canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(endangle)), middlePoint.Y + (float)(radius * Math.Sin(endangle)), lineWidth / 2, arcPaintRound);

			using (SKPath path = new SKPath())
			{

				path.AddArc(rect, (float)StartAngle, (float)currentSweep);

				canvas.DrawPath(path, arcPaint);
			}
		}

		#endregion


	}
}
