using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit
{
    // Represents static gamestate
    // Uses a singleton design pattern
    class GameState
    {
        static private GameState instance = null;

        // Static gamespace size
        static public double GAMESPACE_WIDTH = 1920;
        static public double GAMESPACE_HEIGHT = 1080;
        static public double GAMESPACE_PADDING = 200;

        // Current tick
        int tick = 0;

        // List of current enemies
        private List<Entity> enemies = new List<Entity>();

        // Returns if this position is in the played game state 
        public static bool IsInGameSpace(double x, double y)
        {
            // Check point is in box
            return (-GAMESPACE_PADDING < x
                && x < GAMESPACE_PADDING + GAMESPACE_WIDTH
                && -GAMESPACE_PADDING < y
                && y < GAMESPACE_PADDING + GAMESPACE_HEIGHT
                );
        }

        // Private constructor
        private GameState()
        {

        }

        // Accessor for singleton instance
        public GameState Get()
        {
            if (instance == null)
            {
                instance = new GameState();
            }

            return instance;
        }

        // Updates all entities
        public void UpdateAllEntities()
        {
            foreach (var enemy in enemies)
            {
                enemy.UpdatePositions();
            }
        }

        // Get all entities that are spawned this tick
        public List<Entity> GetActiveEntities()
        {
            var visibleEnemies = new List<Entity>();

            foreach (var enemy in enemies)
            {
                if (enemy.GetSpawningTick() >= this.tick &&
                    enemy.GetSpawningTick() + enemy.GetLifetime() <= this.tick)
                {
                    visibleEnemies.Add(enemy);
                }
            }

            return visibleEnemies;
        }

    }
}
