using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mg_edit.Movement;

using System.Windows.Shapes;

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

        // List of lines drawn into center canvas
        private List<Line> drawnLines = new List<Line>();

        // Constructs with known starting tick
        public Entity(int spawningTick)
        {
            this.spawningTick = spawningTick;
        }

        // Gets internal movement system
        public MovementSystem GetMovementSystem()
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

        // Drawing related functions

        // Adds line to this entities line list
        public void AddLine(Line line)
        {
            this.drawnLines.Add(line);
        }

        // Returns reference to all lines
        public List<Line> GetLines()
        {
            return this.drawnLines;
        }

        // Clears entity of drawn lines
        public void ClearLines()
        {
            drawnLines.Clear();
        }
    }
}
