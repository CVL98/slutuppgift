using System.ComponentModel.DataAnnotations;

namespace slutuppgift.MODELS
{
    internal class Author
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
