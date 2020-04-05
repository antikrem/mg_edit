using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mg_edit.Movement;

namespace mg_edit
{
    // Represents an enemy entity in the game
    class Entity
    {
        // Spawning tick
        private int spawningTick = 0;

        // Movement object to control movement state
        MovementSystem movementSystem = new MovementSystem();

        // List of points specifying position
        private List<(double, double)> positions = new List<(double, double)>();

        // Constructs with known starting tick
        public Entity(int spawningTick)
        {
            this.spawningTick = spawningTick;
        }

        // Gets internal movement system
        public MovementSystem getMovementSystem()
        {
            return movementSystem;
        }

        // Computes all positions this entity takes
        public List<(double, double)> UpdatePositions()
        {
            positions = movementSystem.GetPositions();
            return positions;
        }

        // Gets all positions for entity
        public List<(double, double)> GetPositions()
        {
            return positions;
        }

        // Set spawning tick
        public void SetSpawningTick(int spawningTick)
        {
            this.spawningTick = spawningTick;
        }

        // Get spawning tick
        public int GetSpawningTick()
        {
            return this.spawningTick;
        }

        // Returns how many ticks this entity is alive for
        public int GetLifetime()
        {
            return this.positions.Count;
        }
    }
}
