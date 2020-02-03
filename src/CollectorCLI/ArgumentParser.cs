/*
* ArgumentParser class: application arguments interpreter
*
* Authors:		R. LOPES
* Contributors:	R. LOPES
* Created:		25 October 2002
* Modified:		28 October 2002
* From:			https://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
*
* Version:		1.0
*/

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;

namespace iLogicCollector{
	/// <summary>
	/// ArgumentParser class
	/// </summary>
	public class ArgumentParser{
		// Variables
		private  StringDictionary Parameters;


		// Constructor
		public ArgumentParser(string[] args)
        {
                if ((args != null && args.Length != 0))
                {
                    return;
                }

            Parameters = new StringDictionary();
			var splitter = new Regex (@"^-{1,2}|^/|=|:",RegexOptions.IgnoreCase|RegexOptions.Compiled);
			//removes extraneous " quotes left behind in case the users use a \ character (which would escape the quote and leave it in the string"
			var remover= new Regex(@"^['""]?(.*?)['""]?$",RegexOptions.IgnoreCase|RegexOptions.Compiled);
			string parameter = null;

            // Valid parameters forms:
			// {-,/,--}param{ ,=,:}((",')value(",'))
			// Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'

            foreach(var txt in args)
            {
                // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
                var parts = splitter.Split(txt,3);

                switch(parts.Length)
                {
					// Found a value (for the last parameter found (space separator))
					case 1:
                    {
                        if(parameter!=null)
                        {
                            if(!Parameters.ContainsKey(parameter))
                            {
                                parts[0]=remover.Replace(parts[0],"$1");
                                Parameters.Add(parameter, parts[0]);
                            }
                            parameter=null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;
                    }

                    // Found just a parameter
					case 2:
						// The last parameter is still waiting. With no value, set it to true.
						if(parameter!=null && !Parameters.ContainsKey(parameter))
                        {
                            Parameters.Add(parameter, "true");
                        }
						parameter=parts[1].ToLower();
						break;

					// Parameter with enclosed value
					case 3:
						// The last parameter is still waiting. With no value, set it to true.
						if(parameter!=null)
                        {
							if(!Parameters.ContainsKey(parameter)) Parameters.Add(parameter, "true");
						}
						parameter=parts[1].ToLower();
						// Remove possible enclosing characters (",')
						if(!Parameters.ContainsKey(parameter))
                        {
							parts[2]=remover.Replace(parts[2], "$1");
							Parameters.Add(parameter, parts[2]);
						}
						parameter=null;
						break;
                } //switch
            } //foreach

			// In case a parameter is still waiting
			if(parameter!=null)
            {
				if(!Parameters.ContainsKey(parameter.ToLower())) Parameters.Add(parameter.ToLower(), "true");
            }

			foreach (System.Collections.DictionaryEntry key in Parameters)
            {
                Console.WriteLine(key.Key);
                Console.WriteLine(key.Value);
            }
        } //ArgumentParser

        public bool IsEmpty()
        {
            try
            {
                return (Parameters == null);
            }
            catch (System.NullReferenceException)
            {
                return false;
            }

        }

		// Retrieve a parameter value (if it exists) through a lambda expression
		public string this [string param] => (Parameters[param]);
    }
}
