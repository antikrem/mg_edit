using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Helper
{
    

    class INIParser
    {
        const char INI_DELIMITER = '=';
        const char COMMENT_DELIMITER = ';';
        const char SECTION_NAME_START_DELIMITER = '[';
        const char SECTION_NAME_END_DELIMITER = ']';

        // Map of section to a map of key to values
        private readonly Dictionary<string, Dictionary<string, string>> store = new Dictionary<string, Dictionary<string, string>>();

        // Filepath of loaded ini file
        readonly private string filepath;

        // Adds a new section if it does not exist
        // If it does, silently has no effect
        private void AddSection(string section)
        {
            if (!store.ContainsKey(section))
            {
                store.Add(section, new Dictionary<string, string>());
            }
        }

        // Adds new entry from a properly formmated input line
        private void AddEntry(string section, string line) {
            // Add a new section
            this.AddSection(section);

            //otherwise parse line
            int offset;
		    if ((offset = line.IndexOf(INI_DELIMITER)) > 0) {

			    store[section][line.Substring(0, offset).Trim()] 
				    = line.Substring(offset + 1, line.Length - offset - 1);
		    }
        }

        public INIParser(string filepath)
        {
            this.filepath = filepath;

            string section = "";

            // Trim lines and select non trivial strings
            var lines = File.ReadLines(filepath).Select(i => i.Trim()).ToList().Where(i => i.Length > 0 && i[0] != COMMENT_DELIMITER);

            foreach (var line in lines)
            {
                if (line[0] == SECTION_NAME_START_DELIMITER && line[line.Length - 1] == SECTION_NAME_END_DELIMITER)
                {
                    section = line.Substring(1, line.Length - 2);
                }
                else
                {
                    this.AddEntry(section, line);
                }
                    
            }

        }
    }
}
