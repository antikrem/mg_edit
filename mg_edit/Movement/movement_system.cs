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

        // Starting state of player
        private MovementState startingState = new MovementState();

        // Multimap storing all static movement commands
        private MultiMap<int, MovementQuanta> movementCommands = new MultiMap<int, MovementQuanta>();

        // List of currently active movement commands
        private List<MovementQuanta> activeQuanta = new List<MovementQuanta>();


        // Returns reference to starting state of movement system
        public MovementState GetStartingState()
        {
            return startingState;
        }

        // Adds a new polar command at given tick
        public void AddMovementCommand(int key, MovementQuanta command)
        {
            movementCommands.Add(key, command);
        }

        // Computes all positions and returns
        public List<(double, double)> GetPositions() {
            // List of positions per tick, starting at 0
            List<(double, double)> positions = new List<(double, double)>();

            // Reset state
            activeQuanta.Clear();
            MovementState movementState = new MovementState(startingState);

            // Set starting position
            positions.Add(movementState.Position);

            // Iterate while in gamepace and below tick max
            for (int tick = 0;
                (tick < MAXIMUM_POSITION_COUNT) && GameState.IsInGameSpace(movementState.Position);
                tick++)
            {

                // Update active quanta
                if (movementCommands.Has(tick))
                {
                    movementCommands.Get(tick).ForEach(
                        quanta => { activeQuanta.Add(quanta); quanta.StartingTick = tick; }
                    );
                }

                // Updates against static quanta
                activeQuanta.ForEach(quanta => quanta.UpdateExecution(tick - quanta.StartingTick, movementState));

                movementState.UpdateState();

                // Delete all executed quanta
                activeQuanta.RemoveAll(quanta => !quanta.IsExecuting(tick - quanta.StartingTick));

                positions.Add(movementState.Position);
            }

            return positions;
        }
    }
}
