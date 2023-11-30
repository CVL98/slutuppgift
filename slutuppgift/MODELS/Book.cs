using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public Guid Isbn { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public int Year { get; set; }
        public bool Borrowed { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        private int _grade;
        public int Grade 
        {
            get => _grade;
            set
            {
                if (value<1 || value>5) throw new ArgumentOutOfRangeException(nameof(value));
                _grade = value;
            }
        }
        public Book()
        {
            
        }

    }
}
