using System.ComponentModel.DataAnnotations;

public class CategoryTypeEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "カテゴリタイプ名は必須です")]
    public string TypeName { get; set; } = "";

    [Required(ErrorMessage = "背景色は必須です")]
    public string ColorKey { get; set; } = "";

    [Required(ErrorMessage = "文字色は必須です")]
    public string TextColorKey { get; set; } = "";

    public int SortOrder { get; set; }
}