using Helpers;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using slutuppgift.DATA;
using slutuppgift.MODELS;

namespace slutuppgift
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            DataAccess access = new DataAccess();

            //access.Fill(10);
            //access.clearTables();
            //access.NewUserCard(3);
            //access.NewBook("Stefan Bok",6,7);
            //access.ReturnBook(9);
            //access.BorrowBook(6,9);

            int choice;
            DataAccess.menu();
            do
            {
                Console.WriteLine("\nEnter your choice (0 to exit):");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        access.Fill(10);
                        Console.Clear();
                        DataAccess.menu();
                        Console.WriteLine("\n10 users, authors and books added");
                        break; //Fill 10
                    case 2:
                        Console.Write("\nEnter User surname:");
                        var surname = Console.ReadLine();
                        Console.Write("\nEnter User lastname:");
                        var lastname = Console.ReadLine();
                        if (surname.IsNullOrEmpty() || lastname.IsNullOrEmpty())
                        {
                            Console.Clear();
                            DataAccess.menu();
                            Console.WriteLine("\nInvalid name");
                            break;
                        }
                        access.NewUser(surname, lastname);
                        Console.Clear();
                        DataAccess.menu();
                        Console.WriteLine($"\nUser \"{surname} {lastname}\" added.");
                        break; //Enter user
                    case 3:
                        Console.Write("\nEnter book title:");
                        var title = Console.ReadLine();

                        using (var context = new Context())
                        {
                            var authors = context.Authors;
                            var counter = 0;
                            foreach (var author in authors)
                            {
                                counter++;
                                Console.Write($"{author.Id,-2}. {author.Name,-19}");
                                if (counter % 3 == 0) Console.WriteLine();
                            }
                        }

                        Console.Write("\nEnter book author(s):");
                        var authorIds = Console.ReadLine();
                        if (title.IsNullOrEmpty() || authorIds.IsNullOrEmpty())
                        {
                            Console.Clear();
                            DataAccess.menu();
                            Console.WriteLine("\nInvalid book");
                            break;
                        }
                        char[] separators = new char[] { ' ', ',' };
                        string[] inputArray = authorIds.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        int[] Ids = new int[inputArray.Length];
                        bool invalidInput = false;
                        for (int i = 0; i < inputArray.Length; i++)
                        {
                            if (int.TryParse(inputArray[i], out Ids[i]))
                            {
                            }
                            else
                            {
                                Console.Clear();
                                DataAccess.menu();
                                Console.WriteLine("\nInvalid author Id");
                                invalidInput = true;
                                break;
                            }
                        }
                        if (invalidInput) break;

                        access.NewBook(title, Ids);
                        Console.Clear();
                        DataAccess.menu();

                        using (var context = new Context())
                        {
                            Console.Write($"\nBook \"{title}\" by ");
                            for (int i = 0; i < Ids.Length; i++)
                            {
                                var author = context.Authors.FirstOrDefault(a => a.Id == Ids[i]);
                                Console.Write($"{author.Name}");
                                if (i < Ids.Length - 2) Console.Write(", ");
                                if (i == Ids.Length - 2) Console.Write(" and ");
                            }
                            Console.Write(" added.\n");
                        }
                        break; //Enter book
                    case 4:
                        using (var context = new Context())
                        {
                            var users = context.Users;
                            var counter = 0;
                            Console.WriteLine();
                            foreach (var user in users)
                            {
                                counter++;
                                Console.Write($"{user.Id,-2}. {user.FirstName,-11} {user.LastName,-11}");
                                if (counter % 2 == 0) Console.WriteLine();
                            }
                        }
                        Console.Write("\nEnter User id:");
                        int.TryParse(Console.ReadLine(), out var userid);
                        Console.Clear();
                        using (var context = new Context())
                        {
                            var books = context.Books;
                            var counter = 0;
                            foreach (var book in books)
                            {
                                counter++;
                                Console.Write($"{book.Id,-2}. {book.Title,-39}");
                                if (counter % 2 == 0) Console.WriteLine();
                            }
                        }
                        Console.Write("\nEnter book id:");
                        int.TryParse(Console.ReadLine(), out var bookid);

                        if (userid == 0 || bookid == 0)
                        {
                            Console.Clear();
                            DataAccess.menu();
                            Console.WriteLine("\nInvalid user or book id");
                        }

                        access.BorrowBook(userid, bookid);
                        Console.Clear();
                        DataAccess.menu();
                        Console.WriteLine("\n Book borrowed");
                        break; //Borrow Book
                    case 5:
                        using (var context = new Context())
                        {
                            var booksWithCardId = context.Books.Where(book => book.Card != null);

                            var counter = 0;
                            foreach (var book in booksWithCardId)
                            {
                                counter++;
                                Console.Write($"{book.Id,-2}. {book.Title,-39}");
                                if (counter % 2 == 0) Console.WriteLine();
                            }
                        }
                        Console.Write("\nEnter book id:");
                        int.TryParse(Console.ReadLine(), out var rebookid);
                        if (rebookid == 0)
                        {
                            Console.Clear();
                            DataAccess.menu();
                            Console.WriteLine("\nInvalid book id");
                            break;
                        }
                        access.ReturnBook(rebookid);
                        Console.Clear();
                        DataAccess.menu();
                        Console.WriteLine("\n Book returned");
                        break; //Return Book
                        
                    case 0:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            } while (choice != 0);
        }
    }
}

// cd slutuppgift
// dotnet ef migrations add InitialMigration
// dotnet ef database update