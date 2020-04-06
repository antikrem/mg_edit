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

        // Velocity for components of polar aspect
        private (double, double) polarChange = (0, 0);

        // Caps for velocity polar
        private (double, double) polarCap = (double.PositiveInfinity, double.PositiveInfinity);

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

        // Sets starting polar components change
        public void SetPolarSpeedChange(double mag)
        {
            this.polarChange = (mag, this.polarChange.Item2);
        }

        // Sets starting polar components change
        public void SetPolarAngleChange(double ang)
        {
            this.polarChange = (this.polarChange.Item1, ang);
        }

        // Sets starting polar components cap
        public void SetPolarSpeedCap(double mag)
        {
            this.polarCap = (mag, this.polarCap.Item2);
        }

        // Sets starting polar components cap
        public void SetPolarAngleCap(double ang)
        {
            this.polarCap = (this.polarCap.Item1, ang);
        }

        // Computes all positions and returns
        public List<(double, double)> GetPositions() {
            List<(double, double)> positions = new List<(double, double)>();

            // Velocity as cartesian
            var (velX, velY) = velocityCartesian;

            // Acceleration as cartesian
            var (AcelX, AcelY) = accelerationCartesian;

            // Speed as polar
            var (mag, ang) = velocityPolar;

            // Speed change as polar
            var (magChange, angChange) = polarChange;

            // Cap on polar speed change
            var (magCap, angCap) = polarCap;

            // Running x,y position
            var (x, y) = startingPosition;
            positions.Add(startingPosition);

            // Iterate while in gamepace and below tick max
            for (int tick = 0;
                (tick < MAXIMUM_POSITION_COUNT) && GameState.IsInGameSpace(x, y);
                tick++)
            {
                // update cartesian velocity
                velX += AcelX;
                velY += AcelY;

                // update polar velocity
                mag = MathHelper.Clamp(mag + magChange, -magCap, magCap);
                ang += angChange;

                x = x
                    + velocityCartesian.Item1
                    + MathHelper.ToCartesian(mag, ang).Item1;

                y = y
                    + velocityCartesian.Item2
                    + MathHelper.ToCartesian(mag, ang).Item2;

                positions.Add((x, y));
            }

            return positions;
        }
    }
}
