using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Movement
{
    // Encapsulates updates in the 
    public class MovementSystem
    {
        // A hard cap on number of positions, to avoid infinite loops
        private const int MAXIMUM_POSITION_COUNT = 100000;

        // Starting state of player
        private MovementState startingState = new MovementState();

        // List containing all static movement commands
        public List<MovementQuanta> MovementCommands { get; } = new List<MovementQuanta>();

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
            command.StartingTick = key;
            MovementCommands.Add(command);
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
                MovementCommands.ForEach(
                        quanta => {
                                if (quanta.StartingTick == tick)
                                {
                                    activeQuanta.Add(quanta);
                                }
                                
                            }
                    );

                // Updates against static quanta
                activeQuanta.ForEach(quanta => quanta.UpdateExecution(tick - quanta.StartingTick, movementState));

                movementState.UpdateState();

                // Delete all executed quanta
                activeQuanta.RemoveAll(quanta => !quanta.IsExecuting(tick - quanta.StartingTick));

                // On NaN, end early
                if (double.IsNaN(movementState.Position.Item1) || double.IsNaN(movementState.Position.Item2)) {
                    return positions;
                }

                positions.Add(movementState.Position);
            }

            return positions;
        }
    }
}
