using System.ComponentModel.DataAnnotations;

namespace slutuppgift.MODELS
{
    internal class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }

        public Card? Card { get; set; }
        public ICollection<History> Histories { get; set; }
    }
}
