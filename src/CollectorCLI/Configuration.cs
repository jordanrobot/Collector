using System;
using System.CodeDom;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Xml;

namespace iLogicCollector
{
    public class Config
    {
        public string Input { get; set; }
        public string Output { get; set; }
        public bool Force { get; set; } = false;
        public bool Debug { get; set; } = false;
        public bool Recursion { get; set; } = false;
        public static string Version { get; } = Assembly.GetEntryAssembly().GetName().Version.ToString();
        public string CurrentWorkingPath { get; } = Path.GetDirectoryName(Environment.CurrentDirectory);

        public Config(string[] args)
        {
            // Command line parsing
            var parameters = new ArgumentParser(args);

            if (parameters.IsEmpty())
            {
                
            }
            //get parameters
            //-input (optional, or use cwd), -output (oTODO
            //ptional, file name to use), -config (optional, configuration tag to recognize), -force/-f (optional, forces the overwrite of the target file.)

            // Look for specific arguments values and display them if they exist (return null if they don't)

            //Force
            try
            {
                if ((!parameters.IsEmpty()) || (parameters["f"] != null) || (parameters["force"] != null))
                {
                    this.Force = true;
                }
            }
            finally
            {
            }

            //Debug
            try
            {
                if ((!parameters.IsEmpty()) || (parameters["d"] != null) || (parameters["debug"] != null))
                {
                    this.Debug = true;
                }
            }
            finally
            {
            }

            //Recursion
            try
            {
                if ((!parameters.IsEmpty()) || (parameters["r"] != null) || (parameters["recursion"] != null))
                {
                    this.Recursion = true;
                }
            }
            finally
            {
            }


            //Input
            try
            {
                if ((!parameters.IsEmpty()) || (parameters["input"] != null) || (parameters["i"] != null))
                {
                    var tempInput = parameters["input"] ?? parameters["i"];
                    //this.Input = CalculateInputPath(tempInput);
                }
                else
                {
                    this.Input = Environment.CurrentDirectory;
                }
            }
            finally
            {
            }

            //Output

            try
            {
                if ((!parameters.IsEmpty()) || (parameters["output"] == null) && (parameters["o"] == null))
                {
                    //Todo: make the path/output recognize if the entered parameter is a whole path, or just a file name.
                    var temp = Path.GetDirectoryName(Environment.CurrentDirectory);
                    Output = temp + ".txt";
                }
                else
                {
                    var temp = parameters["output"] ?? parameters["o"];

                    this.Output = CalculateOutputPath(temp);
                }
            }
            finally
            {
            }

            try
            {
                //Help;
                if ((parameters["help"] != null) || (parameters["h"] != null) || (parameters["?"] != null))
                {
                    Console.WriteLine("iLogic Collector\n" +
                                      "version: \n" + Version +
                                      "\n" +
                                      "Command options:\n" +
                                      "-i, --input:     Directory whose content you want to process.\n" +
                                      "-o, --output:    The output file to write.\n" +
                                      "-r, --recursion: Recursively process input directory.\n" +
                                      "-f, --force:     Overwrite the output file if it already exists.\n" +
                                      "-h, --help:      Show this help menu.\n");
                }
            }

            //Todo: set the rest of these config options
            finally
            {
            }
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
                return input;
            }

            if (input.EndsWith("\"", StringComparison.CurrentCulture))
            {

                input = input.TrimEnd((char)34);

            } 
            // Check if directory is complete, but does not exists.  This should return an error.
            if (Path.IsPathRooted(input))
            {
                throw new SystemException("The input path does not exist.  Exiting.");
            }

            //Does the input end in a '\' character?  If not, add one...
            var tempPath = CurrentWorkingPath + Path.DirectorySeparatorChar + input;
            if (!input.EndsWith(@"\", StringComparison.CurrentCulture))
            {
                input = input;
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


        public string GetDefaultOutputFile()
        {
            //TODO: fix this bit of code... throwing aNullReferenceException at runtime.
            //Returns the current working path + last directory folder in the Input path + txt
            return this.Input.Split(Path.DirectorySeparatorChar).Last() + ".txt";
        }


        public string CalculateOutputPath(string output)
        {
            if (this.Debug) Console.WriteLine("Debug: Output = " + output);
            if (this.Debug) TestDirectoryList(output);


            // Is output string == null?
            if (string.IsNullOrEmpty(output))
            {
                return CurrentWorkingPath + Path.DirectorySeparatorChar + GetDefaultOutputFile();
            }

            if (!IsFilePathValid(output))
            {
                throw new SystemException("The output parameter is ill-formed.  Exiting.");
            }


                //is the directory well formed?
                    //Yes - continue
                    //No - exit w/ error


                    //is the directory relative or absolute?
                if (!IsPathAbsolute(output))
                {
                    output = this.CurrentWorkingPath + Path.DirectorySeparatorChar + output;
                }

                //does the directory end with a DirectorySeperatorChar?  add if it does not...
                if (!Path.HasExtension(output))
                {
                    if (!output.EndsWith(@"\", StringComparison.CurrentCulture))
                    {
                        output = output + @"\";
                    }
                }
                

                //does a file-name exist?
                if (!Path.HasExtension(output))
                {   //does not have extension
                    output = output + GetDefaultOutputFile();
                }

            //////OLD CODE!!!
            //If the directory exists...
            //if (Directory.Exists(Path.GetDirectoryName(output)))
            //{
              //  output = Path.GetDirectoryName(output);
            //}

            return output;

        }
        private bool IsFilePathValid(string path)
        {
            System.IO.FileInfo fi = null;

            //Catch invalid characters: \\ and \...\..
            if (Regex.IsMatch(path, @"^^(.+\\\\)|(\\\.+\\*$)|(\\\.+\\)"))
            {
                if (this.Debug) Console.WriteLine("Matched case 1 illegal characters...");
                return false;
            }
            //Catch invalid characters: system
            if (Regex.IsMatch(path, "[" + Regex.Escape(new string(System.IO.Path.GetInvalidPathChars())) + "]", RegexOptions.IgnoreCase))
            {
                if (this.Debug) Console.WriteLine("Matched case 2 illegal characters...");
                return false;
            }

            //try getting fileInfo to check if /directory/file path is properly defined...
            try 
            {
                fi = new System.IO.FileInfo(path);
            }
            catch (ArgumentException) { }
            catch (System.IO.PathTooLongException) { }
            catch (NotSupportedException) { }

            return !ReferenceEquals(fi, null);
        }

        private bool IsPathAbsolute(string path)
        {
            try
            {
                return (System.IO.Path.IsPathRooted(path)) ? true : false;
            }
            catch (System.ArgumentException) {Console.WriteLine(path + " Failed to parse (IsPathRooted).  Exiting.");
                return false;
            }

        }

        
        public void FileUnitTest(string test)
        {
            //TESTING TESTING
            Console.WriteLine("\n" + test);
            
            Console.WriteLine("IsFilePathValid = " + IsFilePathValid(test).ToString());
            if (IsFilePathValid(test))
            {
                Console.WriteLine("IsPathAbsolute = " + IsPathAbsolute(test).ToString());
                Console.WriteLine("Filepath Name = " + Path.GetFileName(test));
            }
        }

        public void TestDirectoryList(string output)
        {
            //TESTING TESTING
            FileUnitTest("Output: " + output + "\n");
            Console.WriteLine("===================");
            FileUnitTest(@"C:\Documents\testing.ini");
            FileUnitTest(@"C:\Documents\");
            FileUnitTest(@"C:\Documents");
            FileUnitTest(@"C:Documents\testing.ini");
            FileUnitTest(@"C:Documents\");
            FileUnitTest(@"C:Documents");
            FileUnitTest(@"\Documents\testing.ini");
            FileUnitTest(@"\Documents\");
            FileUnitTest(@"\Documents");
            FileUnitTest(@"Documents\testing.ini");
            FileUnitTest(@"Documents\");
            FileUnitTest(@"Documents");
            FileUnitTest(@"/Documents\testing.ini");
            FileUnitTest(@"/Documents\");
            FileUnitTest(@"/Documents");
            FileUnitTest(@" Documents\testing.ini");
            FileUnitTest(@" Documents\");
            FileUnitTest(@" Documents");
            FileUnitTest(@" /Documents\testing.ini");
            FileUnitTest(@" /Documents\");
            FileUnitTest(@" /Documents");
            FileUnitTest(@"?Documents\testing.ini");
            FileUnitTest(@"?Documents\");
            FileUnitTest(@"?Documents");
            FileUnitTest(@"\\Documents\testing.ini");
            FileUnitTest(@"\\Documents\");
            FileUnitTest(@"\\Documents");
            FileUnitTest(@"\Documents\\testing.ini");
            FileUnitTest(@"\Documents\\");
            FileUnitTest(@"\Documents");
            FileUnitTest(@"C:\Docum|ents\testing.ini");
            FileUnitTest(@"C:\Docum|ents\");
            FileUnitTest(@"C:\Docum|ents");
            FileUnitTest(@"C:\.\Documents\testing.ini");
            FileUnitTest(@"C:\.\Documents\");
            FileUnitTest(@"C:\.\Documents");
            FileUnitTest(@"C:\Documents\...testing.ini");
            FileUnitTest(@"C:\Documents\...");
            FileUnitTest(@"C:\Documents...");
            FileUnitTest(@"C:\Doc  uments\testing.ini");
            FileUnitTest(@"C:\Doc  uments\");
            FileUnitTest(@"C:\Doc  uments");
            FileUnitTest(@"C:\Documents\testing.ini  ");
            FileUnitTest(@"C:\Documents\  ");
            FileUnitTest(@"C:\Documents  ");

            //TESTING TESTING
        }
    }
}