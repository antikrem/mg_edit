﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using mg_edit.Components;

namespace mg_edit
{
    // Parser for a level 
    class LoadParser
    {
        public const string TEMPLATE_FILE = "template_table.txt";
        public const string LOAD_TABLE_FILE = "load_table.txt";

        // Ents loaded from level
        List<Entity> ents = new List<Entity>();

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
                        // Add last template and setup for next template
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

                // Need to add last template
                AddTemplate(templateName, templateContents);

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

            }

            return loadTable;
        }

        // Translates a component name to correct Component type
        static private ComponentCreator TranslateToComponentType(string line)
        {
            string name = line.Substring(1).Split(' ')[0];
            switch (name)
            {
                case "position":
                    return new ComponentPositionCreator();

                case "movement":
                    return new ComponentMovementCreator();

                case "staticMovement":
                    return new ComponentStaticMovementCreator();

                default:
                    return null;
            }
        }

        // Loads level 
        public void LoadLevel()
        {
            // Get level's load table
            var loadTable = GetLevelLoadTable().Split('\n');

            // Variables used in parsing
            List<int> cycles = new List<int>();

            // Ents being loaded and modified
            List<Entity> modifiedEnts = new List<Entity>();

            // Current component 
            ComponentCreator component = null;

            foreach (string line in loadTable)
            {
                var vec = line.Split(' ');

                // Look for starting cycle 
                if (line.StartsWith("@cycle"))
                {
                    cycles.Clear();
                    for (int i = 1; i < vec.Length; i++)
                    {
                        cycles.Add(Int32.Parse(vec[i]));
                    }
                }

                // Look for an ent declaration
                else if (line.StartsWith("ent"))
                {
                    // Push current ents and clear list
                    ents.AddRange(modifiedEnts);
                    modifiedEnts.Clear();
                    cycles.ForEach(cycle => modifiedEnts.Add(new Entity(cycle)));
                }

                // Sets flags on which component to update
                else if (line.StartsWith("+"))
                {
                    component = TranslateToComponentType(line);
                    if (component is Object)
                    {
                        string[] parameters = line.Split(' ').Skip(1).ToArray();
                        modifiedEnts.ForEach(ent => component.Initialise(parameters, ent));
                    }
                }
                
                // Conduct update to component
                else if (line.StartsWith("->"))
                {
                    if (component is Object)
                    {
                        modifiedEnts.ForEach(ent => component.UpdateEntity(line, ent));
                    }
                }

            }

            ents.AddRange(modifiedEnts);
        }

        // Return loaded entities
        public List<Entity> GetEntities()
        {
            return ents;
        }

        // Returns length of level
        public int GetLevelLength()
        {
            int levelLength = 0;

            this.ents.ForEach(ent => levelLength = Math.Max(levelLength, ent.GetSpawningTick() + ent.GetLifetime()));

            return levelLength;
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
