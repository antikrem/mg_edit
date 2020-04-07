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

            // Modified Movement state
            MovementState movementState = new MovementState(startingState);

            // Set starting position
            positions.Add(movementState.Position);

            // Iterate while in gamepace and below tick max
            for (int tick = 0;
                (tick < MAXIMUM_POSITION_COUNT) && GameState.IsInGameSpace(movementState.Position);
                tick++)
            {
                movementState.UpdateState();

                positions.Add(movementState.Position);
            }

            return positions;
        }
    }
}
