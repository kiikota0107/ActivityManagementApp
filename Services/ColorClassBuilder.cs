namespace ActivityManagementApp.Services
{
    public static class ColorClassBuilder
    {
        // カードヘッダ・バッジ用
        public static string BuildBackgroundBadgeClass(string colorKey, string textColorKey)
        {
            return $"bg-{colorKey} text-{textColorKey}";
        }

        // ボタン（outline）
        public static string BuildOutlineButtonClass(string colorKey)
        {
            return $"btn btn-outline-{colorKey}";
        }

        // ボタン（filled）
        public static string BuildFilledButtonClass(string colorKey)
        {
            return $"btn btn-{colorKey}";
        }
    }
}
