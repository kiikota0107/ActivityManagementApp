using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.ViewModels
{
    public class ActivityInputViewModel
    {
        [MaxLength(100, ErrorMessage = "タスクタイトルは100文字以内で入力してください。")]
        public string ActivityDetailTitle { get; set; } = "";

        [MaxLength(1000, ErrorMessage = "タスク詳細は1000文字以内で入力してください。")]
        public string ActivityDetail { get; set; } = "";
    }
}
