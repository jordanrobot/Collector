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
            Console.WriteLine("Hello World!");

 
             var config = new Configuration(args);

             Console.WriteLine("Path = " + config.CollectPath);
             Console.WriteLine("Output = " + config.Output);
             Console.WriteLine("Force = " + config.Force);

        }

        }
}
