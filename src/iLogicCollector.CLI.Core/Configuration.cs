using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace iLogicCollector
{
    public class Config
    {
        public string Input { get; set; }
        public string Output { get; set; } //user entered output (file/path) name to save the collected file to
        public string FilePath { get; set; } //formatted and corrected output filename
        public bool Force { get; set; } = false;
        public bool Debug { get; set; } = false;
        public static string Version { get; }= Assembly.GetEntryAssembly().GetName().Version.ToString();
        public string CurrentWorkingPath { get; } = Path.GetDirectoryName(Environment.CurrentDirectory);

        public Config(string[] args)
        {
            // Command line parsing
            var parameters = new ArgumentParser(args);

            //get parameters
            //-path (optional, or use cwd), -output (optional, file name to use), -config (optional, configuration tag to recognize), -force/-f (optional, forces the overwrite of the target file.)

            // Look for specific arguments values and display them if they exist (return null if they don't)
            //Input
            if ((parameters["input"] != null) || (parameters["i"] != null))
            {
                var tempInput = parameters["input"] ?? parameters["i"];
                this.Input = CalculateInputPath(tempInput);
            }
            else
            {
                this.Input = Environment.CurrentDirectory;
            }


            //Output
            if ((parameters["output"] == null) && (parameters["o"] == null))
            {
                //Todo: make the path/output recognize if the entered parameter is a whole path, or just a file name.
                var temp = Path.GetDirectoryName(Environment.CurrentDirectory);
                Output = temp + ".iLogicVb";
            }
            else
            {
                Output = parameters["output"] ?? parameters["o"];
            }

            //Help;
            if ((parameters["help"] != null) || (parameters["h"] != null) || (parameters["?"] != null))
            {
                Console.WriteLine("iLogic Collector\n" +
                                  "version: \n" + Version +
                                  "\n" +
                                  "Command options:\n" +
                                  "-p, --path:   Directory whose content you want to process.\n" +
                                  "-o, --output: The output file to write.\n" +
                                  "-f, --force:  Overwrite the output file if it already exists.\n" +
                                  "-h, --help:   Show this help menu.\n");
            }

            //Force
            if ((parameters["f"] != null) || (parameters["force"] != null))
            {
                Force = true;
            }

            //Debug
            if ((parameters["d"] != null) || (parameters["debug"] != null))
            {
                Debug = true;
            }


            //Todo: set the rest of these config options


        }

        public string CalculateInputPath(string input)
        {
 
            // Is directory null?
            if (string.IsNullOrEmpty(input))
            {
                if (this.Debug == true) { Console.WriteLine("Input parameter directory is null or empty."); }
                return CurrentWorkingPath + Path.DirectorySeparatorChar;
            }

            // Does directory exist?
            if (Directory.Exists(input))
            {
                //Console.WriteLine("Input parameter directory exists...");
                return input;
            }

            // Check if directory is complete, but does not exists.  This should return an error.
            if (Path.IsPathRooted(input))
            {
                //Console.WriteLine("Input parameter directory does not exist, we're going to exit roughly.");
                throw new SystemException("The input path does not exist.  Exiting.");
            }

            //Does the input end in a '\' character?  If not, add one...
            var tempPath = CurrentWorkingPath + Path.DirectorySeparatorChar + input;
            if (input.EndsWith(@"\", StringComparison.CurrentCulture))
            {
                //Console.WriteLine(@"Input parameter - assume it's relative to cwd.  Need to add a \ character");
                input = input + @"\";
            }

            // Check if C:/Current/working/directory/inputpath/ exists...
            if (Directory.Exists(tempPath))
            {
                if (this.Debug == true) { Console.WriteLine("Input parameter directory, assume relative... it's found!"); }
                return tempPath;
            }

            if (this.Debug == true) { Console.WriteLine("Input parameter directory could not be determined.  Exit."); }
            return null;
        }

        public string CalculateOutputPath(string output)
        {
            return null;
        }
    }
}