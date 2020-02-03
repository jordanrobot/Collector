using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace iLogicCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config(args);

            if (config.Debug == true)
            {

                Console.Write("\n-------------\n Debugging Dump:\n");
                Console.WriteLine("Input =      " + config.Input);
                Console.WriteLine("Output =     " + config.Output);
                Console.WriteLine("Force =      " + config.Force);
                Console.WriteLine("Debug =      "+ config.Debug);
                Console.WriteLine("Recursion =  "+ config.Recursion);
                Console.WriteLine("CWD =        "+ config.CurrentWorkingPath);
            }

            return;
        }

        }
}
