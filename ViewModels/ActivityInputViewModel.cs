using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.ViewModels
{
    public class ActivityInputViewModel
    {
        [MaxLength(10, ErrorMessage = "タスクタイトルは100文字以内で入力してください。")]
        public string ActivityDetailTitle { get; set; } = "";

        [MaxLength(10, ErrorMessage = "タスク詳細は1000文字以内で入力してください。")]
        public string ActivityDetail { get; set; } = "";
    }
}
