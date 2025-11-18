namespace ActivityManagementApp.Services
{
    public class DefaultCategoryGroup
    {
        public string TypeName { get; set; } = "";
        public string ColorKey { get; set; } = "";
        public string TextColorKey { get; set; } = "";
        public List<string> Categories { get; set; } = new();
    }

    public static class DefaultCategorySeed
    {
        public static readonly List<DefaultCategoryGroup> Groups = new()
        {
            new DefaultCategoryGroup
            {
                TypeName = "生活",
                ColorKey = "info",
                TextColorKey = "dark",
                Categories = new()
                {
                    "食事", "歯磨き", "食器洗い", "入浴",
                    "ボディケア", "身支度", "片付け", "掃除",
                    "移動"
                }
            },
            new DefaultCategoryGroup
            {
                TypeName = "娯楽",
                ColorKey = "warning",
                TextColorKey = "dark",
                Categories = new()
                {
                    "SNS", "動画", "ゲーム", "ネット"
                }
            },
            new DefaultCategoryGroup
            {
                TypeName = "成長",
                ColorKey = "success",
                TextColorKey = "white",
                Categories = new()
                {
                    "勉強", "運動"
                }
            },
            new DefaultCategoryGroup
            {
                TypeName = "仕事",
                ColorKey = "primary",
                TextColorKey = "white",
                Categories = new()
                {
                    "業務", "回顧"
                }
            },
            new DefaultCategoryGroup
            {
                TypeName = "睡眠",
                ColorKey = "secondary",
                TextColorKey = "white",
                Categories = new()
                {
                    "就寝", "昼寝"
                }
            },
            new DefaultCategoryGroup
            {
                TypeName = "外出",
                ColorKey = "danger",
                TextColorKey = "white",
                Categories = new()
                {
                    "散歩", "旅行", "ドライブ", "ショッピング", "外食"
                }
            }
        };
    }
}
