using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{
    // Encapsulates updates in the 
    class MovementSystem
    {
        // A hard cap on number of positions, to avoid infinite loops
        private const int MAXIMUM_POSITION_COUNT = 100000;

        // Variables related to entity movement

        // Starting position
        private (double, double) startingPosition = (0, 0);

        // Starting velocity cartesian
        private (double, double) velocityCartesian = (0, 0);

        // Starting acceleration cartesian
        private (double, double) accelerationCartesian = (0, 0);

        // Starting velocity polar
        private (double, double) velocityPolar = (0, 0);

        // Sets starting position
        public void SetPosition(double x, double y)
        {
            startingPosition = (x, y);
        }

        // Sets cartesian velocity
        public void SetVelocity(double x, double y)
        {
            velocityCartesian = (x, y);
        }

        // Sets starting polar components
        public void SetPolarSpeed(double mag)
        {
            this.velocityPolar = (mag, this.velocityPolar.Item2);
        }

        // Sets starting polar components
        public void SetPolarAngle(double ang)
        {
            this.velocityPolar = (this.velocityPolar.Item1, ang);
        }



        // Computes all positions and returns
        public List<(double, double)> GetPositions() {
            List<(double, double)> positions = new List<(double, double)>();

            // Velocity as cartesian
            var (velX, velY) = velocityCartesian;

            // Acceleration as cartesian
            var (AcelX, AcelY) = accelerationCartesian;

            // Velocity as polar
            var (mag, ang) = velocityPolar;

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

            return positions;
        }
    }
}
