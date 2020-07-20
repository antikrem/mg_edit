using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{
    class MovementPolarTurn : MovementQuanta
    {
        private readonly double rate;
        private readonly int duration;

        // Constructor for a MovementPolarTurn
        public MovementPolarTurn(double rate, int duration)
        {
            this.rate = rate;
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
                movementState.AngleChange = rate;
            }
            else if (tick == duration)
            {
                movementState.AngleChange = 0;
            }
        }

        public override string ComposeSaveDefinition(int cycle)
        {
            return "->add_polar_turn(" + cycle.ToString() 
                + ", " + duration.ToString()
                + ", " + (rate * duration).ToString()
                + ")";
        }
    }
}
