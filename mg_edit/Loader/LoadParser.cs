using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mg_edit.Loader
{
    // Parser for a level 
    class LoadParser
    {
        public const string TEMPLATE_FILE = "template_table.txt";
        public const string LOAD_TABLE_FILE = "load_table.txt";

        public const string PARAMETER_TOKEN = "PARAMETERS";

        // All loadables from the file
        List<Loadable> loadables = new List<Loadable>();

        // Entity instances created from loadables
        readonly List<Entity> entities = new List<Entity>();

        // Current folder being loaded from
        private string targetFolder = "";

        // Body of level
        public string LoadTableBody { get; set; }

        // Map of templates
        public readonly Dictionary<string, string> Templates = new Dictionary<string, string>();

        // Map of template PARAMETERS 
        public readonly Dictionary<string, List<string>> TemplateParameters = new Dictionary<string, List<string>>();

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
        private void AddTemplate(string name, string contents, string lastLine)
        {
            if (name != "" && contents != "")
            {
                // Check lastline for proper start
                if (lastLine.Length > 2 
                    && lastLine[0] == '/' && lastLine[1] == '/'
                    && lastLine.Substring(2).Trim().StartsWith(PARAMETER_TOKEN))
                {
                    TemplateParameters[name] = new List<string>(lastLine.Substring(2).Trim().Split(' '));
                    TemplateParameters[name].RemoveAt(0);

                }

                Templates[name] = contents;
            }
        }

        // Load templates from file
        public void LoadTemplates()
        {
            string templateName = "";
            string templateContents = "";

            using (StreamReader file = new StreamReader(targetFolder + TEMPLATE_FILE))
            {
                string lastln = "";
                string parameterln = "";
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
                        // Add last template and setup for next template
                        AddTemplate(templateName, templateContents, parameterln);
                        parameterln = lastln;
                        templateName = ln.Substring(1, ln.Length - 2);
                        templateContents = "";
                    }
                    // Simply add contents
                    else
                    {
                        templateContents = templateContents + ln + '\n';
                    }
                    lastln = ln;
                }

                // Need to add last template
                AddTemplate(templateName, templateContents, parameterln);

            }

        }

        // Expands a template line
        private string ExpandTemplate(string line)
        {
            var vec = line.Split(' ');

            string body = this.Templates[vec[0].Substring(1)];

            for (int i = 1; i < vec.Length; i++)
            {
                body = body.Replace("%" + i.ToString(), vec[i]);
            }
            body = body + line + "\n";

            return body;
        }

        // Loads level's load table as string, with all comments removed and templates expanded
        public string GetExpandedLevelLoadTable()
        {
            string loadTable = "";
            var lines = LoadTableBody.Split('\n').ToList();

            // Trim lines, indenting is irrevelent
            lines = lines.Select(line => line.Trim().Replace("\r", "")).ToList();

            foreach (string ln in lines)
            {
                // Ignore empty string
                if (ln.Length == 0)
                {
                    // Pass
                }
                // Ignore comment
                else if (ln.StartsWith("//"))
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

            return loadTable;
        }

        // Load level's load table body to memory
        public void LoadLevelLoadTableFromFile()
        {
            LoadTableBody = File.ReadAllText(targetFolder + LOAD_TABLE_FILE);
        }

        // Loads level 
        public void LoadLevel()
        {
            // Reset loadables
            loadables.Clear();

            // Get level's load table
            var loadTable = GetExpandedLevelLoadTable().Split('\n');

            // Variables used in parsing
            List<int> cycles = new List<int>();
            
            // Current Entity definition being worked on
            Loadable loadable = null;

            // Current component 
            ComponentCreator component = null;

            foreach (string line in loadTable)
            {
                var vec = line.Split(' ');

                // Look for starting cycle 
                if (line.StartsWith("@cycle") || line.StartsWith("@immediate"))
                {
                    // Clear old loadable
                    loadable = null;
                    cycles.Clear();
                    for (int i = 1; i < vec.Length; i++)
                    {
                        cycles.Add(Int32.Parse(vec[i]));
                    }
                }

                // Look for an ent declaration
                else if (line.StartsWith("ent"))
                {
                    // Generate new entities
                    loadable = new EntityDefinition(cycles);
                    loadables.Add(loadable);

                }

                // Sets flags on which component to update
                else if (line.StartsWith("+"))
                {
                    string name = line.Substring(1).Split(' ')[0];
                    string[] parameters = line.Split(' ').Skip(1).ToArray();

                    component = ComponentCreator.TranslateToComponentType(name);
                    if (component is Object)
                    {
                        component.Initialise(parameters, (EntityDefinition)loadable);
                        ((EntityDefinition)loadable).AddComponent(name, component);
                    }
                }
                
                // Conduct update to component
                else if (line.StartsWith("->"))
                {
                    if (component is Object)
                    {
                        component.UpdateEntity(line, (EntityDefinition)loadable);
                    }
                }

                // Update script
                else if (line.StartsWith("<<"))
                {
                    // Either extend or generate new script
                    if (!(loadable is Script))
                    {
                        loadable = new Script(cycles);
                        loadables.Add(loadable);
                    }
                    ((Script)loadable).Extend(line.Substring(2));
                }

                // Add a template
                else if (line.StartsWith("#"))
                {
                    // Look up template
                    string templateName = line.Substring(1).Split(' ')[0];
                    List<string> parameters = line.Substring(1).Split(' ').Skip(1).ToList();

                    if (Templates.ContainsKey(templateName))
                    {
                        ((EntityDefinition)loadable).AddTemplate(this, templateName, parameters);
                    }
                    
                }
            }
        }

        // Create a cycle directive for this 
        private string ConstructCycleDirective(Loadable loadable)
        {
            if (loadable.SpawningCycles.Count > 0)
            {
                string line = "@cycle ";
                loadable.SpawningCycles.ForEach(x => line = line + x.ToString() + " ");
                return line + "\n";
            }
            else
            {
                return "@immediate\n";
            }
        }

        // Save level to file
        public void SaveLevel()
        {
            string levelBody = "";
            
            foreach (var loadable in loadables)
            {
                // Add cycle for loadable
                levelBody = levelBody + ConstructCycleDirective(loadable);

                // Save each loadable
                levelBody = levelBody + loadable.ConstructSaveDirective();

                // New line between each loadable
                levelBody = levelBody + "\n";
            }
            levelBody.ToString();

            File.WriteAllText(targetFolder + "save" + LOAD_TABLE_FILE, levelBody);
            //File.WriteAllText(targetFolder + LOAD_TABLE_FILE, levelBody);
        }

        // Evaluates from existing definitions into list of entities
        public void EvaluateEntities()
        {
            entities.Clear();
            foreach (Loadable loadable in loadables)
            {
                if (loadable is EntityDefinition)
                {
                    entities.AddRange(((EntityDefinition)loadable).GetEntities());
                }
            }
        }

        // Return loaded entities
        public List<Entity> GetEntities()
        {
            return entities;
        }

        // Returns all loadables
        public List<Loadable> GetLoadables()
        {
            return loadables;
        }

        // Returns length of level
        public int GetLevelLength()
        {
            int levelLength = 0;
            this.entities.ForEach(ent => levelLength = Math.Max(levelLength, ent.GetSpawningTick() + ent.GetLifetime()));
            return levelLength;
        }

        // Constructor sets the target folder
        private LoadParser(string targetFolder)
        {
            this.targetFolder = targetFolder;
        }

        // Creates an instance of LoadParser
        // Takes targetFolder to search for load files
        // returns null on failure
        public static LoadParser CreateLevelLoaderFromFile(string targetFolder)
        {
            LoadParser loader = new LoadParser(targetFolder);
            return File.Exists(targetFolder + LOAD_TABLE_FILE) ? loader : null;

        } 
    }
}
