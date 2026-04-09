using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace DSoft.Maui.Controls;

/// <summary>
/// Specifies the image format for a signature export.
/// </summary>
public enum SignatureImageFormat
{
    /// <summary>PNG format — lossless, supports transparency.</summary>
    Png,
    /// <summary>JPEG format — lossy, no transparency.</summary>
    Jpeg
}

/// <summary>
/// A SkiaSharp-based signature capture view. Users draw with a finger or stylus and the result
/// can be exported as a PNG or JPEG image at any requested size.
/// </summary>
public class SignaturePadView : ContentView
{
    #region Fields

    private readonly SKCanvasView _canvasView;

    // Completed strokes — each inner list is a single continuous line in canvas-pixel coordinates.
    private readonly List<List<SKPoint>> _strokes = new();

    // Strokes currently being drawn, keyed by touch/pointer id.
    private readonly Dictionary<long, List<SKPoint>> _activeStrokes = new();

    #endregion

    #region Events

    /// <summary>
    /// Raised when strokes are added to or cleared from the canvas.
    /// </summary>
    public event EventHandler SignatureChanged;

    #endregion

    #region Bindable Properties

    #region InkColor

    /// <summary>Bindable property for <see cref="InkColor"/>.</summary>
    public static readonly BindableProperty InkColorProperty = BindableProperty.Create(
        nameof(InkColor), typeof(Color), typeof(SignaturePadView), Colors.Black,
        propertyChanged: RedrawCanvas);

    /// <summary>The colour of the ink used to draw the signature. Default is <see cref="Colors.Black"/>.</summary>
    public Color InkColor
    {
        get => (Color)GetValue(InkColorProperty);
        set => SetValue(InkColorProperty, value);
    }

    #endregion

    #region StrokeWidth

    /// <summary>Bindable property for <see cref="StrokeWidth"/>.</summary>
    public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
        nameof(StrokeWidth), typeof(float), typeof(SignaturePadView), 3f,
        propertyChanged: RedrawCanvas);

    /// <summary>
    /// The thickness of the ink strokes in device-independent units (logical pixels). Default is <c>3</c>.
    /// </summary>
    public float StrokeWidth
    {
        get => (float)GetValue(StrokeWidthProperty);
        set => SetValue(StrokeWidthProperty, value);
    }

    #endregion

    #region ViewBackgroundColor

    /// <summary>Bindable property for <see cref="ViewBackgroundColor"/>.</summary>
    public static readonly BindableProperty ViewBackgroundColorProperty = BindableProperty.Create(
        nameof(ViewBackgroundColor), typeof(Color), typeof(SignaturePadView), Colors.White,
        propertyChanged: OnViewBackgroundColorChanged);

    /// <summary>
    /// The background colour displayed behind the signature while the user is drawing.
    /// This colour is <b>not</b> included in the exported image — use the <c>backgroundColor</c>
    /// parameter of <see cref="GetImageAsync"/> to control the export background.
    /// Default is <see cref="Colors.White"/>.
    /// </summary>
    public Color ViewBackgroundColor
    {
        get => (Color)GetValue(ViewBackgroundColorProperty);
        set => SetValue(ViewBackgroundColorProperty, value);
    }

    #endregion

    #endregion

    #region Constructor

    /// <summary>Initialises a new <see cref="SignaturePadView"/>.</summary>
    public SignaturePadView()
    {
        _canvasView = new SKCanvasView
        {
            EnableTouchEvents = true,
            BackgroundColor = Colors.Transparent,
        };

        _canvasView.PaintSurface += OnPaintSurface;
        _canvasView.Touch += OnTouch;

        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
        BackgroundColor = ViewBackgroundColor;
        Content = _canvasView;
    }

    #endregion

    #region Property Changed Callbacks

    private static void RedrawCanvas(BindableObject bindable, object oldValue, object newValue)
    {
        ((SignaturePadView)bindable)._canvasView.InvalidateSurface();
    }

    private static void OnViewBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((SignaturePadView)bindable).BackgroundColor = (Color)newValue;
    }

    #endregion

    #region Touch Handling

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        switch (e.ActionType)
        {
            case SKTouchAction.Pressed:
            case SKTouchAction.Entered:
                // Android can fire Entered instead of Pressed for the initial contact.
                if (!_activeStrokes.ContainsKey(e.Id))
                {
                    var stroke = new List<SKPoint> { e.Location };
                    _activeStrokes[e.Id] = stroke;
                    _strokes.Add(stroke);
                    // Tell any ancestor ScrollView not to steal the touch gesture mid-stroke.
                    SetParentScrollingEnabled(false);
                }
                break;

            case SKTouchAction.Moved:
                if (_activeStrokes.TryGetValue(e.Id, out var active))
                {
                    active.Add(e.Location);
                    _canvasView.InvalidateSurface();
                }
                break;

            case SKTouchAction.Released:
            case SKTouchAction.Cancelled:
            case SKTouchAction.Exited:
                // Android can fire Exited when touch leaves the view boundary mid-stroke.
                var hadStroke = _activeStrokes.Remove(e.Id);
                _canvasView.InvalidateSurface();
                if (_activeStrokes.Count == 0)
                {
                    SetParentScrollingEnabled(true);
                    if (hadStroke)
                        SignatureChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
        }

        e.Handled = true;
    }

    /// <summary>
    /// On Android, asks the nearest ancestor ViewGroup to stop intercepting touch events so that
    /// a parent ScrollView cannot steal the gesture mid-stroke.
    /// </summary>
    private void SetParentScrollingEnabled(bool enabled)
    {
#if ANDROID
        if (_canvasView.Handler?.PlatformView is Android.Views.View nativeView)
            nativeView.Parent?.RequestDisallowInterceptTouchEvent(!enabled);
#endif
    }

    #endregion

    #region Drawing

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        if (_strokes.Count == 0)
            return;

        // Convert logical StrokeWidth to canvas pixels.
        var scale = Width > 0 ? e.Info.Width / (float)Width : 1f;
        RenderStrokes(canvas, _strokes, StrokeWidth * scale, InkColor.ToSKColor());
    }

    /// <summary>
    /// Renders a collection of strokes onto <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas">Destination canvas.</param>
    /// <param name="strokes">Stroke point lists in the canvas coordinate space.</param>
    /// <param name="strokeWidthPx">Stroke thickness in canvas pixels.</param>
    /// <param name="inkColor">Ink colour.</param>
    private static void RenderStrokes(
        SKCanvas canvas,
        IEnumerable<IList<SKPoint>> strokes,
        float strokeWidthPx,
        SKColor inkColor)
    {
        using var linePaint = new SKPaint
        {
            Color = inkColor,
            StrokeWidth = strokeWidthPx,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round,
        };

        using var dotPaint = new SKPaint
        {
            Color = inkColor,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
        };

        foreach (var stroke in strokes)
        {
            if (stroke.Count == 0)
                continue;

            if (stroke.Count == 1)
            {
                // Single tap — render as a filled circle.
                canvas.DrawCircle(stroke[0], strokeWidthPx / 2f, dotPaint);
                continue;
            }

            using var path = new SKPath();
            path.MoveTo(stroke[0]);

            // Smooth quadratic Bézier curve through stroke mid-points for natural-looking ink.
            for (int i = 1; i < stroke.Count - 1; i++)
            {
                var mid = new SKPoint(
                    (stroke[i].X + stroke[i + 1].X) / 2f,
                    (stroke[i].Y + stroke[i + 1].Y) / 2f);
                path.QuadTo(stroke[i], mid);
            }

            path.LineTo(stroke[stroke.Count - 1]);
            canvas.DrawPath(path, linePaint);
        }
    }

    #endregion

    #region Public API

    /// <summary>
    /// Gets a value indicating whether the user has drawn anything on the pad.
    /// </summary>
    public bool HasSignature => _strokes.Count > 0 && _strokes.Any(s => s.Count > 0);

    /// <summary>
    /// Clears the signature and resets the drawing surface.
    /// </summary>
    public void Clear()
    {
        _strokes.Clear();
        _activeStrokes.Clear();
        _canvasView.InvalidateSurface();
        SignatureChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Returns the current canvas size in pixels (the native backing-buffer dimensions).
    /// This is the natural export size — pass it to <see cref="GetImageAsync(int,int,SignatureImageFormat,Color)"/>
    /// or call the <see cref="GetImageAsync(SignatureImageFormat,Color)"/> overload which uses it automatically.
    /// Returns <see cref="Size.Zero"/> before the view has been laid out.
    /// </summary>
    public Size GetCanvasSize()
    {
        var size = _canvasView.CanvasSize;
        return new Size(size.Width, size.Height);
    }

    /// <summary>
    /// Exports the drawn signature as an encoded image at the view's current canvas resolution.
    /// </summary>
    /// <param name="format">
    /// Output format: <see cref="SignatureImageFormat.Png"/> (default) or
    /// <see cref="SignatureImageFormat.Jpeg"/>.
    /// </param>
    /// <param name="backgroundColor">
    /// Background colour for the exported image. Defaults to <see cref="Colors.White"/>.
    /// Pass <see cref="Colors.Transparent"/> with <see cref="SignatureImageFormat.Png"/> for a
    /// transparent background.
    /// </param>
    /// <returns>The encoded image as a byte array, or an empty array if nothing has been drawn.</returns>
    public Task<byte[]> GetImageAsync(
        SignatureImageFormat format = SignatureImageFormat.Png,
        Color backgroundColor = null)
    {
        var size = _canvasView.CanvasSize;
        return GetImageAsync((int)size.Width, (int)size.Height, format, backgroundColor);
    }

    /// <summary>
    /// Exports the drawn signature as an encoded image.
    /// </summary>
    /// <param name="width">Width of the output image in pixels.</param>
    /// <param name="height">Height of the output image in pixels.</param>
    /// <param name="format">
    /// Output format: <see cref="SignatureImageFormat.Png"/> (default) or
    /// <see cref="SignatureImageFormat.Jpeg"/>.
    /// </param>
    /// <param name="backgroundColor">
    /// Background colour for the exported image. Defaults to <see cref="Colors.White"/>.
    /// Pass <see cref="Colors.Transparent"/> with <see cref="SignatureImageFormat.Png"/> for a
    /// transparent background.
    /// </param>
    /// <returns>The encoded image as a byte array, or an empty array if nothing has been drawn.</returns>
    public Task<byte[]> GetImageAsync(
        int width,
        int height,
        SignatureImageFormat format = SignatureImageFormat.Png,
        Color backgroundColor = null)
    {
        backgroundColor ??= Colors.White;

        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;
        canvas.Clear(backgroundColor.ToSKColor());

        var canvasWidth = _canvasView.CanvasSize.Width;
        var canvasHeight = _canvasView.CanvasSize.Height;

        if (canvasWidth > 0 && canvasHeight > 0 && _strokes.Count > 0)
        {
            var scaleX = width / canvasWidth;
            var scaleY = height / canvasHeight;

            // Scale all stroke points from canvas-pixel space to the output image space.
            var scaledStrokes = _strokes
                .Select(s => (IList<SKPoint>)s.Select(p => new SKPoint(p.X * scaleX, p.Y * scaleY)).ToList())
                .ToList();

            // Scale stroke width proportionally so it looks the same as on-screen.
            // StrokeWidth is in logical pixels; the effective output width in logical pixels is (Width).
            var logicalWidth = (float)(Width > 0 ? Width : canvasWidth);
            var outputStrokeWidth = StrokeWidth * (width / logicalWidth);

            RenderStrokes(canvas, scaledStrokes, outputStrokeWidth, InkColor.ToSKColor());
        }

        using var image = surface.Snapshot();
        var encodeFormat = format == SignatureImageFormat.Png
            ? SKEncodedImageFormat.Png
            : SKEncodedImageFormat.Jpeg;

        using var data = image.Encode(encodeFormat, 95);
        return Task.FromResult(data.ToArray());
    }

    #endregion
}
