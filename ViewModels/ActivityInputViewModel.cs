using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.ViewModels
{
    public class ActivityInputViewModel
    {
        [MaxLength(150, ErrorMessage = "タスクタイトルは150文字以内で入力してください。")]
        public string ActivityDetailTitle { get; set; } = "";

        [MaxLength(10000, ErrorMessage = "タスク詳細は10000文字以内で入力してください。")]
        public string ActivityDetail { get; set; } = "";
    }
}
