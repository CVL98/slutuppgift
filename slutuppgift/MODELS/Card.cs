using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slutuppgift.MODELS
{
    internal class Card
    {
        public int Id {  get; set; }
        public string Pin { get; set; } = (new Random().Next(0, 9999)).ToString("D4");
        public ICollection<Book> Books { get; set;}
    }
}
