﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mg_edit.Helper
{

    class INIParser : IDisposable
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

        // Adds a string value to a given section/key
        // Can create new sections if required
        private void AddEntry(string section, string key, string value)
        {
            this.AddSection(section);
            store[section][key] = value;
        }

        // Adds new entry from a properly formmated input line
        private void AddEntryFromLine(string section, string line) {
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
                    this.AddEntryFromLine(section, line);
                }
                    
            }

        }

        // Closes this parser and updates underlying file
        public void Dispose()
        {
            List<string> lines = new List<string>();

            // Add default section
            if (store.ContainsKey(""))
            {
                foreach (var entry in store[""])
                {
                    lines.Add(entry.Key + "=" + entry.Value);
                }
                lines.Add("");
            }

            // Go through non default sections
            foreach (var section in store.ToList().Where(i => i.Key != ""))
            {
                lines.Add("[" + section.Key + "]");
                foreach (var entry in section.Value)
                {
                    lines.Add(entry.Key + "=" + entry.Value);
                }
                lines.Add("");
            }

            File.WriteAllLines(filepath, lines);
        }

        // Checks if given section/key/value combination exists
        public bool Has(string section, string key)
        {
            return store.ContainsKey(section) && store[section].ContainsKey(key);
        }

        // Returns a string for a given section and key
        // Will return the default if key not found
        // Will also set value to the default
        public string Get(string section, string key, string backup)
        {
            if (this.Has(section, key)) {
                return store[section][key];
            }
            else
            {
                AddEntry(section, key, backup);
                return backup;
            }
        }

        // Returns a string for a given key, looks at default section
        // Will return the default if key not found
        // Will also set value to the default
        public string Get(string key, string backup)
        {
            if (this.Has("", key))
            {
                return store[""][key];
            }
            else
            {
                AddEntry("", key, backup);
                return backup;
            }
        }

        // Sets a value
        public void Set(string section, string key, string value)
        {
            AddEntry(section, key, value);
        }

        // Sets a value
        public void Set(string key, string value)
        {
            AddEntry("", key, value);
        }
    }
}
