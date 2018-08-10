using System;
using MyCardNukeSub.Readers;

namespace MyCardNukeSub
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello CardNuke !");
            Console.WriteLine("Read From EventStore for addNewCard");

            ReadFromEventStore reader = new ReadFromEventStore();


            try 
            {
                reader.ReadFromBeginning();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Problem occured in reader : {e.Message}");
            }

        }
    }
}
