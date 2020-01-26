using System;
using System.IO;
using System.Reflection;

namespace iLogicCollector
{
    public class Config
    {
        public string CollectPath { get; set; }
        public string Output { get; set; } //user entered output (file/path) name to save the collected file to
        public string FilePath { get; set; } //formatted and corrected output filename
        public bool Force { get; set; } = false;
        public bool Debug { get; set; } = false;
        public static string Version { get; }= Assembly.GetEntryAssembly().GetName().Version.ToString();


        public Config(string[] args)
        {
            // Command line parsing
            var parameters = new ArgumentParser(args);

            //get parameters
            //-path (optional, or use cwd), -output (optional, file name to use), -config (optional, configuration tag to recognize), -force/-f (optional, forces the overwrite of the target file.)

            // Look for specific arguments values and display them if they exist (return null if they don't)
            //Path
            if ((parameters["path"] != null) || (parameters["p"] != null))
            { CollectPath = parameters["path"]; }
            else
            { CollectPath = Environment.CurrentDirectory; }

            //Output
            if ((parameters["output"] != null) || (parameters["o"] != null))
            { Output = parameters["output"]; }
            else
            {
                //Todo: make the path/output recognize if the entered parameter is a whole path, or just a file name.
                var temp = Path.GetDirectoryName(Environment.CurrentDirectory);
                Output = temp + ".iLogicVb";
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
    }
}