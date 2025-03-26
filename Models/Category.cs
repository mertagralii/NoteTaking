using System.ComponentModel.DataAnnotations;

namespace NoteTaking.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }


        public string? ImageURL { get; set; }
    }
}
