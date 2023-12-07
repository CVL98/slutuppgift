using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using slutuppgift.DATA;

namespace slutuppgift
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            DataAccess access = new DataAccess();
            int choice;
            DataAccess.menu();
            do
            {
                Console.WriteLine("\nEnter your choice (0 or empty to exit):");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        access.Fill(10);
                        DataAccess.Home("10 users, authors and books added");
                        break; //Fill 10
                    case 4:
                        Console.Write("\nEnter User surname:");
                        var surname = Console.ReadLine();
                        Console.Write("\nEnter User lastname:");
                        var lastname = Console.ReadLine();
                        Console.Write("\nCreate User pin:");
                        if (surname.IsNullOrEmpty() || lastname.IsNullOrEmpty() || !int.TryParse(Console.ReadLine(), out int pin))
                        {
                            DataAccess.Home("Invalid name or pin");
                            break;
                        }
                        access.NewUser(surname, lastname, pin);
                        DataAccess.Home($"\nUser \"{surname} {lastname}\" added.");
                        break; //Enter user
                    case 3:
                        DataAccess.Home("Enter book title:");
                        var title = Console.ReadLine();

                        DataAccess.Home("");
                        access.ListAuthors();
                        Console.Write("\nEnter book author(s):");
                        var authorIds = Console.ReadLine();
                        if (title.IsNullOrEmpty() || authorIds.IsNullOrEmpty())
                        {
                            DataAccess.Home("Invalid book");
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
                                DataAccess.Home("Invalid author Id");
                                invalidInput = true;
                                break;
                            }
                        }
                        if (invalidInput) break;

                        access.NewBook(title, Ids);
                        DataAccess.Home("");

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
                    case 7:
                        DataAccess.Home("");
                        access.ListUsers();

                        Console.Write("\nEnter User id:");
                        int.TryParse(Console.ReadLine(), out var userid);
                        DataAccess.Home("");
                        using (var context = new Context())
                        {
                            var booksWithCardId = context.Books.Where(book => book.Card == null);
                            if (booksWithCardId.Count() == 0)
                            {
                                Console.WriteLine("No books are available.");
                                break;
                            }
                            var counter = 0;
                            foreach (var book in booksWithCardId)
                            {
                                counter++;
                                Console.Write($"{book.Id,3}. {book.Title,-36}");
                                if (counter % 2 == 0) Console.WriteLine();
                            }
                            if (booksWithCardId.Count() % 2 != 0) Console.WriteLine();
                        }
                        Console.Write("\nEnter book id:");
                        int.TryParse(Console.ReadLine(), out var bookid);

                        if (userid == 0 || bookid == 0)
                        {
                            DataAccess.Home("Invalid user or book id");
                            break;
                        }

                        DataAccess.Home("");
                        if (access.BorrowBook(userid, bookid))
                        {
                            Console.WriteLine("Book borrowed");
                            break;
                        }
                        break; //Borrow Book
                    case 8:
                        DataAccess.Home("");
                        using (var context = new Context())
                        {
                            var booksWithCardId = context.Books.Where(book => book.Card != null);
                            if (booksWithCardId.Count() == 0)
                            {
                                Console.WriteLine("No books are borrowed.");
                                break;
                            }
                            var counter = 0;
                            foreach (var book in booksWithCardId)
                            {
                                counter++;
                                Console.Write($"{book.Id,3}. {book.Title,-36}");
                                if (counter % 2 == 0) Console.WriteLine();
                            }
                            if (booksWithCardId.Count() % 2 != 0) Console.WriteLine();
                        }
                        Console.Write("\nSelect book to return:");
                        int.TryParse(Console.ReadLine(), out var rebookid);
                        if (rebookid == 0)
                        {
                            DataAccess.Home("Invalid book id.");
                            break;
                        }
                        access.ReturnBook(rebookid);
                        DataAccess.Home("Book returned");
                        break; //Return Book
                    case 2:
                        DataAccess.Home("");
                        Console.Write("Enter author fullname:");
                        var fullname = Console.ReadLine();
                        if (fullname.IsNullOrEmpty())
                        {
                            Console.Clear();
                            DataAccess.menu();
                            Console.WriteLine("\nInvalid name");
                            break;
                        }
                        access.NewAuthor(fullname);
                        DataAccess.Home($"\nAuthor \"{fullname}\" added.");
                        break; //Enter Author
                    case 9:
                        Console.Clear();
                        DataAccess.menu();
                        Console.WriteLine("\nRemove Author[1], Book[2], User[3] or clear all[4]?");
                        if (!int.TryParse(Console.ReadLine(), out int selection))
                        {
                            DataAccess.Home("Invalid.");
                            break;
                        }
                        switch (selection)
                        {
                            case 1:
                                DataAccess.Home("");
                                access.ListAuthors();
                                Console.Write($"\nSelect author to remove:");
                                if (!int.TryParse(Console.ReadLine(), out int author_id))
                                {
                                    Console.Clear();
                                    DataAccess.menu();
                                    Console.WriteLine();
                                    Console.WriteLine("Invalid author id.");
                                    break;
                                }
                                DataAccess.Home("");
                                if (!access.RemoveAuthor(author_id)) Console.WriteLine("Author not found.");
                                else Console.WriteLine("Author removed.");
                                break;
                            case 2:
                                DataAccess.Home("");
                                access.ListBooks();
                                Console.Write($"\nSelect book to remove:");
                                if (!int.TryParse(Console.ReadLine(), out int book_id))
                                {
                                    DataAccess.Home("Invalid book id.");
                                    break;
                                }
                                DataAccess.Home("");
                                if (!access.RemoveBook(book_id)) Console.WriteLine("Book not found.");
                                else Console.WriteLine("Book removed.");
                                break;
                            case 3:
                                DataAccess.Home("");
                                access.ListUsers();

                                Console.Write($"\nSelect user to remove:");
                                if (!int.TryParse(Console.ReadLine(), out int user_id))
                                {
                                    DataAccess.Home("Invalid user id");
                                    break;
                                }
                                DataAccess.Home("");
                                if (!access.RemoveUser(user_id)) Console.WriteLine("User not found.");
                                else Console.WriteLine("User removed.");
                                break;
                            case 4:
                                access.RemoveAll();
                                DataAccess.Home("All data cleared.");
                                break;
                            default:
                                DataAccess.Home("Invalid option");
                                break;
                        }
                        break; //Remove Data
                    case 5:
                        Console.Clear();
                        DataAccess.menu();
                        Console.WriteLine("\nList Authors[1], Books[2], Users[3] or all[4]?");
                        if (!int.TryParse(Console.ReadLine(), out int listchoice))
                        {
                            DataAccess.Home("Invalid.");
                            break;
                        }
                        switch (listchoice)
                        {
                            case 1:
                                DataAccess.Home("");
                                access.ListAuthors();
                                break;
                            case 2:
                                DataAccess.Home("");
                                access.ListBooks();
                                break;
                            case 3:
                                DataAccess.Home("");
                                access.ListUsers();
                                break;
                            case 4:
                                DataAccess.Home("");
                                Console.WriteLine("\nAuthors:");
                                access.ListAuthors();
                                Console.WriteLine("\nBooks:");
                                access.ListBooks();
                                Console.WriteLine("\nUsers:");
                                access.ListUsers();
                                Console.WriteLine();
                                DataAccess.menu();
                                break;
                            default:
                                DataAccess.Home("Invalid option.");
                                break;
                        }
                        break; //List Data
                    case 6:
                        using (var context = new Context())
                        {
                            DataAccess.Home("");
                            access.ListUsers();

                            Console.Write("\nEnter User id:");
                            int.TryParse(Console.ReadLine(), out var userId);
                            var user = context.Users.Include(c => c.Card).SingleOrDefault(p => p.Id == userId);
                            if (user == null)
                            {
                                DataAccess.Home("Invalid user id.");
                                break;
                            }
                            DataAccess.Home("Enter pin:");
                            var password = Console.ReadLine();



                            var books = context.Books.Include(c => c.Card).Where(b => b.Card.Id == user.Card.Id);
                            if (DataAccess.Decrypt(user.Card.Pin) != password)
                            {
                                DataAccess.Home("Invalid pin."); ;
                                break;
                            }
                            DataAccess.Home("");
                            Console.WriteLine($"{"User id:",-9} {user.Id}");
                            Console.WriteLine($"{"Surname:",-9} {user.FirstName}");
                            Console.WriteLine($"{"Lastname:",-9} {user.LastName}");
                            Console.WriteLine($"{"Password:",-9} {user.Card.Pin}");
                            Console.Write("Borrowed books: ");
                            foreach (var book in books)
                            {
                                Console.Write($"({book.Id}) {book.Title}, ");
                            }
                        }
                        break; //View user data

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