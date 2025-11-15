using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.Models
{
    public class CategoryMaster
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; } = "";
        public int CategoryType { get; set; }
        public string UserId { get; set; } = default!;
    }
}
