using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postman_runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Collection Name");
            string inputCollection = Console.ReadLine();

            Locate_collection location = new Locate_collection();
              location.EnterCmd(inputCollection);

            //   Locate_collection location = new Locate_collection();
            //    Process.Start(location.FindCollection("Runner.cmd"));
        }
    }
}
