using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using iLogicCollector.Utility;

namespace iLogicCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Command line parsing
             var parameters = new Arguments(args);
             var config = new Configuration();
            //get parameters
            //-path (optional, or use cwd), -output (optional, file name to use), -config (optional, configuration tag to recognize), -force/-f (optional, forces the overwrite of the target file.)

            // Look for specific arguments values and display them if they exist (return null if they don't)
            if ((parameters["path"] != null) || (parameters["p"] != null))
            { config.CollectPath = parameters["path"];}
            else
            {config.CollectPath = Environment.CurrentDirectory;}

            if ((parameters["output"] != null) || (parameters["o"] != null))
            { config.File = parameters["File"]; }
            else
            {
                var temp = Path.GetDirectoryName(Environment.CurrentDirectory);
                config.File = temp + ".iLogicVb";
            }

            if ((parameters["help"] != null) || (parameters["h"] != null))
            {
                //show the help menu;
            }

            if ((parameters["f"] != null) || (parameters["force"] != null))
            {
                config.Force = true;
            }

        }

        }
}
