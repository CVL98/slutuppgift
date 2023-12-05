using Helpers;
using Microsoft.EntityFrameworkCore;
using slutuppgift.MODELS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

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
            Console.WriteLine("1. Fill 10");
            Console.WriteLine("2. Add new author");
            Console.WriteLine("3. Add new book");
            Console.WriteLine("4. Add new user");
            Console.WriteLine("5. Borrow a book");
            Console.WriteLine("6. Return a book");
        }
        public void NewBook()
        {
            using (var context = new Context())
            {
                Book book = new Book();
                book.Year = rnd.Next(1900, 2023);
                book.Rating = new Random().Next(0, 5 + 1);
                book.Title = GetEnumDescription(rnd.FromEnum<BookTitles>());

                context.Books.Add(book);
                context.SaveChanges();
            }

        }
        public void NewUser()
        {
            using (var context = new Context())
            {
                Card card = new Card();
                card.Pin = (new Random().Next(0, 9999)).ToString("D4");
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

        public void Fill(int number)
        {
            using (var context = new Context())
            {
                for (int i = 0; i < number; i++)
                {
                    NewBook();
                    NewUser();
                    NewAuthor();
                }
            }
        }

        public void ReturnBook(int bookId)
        {
            using (var context = new Context())
            {
                var book = context.Books.Include(b => b.Card).FirstOrDefault(b => b.Id == bookId);

                if (book != null)
                {
                    book.Card = null;
                    book.LoanDate = null;
                    book.ReturnDate = null;

                    context.SaveChanges();
                }
            }
        }

        public void NewUser(string firstName, string lastName)
        {
            using (var context = new Context())
            {
                Card card = new Card();
                card.Pin = (new Random().Next(0, 9999)).ToString("D4");
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

        public void NewUserCard(int id)
        {
            using (var context = new Context())
            {
                var user = context.Users.Find(id);

                if (user == null)
                {
                    Console.WriteLine("User:404");
                    return;
                }

                var card = new Card();
                user.Card = card;
                user.Card.Pin = (new Random().Next(0, 9999)).ToString("D4");

                context.SaveChanges();
            }
        }

        public void BorrowBook(int userId, int bookId)
        {
            using (var context = new Context())
            {
                var user = context.Users.Include(p => p.Card).SingleOrDefault(p => p.Id == userId);

                if (user?.Card == null) return;

                var book = context.Books.Find(bookId);

                if (book != null)
                {
                    book.Card = user.Card;
                    book.LoanDate = DateTime.Now;
                    book.ReturnDate = DateTime.Now.AddDays(14);
                    context.SaveChanges();
                }
            }
        }

        public void ClearTables()
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
                //context.RemoveRange(CardsList);
                context.SaveChanges();
            }
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

