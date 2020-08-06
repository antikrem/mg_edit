using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{
    class MovementPolarAccelerateTo : MovementQuanta
    {
        private readonly int duration;
        private readonly double endingSpeed;

        // Constructor for a MovementPolarTurn
        public MovementPolarAccelerateTo(double endingSpeed, int duration)
        {
            this.endingSpeed = endingSpeed;
            this.duration = duration;
        }

        // Check tick is within duration
        public override bool IsExecuting(int tick)
        {
            return tick <= duration;
        }

        // Update movement system parameters
        public override void UpdateExecution(int tick, MovementState movementState)
        {
            if (tick == 0)
            {
                double rate = (endingSpeed - movementState.Speed) / duration;
                movementState.SpeedChange = rate;
            }
            else if (tick  == duration)
            {
                movementState.SpeedChange = 0;
            }
        }

        public override string ComposeSaveDefinition()
        {
            return "->add_polar_accelerate_to(" + StartingTick.ToString()
                + ", " + duration.ToString()
                + ", " + endingSpeed.ToString()
                + ")";
        }
    }
}
