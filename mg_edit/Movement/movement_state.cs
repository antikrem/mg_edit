using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{
    // Represents the state of mvoement
    // Which is all parameters of movement at a given position
    // No invariants
    public class MovementState
    {
        // Position
        public (double, double) Position
        {
            get;
            set;
        }
        = (0, 0);

        // Cartesian velocity
        public (double, double) CartesianVelocity
        {
            get;
            set;
        }
        = (0, 0);

        // Cartesian acceleration
        public (double, double) CartesianAcceleration
        {
            get;
            set;
        }
        = (0, 0);

        // Polar first order change 
        public double Speed { get; set; } = 0;
        public double Angle { get; set; } = 0;

        // Polar second order change 
        public double SpeedChange { get; set; } = 0;
        public double AngleChange { get; set; } = 0;

        // Polar second order cap 
        public double SpeedCap { get; set; } = double.PositiveInfinity;
        public double AngleCap { get; set; } = double.PositiveInfinity;

        // Conducts a single step of movement state update, given current parameters
        public void UpdateState()
        {
            // Update cartesian velocity
            this.CartesianVelocity = (
                CartesianAcceleration.Item1 + CartesianVelocity.Item1,
                CartesianAcceleration.Item2 + CartesianVelocity.Item2
            );

            // Update polar velocity
            Speed = MathHelper.Clamp(Speed + SpeedChange, -SpeedCap, SpeedCap);
            Angle = MathHelper.Clamp(Angle + AngleChange, -AngleCap, AngleCap);

            // Update position
            Position = (
                Position.Item1 + CartesianVelocity.Item1 + MathHelper.ToCartesian(Speed, Angle).Item1,
                Position.Item2 + CartesianVelocity.Item2 + MathHelper.ToCartesian(Speed, Angle).Item2
            );
        }

        // Default constructor
        public MovementState()
        {

        }

        // Copy constructor
        // Provides deep clone
        public MovementState(MovementState old)
        {
            Position = old.Position;

            CartesianVelocity = old.CartesianVelocity;
            CartesianAcceleration = old.CartesianAcceleration;

            Speed = old.Speed;
            Angle = old.Angle;

            SpeedChange = old.SpeedChange;
            AngleChange = old.AngleChange;

            SpeedCap = old.SpeedCap;
            AngleCap = old.AngleCap;

        }
    }
}
