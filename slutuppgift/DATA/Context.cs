using Microsoft.EntityFrameworkCore;
using slutuppgift.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slutuppgift.DATA
{
    internal class Context : DbContext
    {
        public DbSet<Books> Books { get; set; }
        public DbSet<Authors> Authors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=NewtonLibraryCV;" +
                                        " Trusted_Connection=True; Trust Server Certificate =Yes;" +
                                        " User Id=NewtonLibrary; password=NewtonLibrary");
        }
    }
}
