using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit
{
    // Parser for a level 
    class LoadParser
    {
        private const string TEMPLATE_FILE = "template_table.txt";
        private const string LOAD_TABLE_FILE = "load_table.txt";

        // Current folder being loaded from
        private string targetFolder = "";

        // Map of templates
        private Dictionary<string, string> templates = new Dictionary<string, string>();

        // Check if string is trivial
        static bool IsTrivialString(string line)
        {
            return
                // Check for empty string
                (line.Length == 0) ||
                // Check for starting comment
                (line.Length >= 2 && line[0] == '/' && line[1] == '/');
        }

        // Adds a template, ignores empty strings
        private void AddTemplate(string name, string contents)
        {
            if (name != "" && contents != "")
            {
                templates[name] = contents;

            }
        }

        // Load templates from file
        public void LoadTemplates()
        {
            string templateName = "";
            string templateContents = "";

            using (StreamReader file = new StreamReader(targetFolder + TEMPLATE_FILE))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    // Trim string, indenting is irrelevent
                    ln = ln.Trim();

                    // Check for comment
                    if (IsTrivialString(ln))
                    {
                        // Ignore trivial strings
                    }
                    // Check for declaraction
                    else if (ln[0] == '[' && ln[ln.Length - 1] == ']')
                    {
                        AddTemplate(templateName, templateContents);
                        templateName = ln.Substring(1, ln.Length - 2);
                        templateContents = "";
                    }
                    // Simply add contents
                    else
                    {
                        templateContents = templateContents + ln + '\n';
                    }
                }

            }

        }

        // Expands a template line
        private string ExpandTemplate(string line)
        {
            var vec = line.Split(' ');

            string body = this.templates[vec[0].Substring(1)];

            for (int i = 1; i < vec.Length; i++)
            {
                body = body.Replace("%" + i.ToString(), vec[i]);
            }

            return body;
        }

        // Loads level's load table as string, with all comments removed and templates expanded
        public string GetLevelLoadTable()
        {
            string loadTable = "";

            using (StreamReader file = new StreamReader(targetFolder + LOAD_TABLE_FILE))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    // Trim string, indenting is irrelevent
                    ln = ln.Trim();

                    // Ignore empty string
                    if (ln.Length == 0)
                    {
                        // Pass
                    }
                    // Ignore comment
                    else if (ln.Length >= 2 && ln[0] == '/' && ln[1] == '/')
                    {
                        // Pass
                    }
                    else if (ln[0] == '#')
                    {
                        loadTable = loadTable + this.ExpandTemplate(ln) + "\n";
                    }
                    else
                    {
                        loadTable = loadTable + ln + "\n";
                    }

                }

            }

            return loadTable;
        }

        // Loads entities 
        public void LoadEntities()
        {
            // Get level's load table
            var loadTable = GetLevelLoadTable().Split('\n');

            // Variables used in parsing
            List<int> cycles = new List<int>();
            // List of entities being updated
            List<Entity> ents = new List<Entity>();

            foreach (string line in loadTable)
            {
                var vec = line.Split('\n');

                // Look for starting cycle 
                if (line.StartsWith("@cycle"))
                {
                    cycles.Clear();
                    for (int i = 0; i < vec.Length; i++)
                    {
                        cycles.Add(Int32.Parse(vec[i]));
                    }
                }
                // Look for an ent declaration
                else if (line.StartsWith("ent"))
                {

                }
                // Sets flags on which component to update
                else if (line.StartsWith("+"))
                {

                }
                // Conduct update to component


            }
        }

        // Constructor sets the target folder
        private LoadParser(string targetFolder)
        {
            this.targetFolder = targetFolder;
        }

        // Returns true if the folder is valid
        public bool IsValidConstruction()
        {
            return File.Exists(targetFolder + LOAD_TABLE_FILE);
        }

        // Creates an instance of LoadParser
        // Takes targetFolder to search for load files
        // returns null on failure
        public static LoadParser CreateLevelLoader(string targetFolder)
        {
            LoadParser loader = new LoadParser(targetFolder);
            return loader.IsValidConstruction() ? loader : null;

        } 
    }
}
