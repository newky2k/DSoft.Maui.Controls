namespace MauiSampleApp;

/// <summary>
/// Lets the sample app override the OS theme so controls can be checked in light
/// and dark mode without leaving the app. The choice is persisted between launches.
/// </summary>
public static class ThemeService
{
    private const string PreferenceKey = "sample_app_theme";

    public static AppTheme Current
    {
        get => (AppTheme)Preferences.Default.Get(PreferenceKey, (int)AppTheme.Unspecified);
        private set => Preferences.Default.Set(PreferenceKey, (int)value);
    }

    /// <summary>Label for a toolbar item showing the active choice.</summary>
    public static string Label => Current switch
    {
        AppTheme.Light => "Theme: Light",
        AppTheme.Dark => "Theme: Dark",
        _ => "Theme: System",
    };

    /// <summary>Applies the persisted theme. Call once at startup.</summary>
    public static void ApplySaved() => Apply(Current);

    /// <summary>Cycles System → Light → Dark → System.</summary>
    public static void Cycle() => Apply(Current switch
    {
        AppTheme.Unspecified => AppTheme.Light,
        AppTheme.Light => AppTheme.Dark,
        _ => AppTheme.Unspecified,
    });

    private static void Apply(AppTheme theme)
    {
        Current = theme;

        if (Application.Current != null)
            Application.Current.UserAppTheme = theme;
    }
}
