using Helpers;
using Microsoft.EntityFrameworkCore;
using slutuppgift.MODELS;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace slutuppgift.DATA
{
    internal class DataAccess
    {
        csSeedGenerator rnd = new csSeedGenerator();


        public enum BookTitles
        {
            [Description("Metro 2033")] Metro, [Description("Lord of the rings")] Lotr, [Description("Judge Dredd")] Dredd,
            [Description("Game Of Thrones")] GOT, [Description("Silent Hill")] SH, [Description("Batman Bin Suparman")] FMARVEL, Halo,
            [Description("The Picture of Dorian Gray")] DG, [Description("Never Let Me Go")] Never, [Description("The Road")] VÄG, [Description("March: Book One (Oversized Edition)")] Stor, [Description("The Hobbit")] Liten,
            [Description("Pride and Prejudice")] TroddeDettaVaEnFilm, [Description("A Tale of Two Cities")] SimCity, [Description("Crime and Punishment")] HörtDennaVaBra, [Description("We Should All Be Feminists")] IVissaFall, [Description("Persepolis")] NuräckerD,
        }
        public static void menu()
        {

            Console.WriteLine("\n1. Fill 10---------------");
            Console.WriteLine("2. Add new author--------");
            Console.WriteLine("3. Add new book----------");
            Console.WriteLine("4. Add new user----------");
            Console.WriteLine("5. List data----[options]");
            Console.WriteLine("6. User data------[Login]");
            Console.WriteLine("7. Borrow a book---------");
            Console.WriteLine("8. Return a book---------");
            Console.WriteLine("9. Remove data--[options]");


        }
        public static void Home(string text)
        {
            Console.Clear();
            menu();
            Console.WriteLine();
            if (text != null || text != "") Console.Write(text + "\n");
        }

        public void Fill(int number)
        {
            using (var context = new Context())
            {
                for (int i = 0; i < number; i++) NewAuthor();
                for (int i = 0; i < number; i++) NewBook();
                for (int i = 0; i < number; i++) NewUser();
            }
        }

        public void ListAuthors()
        {
            using (var context = new Context())
            {
                var authors = context.Authors.ToList();
                var counter = 0;
                Console.WriteLine();
                foreach (var author in authors)
                {
                    var books = context.Books.Include(b => b.Authors).Where(b => b.Authors.Any(a => a.Id == author.Id));
                    counter++;
                    Console.Write($"Author:{author.Id,3}. {author.Name,-19} Books: ");
                    foreach (var book in books)
                    {
                        Console.Write($"({book.Id}),");
                    }
                    if (counter % 1 == 0) Console.WriteLine();
                }
            }
        }
        public void ListUsers()
        {
            using (var context = new Context())
            {
                var users = context.Users.Include(c => c.Card);
                var counter = 0;
                Console.WriteLine();
                foreach (var user in users)
                {
                    counter++;
                    Console.Write($"User:{user.Id,3}. {user.FirstName,-12} {user.LastName,-12} Password: {user.Card.Pin,12}\t");
                    if (counter % 1 == 0) Console.WriteLine();
                }
            }
        }
        public void ListBooks()
        {
            using (var context = new Context())
            {
                var books = context.Books.Include(c => c.Card).ToList();

                var counter = 0;
                Console.WriteLine();

                foreach (var book in books)
                {
                    var user = context.Users.Include(u => u.Card).FirstOrDefault(u => u.Card == book.Card);
                    string status = "[Borrowed]";
                    if (book.Borrowed == false) status = "[Available]";
                    counter++;
                    Console.Write($"Book:{book.Id,3}. {book.Title,-36} Status:{status,11} ");
                    if (user != null) Console.Write($"- ({user.Id}) {user.FirstName} {user.LastName}");
                    if (counter % 1 == 0) Console.WriteLine();
                }
            }
        }

        public void NewBook()
        {
            using (var context = new Context())
            {
                var authors = context.Authors.ToList();


                Book book = new Book();
                book.Year = rnd.Next(1900, 2023);
                book.Borrowed = false;
                book.Rating = new Random().Next(0, 5 + 1);
                book.Title = GetEnumDescription(rnd.FromEnum<BookTitles>());
                int numberOfAuthors = rnd.Next(1, authors.Count + 1);
                book.Authors = authors.OrderBy(a => Guid.NewGuid()).Take(numberOfAuthors).ToList();

                context.Books.Add(book);
                context.SaveChanges();
            }

        }
        public void NewUser()
        {
            using (var context = new Context())
            {
                Card card = new Card();
                card.Pin = Encrypt((new Random().Next(0, 9999)).ToString("D4"));
                User user = new User();
                user.FirstName = rnd.FirstName;
                user.LastName = rnd.LastName;
                user.Card = card;

                context.Cards.Add(card);
                context.Users.Add(user);
                context.SaveChanges();
            }

        }
        public void NewAuthor()
        {
            using (var context = new Context())
            {
                Author author = new Author();
                author.Name = rnd.FullName;

                context.Authors.Add(author);
                context.SaveChanges();
            }

        }
        public void NewUser(string firstName, string lastName, string pin)
        {
            using (var context = new Context())
            {
                Card card = new Card();
                card.Pin = Encrypt(pin);
                User user = new User();
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Card = card;

                context.Cards.Add(card);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public void NewBook(string title, params int[] authorIds)
        {
            using (var context = new Context())
            {
                var authors = context.Authors.Where(a => authorIds.Contains(a.Id)).ToList();

                var book = new Book
                {
                    Title = title,
                    Authors = authors,

                    Year = new Random().Next(1950, 2023),
                    Rating = new Random().Next(0, 5 + 1),
                    Borrowed = false

                };
                context.Books.Add(book);

                context.SaveChanges();
            }
        }
        public void NewAuthor(string fullname)
        {
            using (var context = new Context())
            {
                Author author = new Author();
                author.Name = fullname;

                context.Authors.Add(author);
                context.SaveChanges();
            }

        }

        public bool RemoveBook(int bookId)
        {
            using (var context = new Context())
            {
                var book = context.Books.Find(bookId);

                if (book == null)
                {
                    Console.WriteLine("Book:404");
                    return false;
                }
                context.Books.Remove(book);
                context.SaveChanges();
                return true;
            }
        }

        public bool RemoveUser(int userId)
        {
            using (var context = new Context())
            {
                var user = context.Users.Include(c => c.Card).SingleOrDefault(p => p.Id == userId);

                if (user == null)
                {
                    Console.WriteLine("User:404");
                    return false;
                }

                var books = context.Books.Where(b => b.Card == user.Card).ToList();
                foreach (var book in books)
                {
                    ReturnBook(book.Id);
                }
                if (user.Card != null)
                {

                    context.Cards.Remove(user.Card);

                }

                context.Users.Remove(user);
                context.SaveChanges();
                return true;
            }
        }

        public bool RemoveAuthor(int authorId)
        {
            using (var context = new Context())
            {
                var author = context.Authors.Find(authorId);

                if (author == null)
                {
                    Console.WriteLine("Author:404");
                    return false;
                }
                context.Authors.Remove(author);
                context.SaveChanges();
                return true;
            }
        }
        public void RemoveAll()
        {
            using (var context = new Context())
            {
                var UsersList = context.Users.ToList();
                context.Users.RemoveRange(UsersList);
                var BooksList = context.Books.ToList();
                context.Books.RemoveRange(BooksList);
                var AuthorsList = context.Authors.ToList();
                context.Authors.RemoveRange(AuthorsList);
                var CardsList = context.Cards.ToList();
                context.Cards.RemoveRange(CardsList);
                context.RemoveRange(CardsList);
                context.SaveChanges();
            }
        }

        public bool BorrowBook(int userId, int bookId)
        {
            using (var context = new Context())
            {
                var user = context.Users.Include(p => p.Card).SingleOrDefault(p => p.Id == userId);


                var book = context.Books.Find(bookId);

                if (book != null && !book.Borrowed)
                {
                    book.Card = user.Card;
                    book.LoanDate = DateTime.Now;
                    book.ReturnDate = DateTime.Now.AddDays(14);
                    book.Borrowed = true;

                    var History = new History
                    {
                        BorrowDate = DateTime.Now,
                        UserId = userId,
                        BookId = bookId
                    };
                    context.Histories.Add(History);
                    context.SaveChanges();
                    return true;
                }
                Console.WriteLine("\nBook is not available.");
                return false;
            }
        }
        public void ReturnBook(int bookId)
        {
            using (var context = new Context())
            {
                var book = context.Books.Include(b => b.Card).FirstOrDefault(b => b.Id == bookId);

                if (book != null)
                {
                    var borrowHistory = context.Histories.FirstOrDefault(bh => bh.BookId == bookId && bh.Returned == null);

                    if (borrowHistory != null)
                    {
                        borrowHistory.Returned = DateTime.Now;
                    }

                    book.Card = null;
                    book.LoanDate = null;
                    book.ReturnDate = null;
                    book.Borrowed = false;

                    context.SaveChanges();
                }
            }
        }



        public static string Encrypt(string text)
        {
            if (text == null) return null;
            string key = "thisisnotakey";
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptedBytes = new byte[textBytes.Length];

            for (int i = 0; i < textBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return Convert.ToBase64String(encryptedBytes);
        }
        public static string Decrypt(string text)
        {
            if (text == null) return null;
            string key = "thisisnotakey";
            byte[] textBytes = Convert.FromBase64String(text);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptedBytes = new byte[textBytes.Length];

            for (int i = 0; i < textBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return Encoding.UTF8.GetString(encryptedBytes);
        }







        private string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return value.ToString();
        }
    }
}

