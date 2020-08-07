using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{
    class MovementPolarTurn : MovementQuanta
    {
        public double Rate;
        public int Duration;

        // Constructor for a MovementPolarTurn
        public MovementPolarTurn(double rate, int duration)
        {
            this.Rate = rate;
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
                movementState.AngleChange = Rate;
            }
            else if (tick == Duration)
            {
                movementState.AngleChange = 0;
            }
        }

        public override string ComposeSaveDefinition()
        {
            return "->add_polar_turn(" + StartingTick.ToString() 
                + ", " + Duration.ToString()
                + ", " + (Rate * Duration).ToString()
                + ")";
        }
    }
}
