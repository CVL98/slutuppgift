using Helpers;
using Microsoft.EntityFrameworkCore;
using slutuppgift.MODELS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

        public void CreateFiller()
        {
            using (var context = new Context())
            {
                for (int i = 0; i < 10; i++)
                {
                    User user = new User();

                    user.FirstName = rnd.FirstName;
                    user.LastName = rnd.LastName;

                    Book book = new Book();
                    book.Year = rnd.Next(1900, 2023);
                    book.Title = GetEnumDescription(rnd.FromEnum<BookTitles>());

                    Card card = new Card();
                    Author author = new Author();

                    author.Name = rnd.FullName;

                    context.Users.Add(user);
                    context.Books.Add(book);
                    context.Authors.Add(author);
                    context.Cards.Add(card);
                }
                context.SaveChanges();
            }
        }

        public void MarkBookAsNotLoaned(int bookId)
        {
            using (var context = new Context())
            {
                var book = context.Books.Include(b => b.Card).FirstOrDefault(b => b.Id == bookId);

                if (book != null)
                {
                    // Update LoanCardId to null, marking the book as not loaned
                    book.Card = null; // Card_Id

                    // If the book was associated with a LoanCard, remove it from the LoanCard's collection
                    if (book.Card != null)
                    {
                        book.Card.Books.Remove(book);
                    }

                    // Save changes to the database
                    context.SaveChanges();
                }
            }
        }

        public void AddPersonToDatabase(string firstName, string lastName)
        {
            using (var context = new Context())
            {
                var user = new User
                {
                    FirstName = firstName,
                    LastName = lastName
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public void AddBookToDatabase(string title, params int[] authorIds)
        {
            using (var context = new Context())
            {
                var authors = context.Authors.Where(a => authorIds.Contains(a.Id)).ToList();

                var book = new Book
                {
                    Title = title,
                    Authors = authors,
                    Year = new Random().Next(1950, 2023)

                };

                context.Books.Add(book);
                context.SaveChanges();
            }
        }

        public void AddLoanCardToPerson(int id)
        {
            using (var context = new Context())
            {
                // Step 1: Retrieve the Person
                var user = context.Users.Find(id);

                if (user == null)
                {
                    // Handle the case where the person with the specified ID doesn't exist
                    // You can throw an exception, log a message, or take appropriate action
                    return;
                }

                // Step 2: Create a new LoanCard
                var card = new Card();

                // Step 3: Link the LoanCard to the Person
                user.Card = card;

                // Step 4: Save changes to the database
                context.SaveChanges();
            }
        }

        public void AddBookIdToPersonLoanCard(int userId, int bookId)
        {
            using (var context = new Context())
            {
                // Step 1: Retrieve the Person with LoanCard
                var user = context.Users.Include(p => p.Card).SingleOrDefault(p => p.Id == userId);

                if (user == null)
                {
                    // Handle the case where the person with the specified ID doesn't exist
                    // You can throw an exception, log a message, or take appropriate action
                    return;
                }

                // Step 2: Check if the person has a LoanCard
                if (user.Card == null)
                {
                    // Handle the case where the person doesn't have a LoanCard
                    // You can create a new LoanCard, associate it with the person, and proceed
                    return;
                }

                // Step 3: Link the existing book to the LoanCard using the book ID

                var book = context.Books.Find(bookId);

                if (book != null)
                {
                    // Assuming LoanCardId is the foreign key in the Book entity
                    book.Card = user.Card;
                    context.SaveChanges(); // Save changes to the book
                }
            }
        }

        public void Clear()
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
                context.RemoveRange(CardsList);
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

