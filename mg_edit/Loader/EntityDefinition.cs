using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mg_edit.Movement;
using mg_edit.TextEdit;

namespace mg_edit.Loader
{
    // Full definition of an entity loaded from script
    class EntityDefinition : Loadable
    {
        // Movement object to control movement state
        public MovementSystem MovementSystem { get; } = new MovementSystem();

        // List of instances of this definition
        public List<Entity> Instances { get; } = new List<Entity>();

        // List of templates
        private List<TemplateInstance> Templates { get; } = new List<TemplateInstance>();

        // Map of component names to components
        private Dictionary<string, Component> Components { set; get; } = new Dictionary<string, Component>();

        public EntityDefinition(List<int> SpawningCycles)
        {
            this.SpawningCycles = SpawningCycles;
        }

        // Add a template, if the template contains an existing component, remove it 
        public void AddTemplate(LoadParser parser, string templateName, List<string> parameters)
        {
            var template = new TemplateInstance(templateName, parameters);
            Templates.Add(template);

            string body = parser.Templates[templateName];

            Components = Components
                .Where(entry => !body.Contains(entry.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

        }

        // Returns all instances of this definition
        public List<Entity> GetEntities()
        {
            Instances.Clear();
            SpawningCycles.ForEach( i => Instances.Add(new Entity(MovementSystem, i)) );
            return Instances;
        }

        protected override ILoadablePanel GenerateLoadPanel()
        {
            return new EntityDefinitionLoadPanel(this);
        }

        // Adds component
        public void AddComponent(string componentName, Component component)
        {
            Components["+" + componentName] = component;
        }

        // Creates a script save for this
        public override string ConstructSaveDirective()
        {
            List<string> body = new List<string>();

            // Add all templates
            Templates.ForEach(x => body.Add(x.ComposeSaveDirective()));

            // Add extra components
            foreach(var x in Components.Values) {
                body.Add(x.ComposeSaveDirective(this));
            }

            return string.Join("\n", body.ToArray()) + "\n";
        }

        // Reload this entity from templates
        // Modyfying components is instant
        public void Reload(LoadParser level)
        {
            foreach (var template in Templates)
            {
                string[] body = template.GetSubbedTemplate(level).Split('\n'); ;

                // Current component 
                Component component = null;

                foreach (var line in body)
                {
                    if (line.StartsWith("+"))
                    {
                        string name = line.Substring(1).Split(' ')[0];
                        string[] parameters = line.Split(' ').Skip(1).ToArray();

                        component = Component.TranslateToComponentType(name);
                        if (component is Object)
                        {
                            component.Initialise(parameters, this);
                            this.AddComponent(name, component);
                        }
                    }

                    // Conduct update to component
                    else if (line.StartsWith("->") && (component is Object))
                    {
                        component.UpdateEntity(line, this);
                    }


                }

            }
        }
    }
}
