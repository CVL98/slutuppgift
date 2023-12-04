using Helpers;
using slutuppgift.DATA;

namespace slutuppgift
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            DataAccess access = new DataAccess();

            access.CreateFiller();
            access.Clear();
            access.AddLoanCardToPerson(3);
            access.AddBookIdToPersonLoanCard(3,11);
            access.AddBookToDatabase("Stefan Bok",6,7);
            access.MarkBookAsNotLoaned(11);
        }
    }
}