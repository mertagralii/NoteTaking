using System.ComponentModel.DataAnnotations;

namespace NoteTaking.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Lütfen kategori giriniz.")]
        
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

        [Required (ErrorMessage ="Lütfen Başlık Giriniz.")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Lütfen Açıklama Giriniz.")]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public bool IsArchive { get; set; } = false;


    }
}
