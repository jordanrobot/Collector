using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Collector
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Configuration(args);

            if (config.Debug == true)
            {
                ShowDebugOutput(config);
            }

            return;
        }

        private static void ShowDebugOutput(Configuration config)
        {
            Console.Write("\n-------------\n Debugging Dump:\n");
            Console.WriteLine("Input    = " + config.Input);
            Console.WriteLine("Output   = " + config.Output);
            Console.WriteLine("Force    = " + config.Force);
            Console.WriteLine("Debug    = " + config.Debug);
            Console.WriteLine("Recursion= " + config.Recursion);
            Console.WriteLine("CWD      = " + config.CurrentWorkingPath);
        }
    }
}
