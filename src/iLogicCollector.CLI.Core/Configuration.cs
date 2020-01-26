using System;
using System.IO;
using iLogicCollector.Utility;

namespace iLogicCollector
{
    public class Configuration
    {
        public string CollectPath;
        public string Output; //user entered output (file/path) name to save the collected file to
        public string FilePath; //formatted and corrected output filename
        public bool Force = false;

        public Configuration(string[] args)
        {
            // Command line parsing
            var parameters = new Arguments(args);

            //get parameters
            //-path (optional, or use cwd), -output (optional, file name to use), -config (optional, configuration tag to recognize), -force/-f (optional, forces the overwrite of the target file.)

            // Look for specific arguments values and display them if they exist (return null if they don't)
            if ((parameters["path"] != null) || (parameters["p"] != null))
            { CollectPath = parameters["path"]; }
            else
            { CollectPath = Environment.CurrentDirectory; }

            if ((parameters["output"] != null) || (parameters["o"] != null))
            { Output = parameters["output"]; }
            else
            {
                //Todo: make the path/output recognize if the entered parameter is a whole path, or just a file name.
                var temp = Path.GetDirectoryName(Environment.CurrentDirectory);
                Output = temp + ".iLogicVb";
            }

            if ((parameters["help"] != null) || (parameters["h"] != null))
            {
                //show the help menu;
            }

            if ((parameters["f"] != null) || (parameters["force"] != null))
            {
                Force = true;
            }
            //Todo: set the rest of these config options


        }
    }
}