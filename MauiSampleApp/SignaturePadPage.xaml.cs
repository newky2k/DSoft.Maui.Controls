using DSoft.Maui.Controls;
using DSoft.Maui.Controls.Core.Enums;

namespace MauiSampleApp;

public partial class SignaturePadPage : ContentPage
{
    private static readonly Color[] InkColors =
    [
        Colors.Black,
        Color.FromArgb("#1565C0"),
        Color.FromArgb("#B71C1C"),
        Color.FromArgb("#1B5E20"),
    ];

    private static readonly float[] StrokeWidths = [1f, 3f, 5f, 8f];

    private static readonly Color[] ViewBackgrounds =
    [
        Color.FromArgb("#F0F0F0"),
        Colors.White,
        Color.FromArgb("#424242"),
        Color.FromArgb("#FFFDE7"),
    ];

    private static readonly Color[] ExportBackgrounds =
    [
        Colors.White,
        Colors.Transparent,
        Color.FromArgb("#FFF9C4"),
        Color.FromArgb("#E3F2FD"),
    ];

    private Color _exportBackground = Colors.White;
    private SignatureImageFormat _pendingFormat = SignatureImageFormat.Png;

    public SignaturePadPage()
    {
        InitializeComponent();

        InkColorPicker.SelectedIndex = 0;
        StrokeWidthPicker.SelectedIndex = 1;
        ViewBgPicker.SelectedIndex = 0;
        ExportBgPicker.SelectedIndex = 0;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        SignaturePad.Clear();
        PreviewFrame.IsVisible = false;
        StatusLabel.Text = "Export a signature to see the preview below.";
    }

    private async void OnExportPngClicked(object sender, EventArgs e)
    {
        _pendingFormat = SignatureImageFormat.Png;
        await ExportAsync();
    }

    private async void OnExportJpegClicked(object sender, EventArgs e)
    {
        _pendingFormat = SignatureImageFormat.Jpeg;
        await ExportAsync();
    }

    private async Task ExportAsync()
    {
        if (!SignaturePad.HasSignature)
        {
            await DisplayAlert("No Signature", "Please draw a signature before exporting.", "OK");
            return;
        }

        var bytes = await SignaturePad.GetImageAsync(
            width: 600,
            height: 300,
            format: _pendingFormat,
            backgroundColor: _exportBackground);

        if (bytes == null || bytes.Length == 0)
        {
            StatusLabel.Text = "Export produced an empty image.";
            return;
        }

        SignaturePreview.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
        PreviewLabel.Text = $"{_pendingFormat} · 600×300 px · {bytes.Length / 1024.0:F1} KB · background: {ExportBgPicker.SelectedItem}";
        PreviewFrame.IsVisible = true;
        StatusLabel.Text = "Exported signature preview:";
    }

    private void OnInkColorChanged(object sender, EventArgs e)
    {
        var idx = InkColorPicker.SelectedIndex;
        if (idx >= 0 && idx < InkColors.Length)
            SignaturePad.InkColor = InkColors[idx];
    }

    private void OnStrokeWidthChanged(object sender, EventArgs e)
    {
        var idx = StrokeWidthPicker.SelectedIndex;
        if (idx >= 0 && idx < StrokeWidths.Length)
            SignaturePad.StrokeWidth = StrokeWidths[idx];
    }

    private void OnViewBgChanged(object sender, EventArgs e)
    {
        var idx = ViewBgPicker.SelectedIndex;
        if (idx >= 0 && idx < ViewBackgrounds.Length)
            SignaturePad.ViewBackgroundColor = ViewBackgrounds[idx];
    }

    private void OnExportBgChanged(object sender, EventArgs e)
    {
        var idx = ExportBgPicker.SelectedIndex;
        if (idx >= 0 && idx < ExportBackgrounds.Length)
            _exportBackground = ExportBackgrounds[idx];
    }
}
