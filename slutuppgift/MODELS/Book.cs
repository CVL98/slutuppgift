using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
