using System.ComponentModel.DataAnnotations;

namespace slutuppgift.MODELS
{
    internal class Card
    {
        [Key]
        public int Id {  get; set; }

        public string Pin { get; set; }

        public ICollection<Book>? Books { get; set;}
    }
}
