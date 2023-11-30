using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace slutuppgift.MODELS
{
    internal class Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Card {  get; set; }
        public int Pin {  get; set; } = new Random().Next(1000, 9999);
        public Users()
        {
            
        }
    }
}
