using System.ComponentModel.DataAnnotations;

namespace NoteTaking.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public bool IsArchive { get; set; } = false;


    }
}
