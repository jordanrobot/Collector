using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace iLogicCollector.CLI.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //get parameters
            //-path (optional, or use cwd), -file (optional, file name to use), -config (optional, configuration tag to recognize)


            if (args == null)
            {
                Console.WriteLine("args is null");
            }


            ArgumentParser parser = new ArgumentParser(args);

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
                    //do something
                }
            }
        }

    }

    public class ArgumentParser
    {
        public Dictionary<string, string> ParameterList { get; set; } = new Dictionary<string, string>(;

        public void ArgumentParser(string[] keys]) 
        {
            foreach (string i in keys)
            {
                ParameterList.add(i, "");
            }
        }
        public void Parse(string[] args)
        {
            while (args.Any())
            {
                if (ParameterList.ContainsKey(args[0]) && !ParameterList.ContainsKey(args[1]))
                {
                    ParameterList[args[0]] = args[1];
                    args = args.Skip(2).ToArray();
                }
                else
                {
                    InvalidDataException err;
                    throw new System.ArgumentException("Input arguments were not recognized.");
                }
            }
        }
}

    public class ConfigurationPojo
    {
        public string CollectPath;
        public string File;
        public string FilePath;
        public string Target;

    }
}
