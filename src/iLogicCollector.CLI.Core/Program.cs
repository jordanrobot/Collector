using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace iLogicCollector.CLI.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //get parameters
            //-path (optional, or use cwd), -file (optional, file name to use)

            String directory = "stuff"; //set directory based on parameter match
            String filename = "stuff"; //set filename
            String filePath = directory + "/" + filename + ".iLogicVB";
            var buffer = new List<String>();

            //Get list of files in directory
            string[] fileList = System.IO.Directory.GetFiles(directory);

            // ignore binary files (only look into text files?)
            foreach (var i in fileList) 
            {
                //get the file lines and read the first few to test.
                //search through files, look for matches to the tags.
                //add to the buff if found.
                if (Regex.IsMatch(i, "(?i)</iLogicCollectorHeader>"))
                {
                    ''
                }
            }
        }
    }
}
