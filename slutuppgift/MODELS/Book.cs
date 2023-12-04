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
        public int Id { get; set; }
        public Guid Isbn { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public int Year { get; set; }
        private bool _borrowed = false;
        public bool Borrowed
        {
            get
            {
                
                if (!_borrowed && Card!=null)
                {
                    LoanDate = DateTime.Now;
                    ReturnDate = DateTime.Now.AddDays(14);
                }
                if (_borrowed && Card == null)
                {
                    LoanDate = null;
                    ReturnDate = null;
                }
                if (Card==null) _borrowed = false;
                else if (Card!=null) _borrowed = true;
                return _borrowed;
            }
        }
        private DateTime? _loandate;
        public DateTime? LoanDate
        {
            get
            {
                if (!Borrowed) LoanDate = null;
                return _loandate;
            }
            set => _loandate = value;
        }
        private DateTime? _returndate;
        public DateTime? ReturnDate
        {
            get
            {
                if (!Borrowed) _returndate = null;
                return _returndate;
            }
            set => _returndate = value;
        }
        private int _grade;
        public int Grade
        {
            get => _grade;
            set
            {
                if (value < 1 || value > 5) throw new ArgumentOutOfRangeException(nameof(value));
                _grade = value;
            }
        }
        public ICollection<Author>? Authors { get; set; }
        //public int? Card_Id { get; set; }
        public Card? Card { get; set; }
        public Book()
        {

        }
    }
}
