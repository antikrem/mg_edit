using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{

    // Base class for a movement command
    public abstract class MovementQuanta
    {
        public int StartingTick { set; get; }

        // Returns true if this MovementQuanta should still be executing
        public abstract bool IsExecuting(int tick);

        // Takes current position and movement system
        // Updates movement system with this tick of a MovementQuanta's derived functionality
        public abstract void UpdateExecution(int tick, MovementState movementState);

        // Returns a string representing this movement quanta for save
        abstract public string ComposeSaveDefinition();
    }
}
