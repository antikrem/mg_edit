using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mg_edit.Movement;

namespace mg_edit.Loader
{
    // Full definition of an entity loaded from script
    class EntityDefinition : Loadable
    {
        // Movement object to control movement state
        public MovementSystem MovementSystem { get; }

        // List of instances of this definition
        private List<Entity> Instances { get; }

        public EntityDefinition(List<int> SpawningCycles)
        {
            MovementSystem = new MovementSystem();
            Instances = new List<Entity>();
            this.SpawningCycles = SpawningCycles;
        }

        // Returns all instances of this definition
        public List<Entity> GetEntities()
        {
            Instances.Clear();
            SpawningCycles.ForEach( i => Instances.Add(new Entity(MovementSystem, i)) );
            return Instances;
        }
    }
}
