using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication8
{
    class Program
    {
        static void Main(string[] args)
        {
            FetchURLS newFetcher = new FetchURLS();
            Thread oThread = new Thread(new ThreadStart(newFetcher.Beta));

            oThread.Start();
            while (!oThread.IsAlive) ;

            //oThread.Abort();
            Console.WriteLine();
            Console.WriteLine("URLs fetched");
            int num;
            Console.WriteLine("Enter a number: ");
            num = Convert.ToInt32(Console.ReadLine());
    
        }
    }
}
