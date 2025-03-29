using System.ComponentModel.DataAnnotations;

namespace NoteTaking.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required (ErrorMessage ="Lütfen Kategoriye bir isim veriniz.")]
        public required string Name { get; set; }

        [Required (ErrorMessage ="Lütfen Kategoriye bir resim URL'si girin.")]
        public string? ImageURL { get; set; }
    }
}
