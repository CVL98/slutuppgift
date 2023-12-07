using Microsoft.EntityFrameworkCore;
using slutuppgift.MODELS;

namespace slutuppgift.DATA
{
    internal class Context : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<History> Histories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=NewtonLibraryCV;" +
                                        " Trusted_Connection=True; Trust Server Certificate =Yes;" +
                                        " User Id=NewtonLibrary; password=NewtonLibrary");
        }
    }
}
