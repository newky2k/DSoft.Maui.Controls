using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Collections.Generic;
using System.Linq;

namespace DSoft.Maui.Controls
{
    /// <summary>
    /// A pure-MAUI / SkiaSharp ContentView that renders a heatmap grid.
    /// Each cell supplies its own <see cref="Color"/> directly — no colour-stop
    /// interpolation is performed by this control.
    /// </summary>
    public class HeatmapChartView : ContentView
    {
        #region Fields

        private readonly SKCanvasView _canvasView = new SKCanvasView
        {
            BackgroundColor = Colors.Transparent,
        };

        #endregion

        #region Bindable Properties

        #region XLabels

        public static readonly BindableProperty XLabelsProperty = BindableProperty.Create(
            nameof(XLabels), typeof(IList<string>), typeof(HeatmapChartView), null,
            propertyChanged: RedrawCanvas);

        /// <summary>Labels displayed along the bottom (one per column).</summary>
        public IList<string> XLabels
        {
            get => (IList<string>)GetValue(XLabelsProperty);
            set => SetValue(XLabelsProperty, value);
        }

        #endregion

        #region YLabels

        public static readonly BindableProperty YLabelsProperty = BindableProperty.Create(
            nameof(YLabels), typeof(IList<string>), typeof(HeatmapChartView), null,
            propertyChanged: RedrawCanvas);

        /// <summary>Labels displayed along the left side (one per row, top to bottom).</summary>
        public IList<string> YLabels
        {
            get => (IList<string>)GetValue(YLabelsProperty);
            set => SetValue(YLabelsProperty, value);
        }

        #endregion

        #region Cells

        public static readonly BindableProperty CellsProperty = BindableProperty.Create(
            nameof(Cells), typeof(IList<HeatmapCell>), typeof(HeatmapChartView), null,
            propertyChanged: RedrawCanvas);

        /// <summary>The collection of cells to render. Any grid position without a cell is left empty.</summary>
        public IList<HeatmapCell> Cells
        {
            get => (IList<HeatmapCell>)GetValue(CellsProperty);
            set => SetValue(CellsProperty, value);
        }

        #endregion

        #region CellSpacing

        public static readonly BindableProperty CellSpacingProperty = BindableProperty.Create(
            nameof(CellSpacing), typeof(float), typeof(HeatmapChartView), 2f,
            propertyChanged: RedrawCanvas);

        /// <summary>Gap in pixels between adjacent cells.</summary>
        public float CellSpacing
        {
            get => (float)GetValue(CellSpacingProperty);
            set => SetValue(CellSpacingProperty, value);
        }

        #endregion

        #region LabelFontSize

        public static readonly BindableProperty LabelFontSizeProperty = BindableProperty.Create(
            nameof(LabelFontSize), typeof(float), typeof(HeatmapChartView), 12f,
            propertyChanged: RedrawCanvas);

        /// <summary>Font size (sp) used for axis labels.</summary>
        public float LabelFontSize
        {
            get => (float)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        #endregion

        #region LabelColor

        public static readonly BindableProperty LabelColorProperty = BindableProperty.Create(
            nameof(LabelColor), typeof(Color), typeof(HeatmapChartView), Colors.Black,
            propertyChanged: RedrawCanvas);

        /// <summary>Colour used to draw axis label text.</summary>
        public Color LabelColor
        {
            get => (Color)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }

        #endregion

        #region ShowGridLines

        public static readonly BindableProperty ShowGridLinesProperty = BindableProperty.Create(
            nameof(ShowGridLines), typeof(bool), typeof(HeatmapChartView), false,
            propertyChanged: RedrawCanvas);

        /// <summary>When true, a thin border is drawn around every cell.</summary>
        public bool ShowGridLines
        {
            get => (bool)GetValue(ShowGridLinesProperty);
            set => SetValue(ShowGridLinesProperty, value);
        }

        #endregion

        #region GridLineColor

        public static readonly BindableProperty GridLineColorProperty = BindableProperty.Create(
            nameof(GridLineColor), typeof(Color), typeof(HeatmapChartView), Colors.LightGray,
            propertyChanged: RedrawCanvas);

        /// <summary>Colour of the optional grid-line borders.</summary>
        public Color GridLineColor
        {
            get => (Color)GetValue(GridLineColorProperty);
            set => SetValue(GridLineColorProperty, value);
        }

        #endregion

        #region EmptyCellColor

        public static readonly BindableProperty EmptyCellColorProperty = BindableProperty.Create(
            nameof(EmptyCellColor), typeof(Color), typeof(HeatmapChartView), Colors.Transparent,
            propertyChanged: RedrawCanvas);

        /// <summary>Fill colour for grid positions that have no matching <see cref="HeatmapCell"/>.</summary>
        public Color EmptyCellColor
        {
            get => (Color)GetValue(EmptyCellColorProperty);
            set => SetValue(EmptyCellColorProperty, value);
        }

        #endregion

        #region XLabelRotation

        public static readonly BindableProperty XLabelRotationProperty = BindableProperty.Create(
            nameof(XLabelRotation), typeof(float), typeof(HeatmapChartView), 0f,
            propertyChanged: RedrawCanvas);

        /// <summary>
        /// Clockwise rotation in degrees applied to each X-axis label.
        /// Use 45 or 90 for long labels that would otherwise overlap.
        /// </summary>
        public float XLabelRotation
        {
            get => (float)GetValue(XLabelRotationProperty);
            set => SetValue(XLabelRotationProperty, value);
        }

        #endregion

        #endregion

        #region Constructor

        public HeatmapChartView()
        {
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Fill;

            _canvasView.PaintSurface += OnPaintSurface;
            Content = _canvasView;
        }

        #endregion

        #region Drawing

        private static void RedrawCanvas(BindableObject bindable, object oldValue, object newValue)
        {
            var self = bindable as HeatmapChartView;
            self?._canvasView.InvalidateSurface();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear();

            var info = e.Info;
            var scale = (float)(info.Width / Width);

            var xLabels = XLabels ?? new List<string>();
            var yLabels = YLabels ?? new List<string>();
            var cells = Cells ?? new List<HeatmapCell>();

            var columnCount = xLabels.Count;
            var rowCount = yLabels.Count;

            if (columnCount == 0 || rowCount == 0)
                return;

            var labelFontSize = LabelFontSize * scale;
            var spacing = CellSpacing * scale;

            using var labelPaint = new SKPaint
            {
                Color = LabelColor.ToSKColor(),
                TextSize = labelFontSize,
                IsAntialias = true,
            };

            // --- Measure Y-label column width (longest label) ---
            float yLabelWidth = 0f;
            foreach (var label in yLabels)
            {
                var w = labelPaint.MeasureText(label);
                if (w > yLabelWidth) yLabelWidth = w;
            }
            float yLabelMargin = yLabelWidth + (8f * scale); // 8 dp padding

            // --- Measure X-label row height ---
            // When rotated the height needed is the text width; when flat it is the text height.
            float xLabelHeight = 0f;
            float xRotation = XLabelRotation;
            if (xRotation == 0f)
            {
                // unrotated: just one line of text
                var fm = labelPaint.FontMetrics;
                xLabelHeight = fm.Descent - fm.Ascent + (8f * scale);
            }
            else
            {
                // rotated: the projected height is approximately the text width × sin(θ)
                double rad = System.Math.Abs(xRotation) * System.Math.PI / 180.0;
                float maxTextWidth = 0f;
                foreach (var label in xLabels)
                {
                    var w = labelPaint.MeasureText(label);
                    if (w > maxTextWidth) maxTextWidth = w;
                }
                xLabelHeight = (float)(maxTextWidth * System.Math.Sin(rad)) + (8f * scale);
            }
            float xLabelMargin = xLabelHeight;

            // --- Grid area ---
            float gridLeft = yLabelMargin;
            float gridTop = 0f;
            float gridRight = info.Width;
            float gridBottom = info.Height - xLabelMargin;

            float gridWidth = gridRight - gridLeft;
            float gridHeight = gridBottom - gridTop;

            float cellWidth = (gridWidth - spacing * (columnCount - 1)) / columnCount;
            float cellHeight = (gridHeight - spacing * (rowCount - 1)) / rowCount;

            if (cellWidth <= 0 || cellHeight <= 0)
                return;

            // Build a lookup for fast cell colour access
            var cellLookup = cells.ToDictionary(c => (c.Row, c.Column), c => c.Color);

            using var cellPaint = new SKPaint { IsAntialias = false };
            using var gridPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1f,
                Color = GridLineColor.ToSKColor(),
                IsAntialias = false,
            };

            // --- Draw cells ---
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    float left = gridLeft + col * (cellWidth + spacing);
                    float top = gridTop + row * (cellHeight + spacing);
                    var rect = new SKRect(left, top, left + cellWidth, top + cellHeight);

                    Color fillColor = cellLookup.TryGetValue((row, col), out var c) ? c : EmptyCellColor;
                    cellPaint.Style = SKPaintStyle.Fill;
                    cellPaint.Color = fillColor.ToSKColor();
                    canvas.DrawRect(rect, cellPaint);

                    if (ShowGridLines)
                        canvas.DrawRect(rect, gridPaint);
                }
            }

            // --- Draw Y labels (left axis, vertically centred per row) ---
            var fontMetrics = labelPaint.FontMetrics;
            float textHeight = fontMetrics.Descent - fontMetrics.Ascent;

            for (int row = 0; row < rowCount; row++)
            {
                float cellTop = gridTop + row * (cellHeight + spacing);
                float cellMidY = cellTop + cellHeight / 2f;
                float textY = cellMidY - fontMetrics.Ascent - textHeight / 2f;

                string label = yLabels[row];
                float textWidth = labelPaint.MeasureText(label);
                float textX = yLabelMargin - textWidth - (4f * scale);

                canvas.DrawText(label, textX, textY, labelPaint);
            }

            // --- Draw X labels (bottom axis, centred per column) ---
            for (int col = 0; col < columnCount; col++)
            {
                float cellLeft = gridLeft + col * (cellWidth + spacing);
                float cellMidX = cellLeft + cellWidth / 2f;

                string label = xLabels[col];
                float textWidth = labelPaint.MeasureText(label);

                if (xRotation == 0f)
                {
                    float textX = cellMidX - textWidth / 2f;
                    float textY = gridBottom + (8f * scale) - fontMetrics.Ascent;
                    canvas.DrawText(label, textX, textY, labelPaint);
                }
                else
                {
                    // Rotate around the top-centre of the label area
                    float pivotX = cellMidX;
                    float pivotY = gridBottom + (4f * scale);

                    canvas.Save();
                    canvas.Translate(pivotX, pivotY);
                    canvas.RotateDegrees(xRotation);
                    canvas.DrawText(label, 0, -fontMetrics.Ascent, labelPaint);
                    canvas.Restore();
                }
            }
        }

        #endregion
    }
}
