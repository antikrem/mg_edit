using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{
    class MovementPolarTurn : MovementQuanta
    {
        public double Total;
        public int Duration;

        public MovementPolarTurn(int startingTick)
        {
            this.StartingTick = startingTick;
            this.Total = 0;
            this.Duration = 1;
        }

        // Constructor for a MovementPolarTurn
        public MovementPolarTurn(double rate, int duration)
        {
            this.Total = rate;
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
                movementState.AngleChange = this.Total/this.Duration;
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
                + ", " + Total.ToString()
                + ")";
        }
    }
}
