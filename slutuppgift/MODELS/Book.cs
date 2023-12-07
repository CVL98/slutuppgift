using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace slutuppgift.MODELS
{
    internal class Book
    {
        [Key]
        public int Id { get; set; }

        public Guid Isbn { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public int Year { get; set; }
        public int Rating { get; set; }
        public bool Borrowed { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        

        public ICollection<Author>? Authors { get; set; }
        public Card? Card { get; set; }
    }
}
