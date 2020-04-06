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

        // Static gamespace size, gamespace is origin centered 
        static public double GAMESPACE_WIDTH = 960;
        static public double GAMESPACE_HEIGHT = 540;
        static public double GAMESPACE_PADDING = 200;

        // Default load location for levels

        // Current tick
        int tick = 0;

        // List of current enemies
        private List<Entity> enemies = new List<Entity>();

        // Returns if this position is in the played game state 
        public static bool IsInGameSpace(double x, double y)
        {
            // Check point is in box
            return (-(GAMESPACE_PADDING + GAMESPACE_WIDTH) < x
                && x < (GAMESPACE_PADDING + GAMESPACE_WIDTH)
                && -(GAMESPACE_PADDING + GAMESPACE_HEIGHT) < y
                && y < (GAMESPACE_PADDING + GAMESPACE_HEIGHT)
                );
        }

        // Updates this gamestate to represent a level
        // returns false on failture
        public bool LoadLevel(string level)
        {
            LoadParser loader = LoadParser.CreateLevelLoader(level);

            // Check for bad loader
            if (loader is null) goto load_fail;

            // Load all templates
            loader.LoadTemplates();

            // Load entities
            loader.LoadLevel();

            // Load entities
            this.enemies = loader.GetEntities();
            this.UpdateAllEntities();

            // Sucessfully loaded
            return true;

        load_fail:
            Console.WriteLine("Failed to load level " + level);
            return false;
            
        }

        // Private constructor
        private GameState()
        {
            this.LoadLevel("level/");
        }

        // Accessor for singleton instance
        public static GameState Get()
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
