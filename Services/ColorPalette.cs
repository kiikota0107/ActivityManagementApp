namespace ActivityManagementApp.Services
{
    public class ColorDefinition
    {
        public string Key { get; set; } = "";
        public string Label { get; set; } = "";
        public string TextColor { get; set; } = "";
    }

    public static class ColorPalette
    {
        public static readonly List<ColorDefinition> All = new()
        {
            new ColorDefinition { Key = "primary",   Label = "Primary（青）",       TextColor = "white" },
            new ColorDefinition { Key = "secondary", Label = "Secondary（グレー）", TextColor = "white" },
            new ColorDefinition { Key = "success",   Label = "Success（緑）",       TextColor = "white" },
            new ColorDefinition { Key = "danger",    Label = "Danger（赤）",        TextColor = "white" },
            new ColorDefinition { Key = "warning",   Label = "Warning（黄）",       TextColor = "dark" },
            new ColorDefinition { Key = "info",      Label = "Info（水色）",        TextColor = "dark" },
            new ColorDefinition { Key = "light",     Label = "Light（薄グレー）",   TextColor = "dark" },
            new ColorDefinition { Key = "dark",      Label = "Dark（黒）",          TextColor = "white" }
        };

        public static string GetTextColor(string colorKey)
        {
            return All.FirstOrDefault(x => x.Key == colorKey)?.TextColor ?? "white";
        }

        public static string GetLabel(string colorKey)
        {
            return All.FirstOrDefault(x => x.Key == colorKey)?.Label ?? colorKey;
        }
    }
}
