using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{
    class MovementPolarAccelerateTo : MovementQuanta
    {
        public int Duration { get; set; }
        public double EndingSpeed { get; set; }

        public MovementPolarAccelerateTo()
        {
            StartingTick = 0;
            this.EndingSpeed = 0;
            this.Duration = 0;
        }

        // Constructor for a MovementPolarTurn
        public MovementPolarAccelerateTo(double endingSpeed, int duration)
        {
            this.EndingSpeed = endingSpeed;
            this.Duration = duration;
        }

        // Check tick is within duration
        public override bool IsExecuting(int tick)
        {
            return tick <= Duration;
        }

        // Update movement system parameters
        public override void UpdateExecution(int tick, MovementState movementState)
        {
            if (tick == 0)
            {
                double rate = (EndingSpeed - movementState.Speed) / Duration;
                movementState.SpeedChange = rate;
            }
            else if (tick  == Duration)
            {
                movementState.SpeedChange = 0;
            }
        }

        public override string ComposeSaveDefinition()
        {
            return "->add_polar_accelerate_to(" + StartingTick.ToString()
                + ", " + Duration.ToString()
                + ", " + EndingSpeed.ToString()
                + ")";
        }
    }
}
