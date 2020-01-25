using System;
using System.IO;
using iLogicCollector.Utility;

namespace iLogicCollector
{
    public class Configuration
    {
        public string CollectPath;
        public string File;
        public string FilePath;
        public string Target;
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
            { File = parameters["File"]; }
            else
            {
                var temp = Path.GetDirectoryName(Environment.CurrentDirectory);
                File = temp + ".iLogicVb";
            }

            if ((parameters["help"] != null) || (parameters["h"] != null))
            {
                //show the help menu;
            }

            if ((parameters["f"] != null) || (parameters["force"] != null))
            {
                Force = true;
            }

        }
    }
}