using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace slutuppgift.MODELS
{
    internal class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public bool Card {  get; set; }
        public string Pin {  get; set; } = (new Random().Next(1000, 9999)).ToString("D4");
        public User()
        {
            
        }
    }
}
