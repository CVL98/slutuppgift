using System.ComponentModel.DataAnnotations;

namespace slutuppgift.MODELS
{
    internal class History
    {
        [Key]
        public int Id { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime? Returned { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }

    }
}
