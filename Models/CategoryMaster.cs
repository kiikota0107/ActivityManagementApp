using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.Models
{
    public class CategoryMaster
    {
        [Key]
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string CategoryName { get; set; } = "";
        public int CategoryTypeMasterId { get; set; }
        public CategoryTypeMaster CategoryTypeMaster { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}
