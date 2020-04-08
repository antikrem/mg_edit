using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mg_edit.Movement;

using System.Windows.Shapes;
using System.Windows.Media;

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

        // Circle radius
        private const int MARKER_RADIUS = 10;

        // Canvas element relating to circle of current 
        // Entity position
        readonly Ellipse marker = new Ellipse()
        {
            Width = 2 * MARKER_RADIUS,
            Height = 2 * MARKER_RADIUS,
            Stroke = Brushes.Red,
            StrokeThickness = 3
        };


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

        // Get starting movement state
        public MovementState GetStartingMovementState()
        {
            return movementSystem.GetStartingState();
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

        // Gets position at a given tick 
        // Will throw exception if bad tick
        public (double, double) GetPosition(int tick)
        {
            // Gets position, bit fuzzy on edges
            return positions[Math.Max(0, tick - spawningTick - 1)];
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

        // Gets reference to internal ellipse
        public Ellipse GetMarker()
        {
            return marker;
        }

        // Set marker position
        public void SetMarkerPosition((double, double) position)
        {
            marker.Margin = new System.Windows.Thickness(
                position.Item1 - MARKER_RADIUS,
                position.Item2 - MARKER_RADIUS,
                0,
                0
            );
        }
    }
}
