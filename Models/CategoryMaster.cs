using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.Models
{
    public class CategoryMaster
    {
        // ★★★変更した場合はマイグレーション実行する★★★
        [Key]
        public int Id { get; set; }
        public string? CategoryName { get; set; }
    }
}
