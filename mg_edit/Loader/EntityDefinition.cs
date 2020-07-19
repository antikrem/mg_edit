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
        public MovementSystem MovementSystem { get; }

        // List of instances of this definition
        public List<Entity> Instances { get; }

        // List of templates
        private List<TemplateInstance> Templates { get; }

        // Map of component names to components
        private Dictionary<string, ComponentCreator> Components { set; get; }

        public EntityDefinition(List<int> SpawningCycles)
        {
            MovementSystem = new MovementSystem();
            Instances = new List<Entity>();
            Templates = new List<TemplateInstance>();
            Components = new Dictionary<string, ComponentCreator>();

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
        public void AddComponent(string componentName, ComponentCreator component)
        {
            Components["+" + componentName] = component;
        }

        // Creates a script save for this
        public override string ConstructSaveDirective()
        {
            List<string> body = new List<string>();

            // Add all templates
            Templates.ForEach(x => body.Add(x.ToString()));
            
            return string.Join("\n", body.ToArray()) + "\n";
        }

        
    }
}
