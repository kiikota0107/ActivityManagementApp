using System.ComponentModel.DataAnnotations;

namespace ActivityManagementApp.Models
{
    public class CategoryTypeMaster : IHasUserId
    {
        [Key]
        public int Id { get; set; }
        public int SortOrder { get; set; }
        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
        public string ColorKey { get; set; } = "secondary";
        public string TextColorKey { get; set; } = "white";
    }
}
