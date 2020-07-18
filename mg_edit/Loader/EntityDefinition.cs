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

        public EntityDefinition(List<int> SpawningCycles)
        {
            MovementSystem = new MovementSystem();
            Instances = new List<Entity>();
            Templates = new List<TemplateInstance>();

            this.SpawningCycles = SpawningCycles;
        }

        // Add a template
        public void AddTemplate(string templateName, List<string> parameters)
        {
            Templates.Add(new TemplateInstance(templateName, parameters));
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
    }
}
