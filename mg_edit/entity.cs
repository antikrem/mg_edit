using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit
{
    // Represents an enemy entity in the game
    class Entity
    {
        // A hard cap on number of positions, to avoid infinite loops
        private const int MAXIMUM_POSITION_COUNT = 100000;

        // Spawning tick
        private int spawningTick = 0;

        // Starting position
        private (double, double) startingPosition = (0,0);

        // List of points specifying position
        private List<(double, double)> positions = new List<(double, double)>();

        // Starting velocity
        private (double, double) startingVelocity = (0, 0);
        // starting speed and angle
        private double speed;
        private double angle;

        // Constructs with known starting tick
        public Entity(int spawningTick)
        {
            this.spawningTick = spawningTick;
        }

        // Sets starting position
        public void SetPosition(double x, double y)
        {
            startingPosition = (x, y);
        }

        // Sets starting velocity
        public void SetVelocity(double x, double y)
        {
            this.startingVelocity = (x, y);
        }

        // Sets starting polar components
        public void SetPolarSpeed(double mag)
        {
            this.speed = mag;
        }

        // Sets starting polar components
        public void SetPolarAngle(double ang)
        {
            this.angle = ang;
        }

        // Computes all positions this entity takes
        public void UpdatePositions()
        {
            // Velocity as cartesian
            var (velX, velY) = startingVelocity;

            // Acceleration as cartesian
            var (AcelX, AcelY) = startingVelocity;

            // Velocity as polar
            var (mag, ang) = (speed, angle);

            // Running x,y position
            var (x, y) = startingPosition;
            positions.Add(startingPosition);

            // Iterate while in gamepace and below tick max
            for (int tick = 0; 
                (tick < MAXIMUM_POSITION_COUNT) && GameState.IsInGameSpace(x, y);
                tick++)
            {
                x = x + velX;
                y = y + velY;

                positions.Add((x, y));
            }
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
