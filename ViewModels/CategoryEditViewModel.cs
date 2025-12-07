using System.ComponentModel.DataAnnotations;

public class CategoryEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "カテゴリ名は必須です")]
    public string CategoryName { get; set; } = "";

    [Required(ErrorMessage = "カテゴリタイプは必須です")]
    public int CategoryTypeMasterId { get; set; }

    public int SortOrder { get; set; }
    public bool SkipAppLockOnActiveFlg { get; set; }
}