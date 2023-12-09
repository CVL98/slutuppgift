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
            //optionsBuilder.UseSqlServer("Server=localhost; Database=NewtonLibraryCV;" +
            //                            " Trusted_Connection=True; Trust Server Certificate =Yes;" +
            //                            " User Id=NewtonLibrary; password=NewtonLibrary");
            optionsBuilder.UseSqlServer("Server=tcp:newton-db-server-cv.database.windows.net,1433;Initial Catalog=newton-db-cv;Persist Security Info=False;User ID=CVL;Password=HejJens123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
