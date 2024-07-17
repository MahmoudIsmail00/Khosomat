using System.ComponentModel.DataAnnotations;

namespace Khosomat.Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.Now;
    }
}
