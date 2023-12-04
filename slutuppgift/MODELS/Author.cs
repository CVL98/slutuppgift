using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slutuppgift.MODELS
{
    internal class Author
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public Author()
        {
            
        }
        public ICollection<Book> Books { get; set; }
    }
}
