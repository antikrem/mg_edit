using System;
using System.Collections.Generic;
using mg_edit.Loader;

using mg_edit.TextEdit;


namespace mg_edit
{
    // Represents static gamestate
    // Uses a singleton design pattern
    class GameState
    {
        static private GameState instance = null;

        public MainWindow MainWindow { get; set; }
        public TextEditWindow TextEditWindow { get; set; }

        // Static gamespace size, gamespace is origin centered 
        static public double GAMESPACE_WIDTH = 960;
        static public double GAMESPACE_HEIGHT = 540;
        static public double GAMESPACE_PADDING = 200;

        // Additional padding to end of level
        private const int LEVEL_LENGTH_PADDING = 200;

        // Length of level in ticks
        private int levelLength = LEVEL_LENGTH_PADDING;

        // Current tick
        public int Tick {get; set;}

        // Current cursor position in game space
        public (double, double) CursorPosition { get; set; } = (0.0, 0.0);

        // Current level path
        public string LevelFolder { get; set; } = null;

        // Last load parser
        public LoadParser Loader { get; set; } = null;

        // List of current enemies
        private List<Entity> enemies = new List<Entity>();

        // Returns if this position is in the played game state 
        public static bool IsInGameSpace((double, double) position)
        {
            // Check point is in box
            return (
                -(GAMESPACE_PADDING + GAMESPACE_WIDTH) < position.Item1
                && position.Item1 < (GAMESPACE_PADDING + GAMESPACE_WIDTH)
                && -(GAMESPACE_PADDING + GAMESPACE_HEIGHT) < position.Item2
                && position.Item2 < (GAMESPACE_PADDING + GAMESPACE_HEIGHT)
            );
        }

        // Updates this gamestate to represent a level
        // returns false on failure
        public bool LoadLevel()
        {
            if (LevelFolder is null) return false;

            Loader = LoadParser.CreateLevelLoaderFromFile(LevelFolder);

            // Check for bad loader
            if (Loader is null)  return false;

            // Load all templates
            Loader.LoadTemplates();

            // Load level to load file
            Loader.LoadLevelLoadTableFromFile();

            // Load entities
            Loader.LoadLevel();

            // Sucessfully loaded
            return this.ReloadLevel();
            
        }

        // Reloads the level with just the updated level body
        public bool ReloadLevel()
        {
            // Load entities
            Loader.EvaluateEntities();
            this.enemies = Loader.GetEntities();

            // Set level length
            this.levelLength = Loader.GetLevelLength() + LEVEL_LENGTH_PADDING;

            return true;
        }

        // Private constructor
        private GameState()
        {
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

        // Gets the current load parser
        static public LoadParser GetLevel()
        {
            return GameState.Get().Loader;
        }

        // Gets total level length
        public int GetLevelTotalLength()
        {
            return this.levelLength;
        } 

        // Get all entities that are spawned this tick
        public List<Entity> GetActiveEntities()
        {
            var visibleEnemies = new List<Entity>();

            foreach (var enemy in enemies)
            {
                if (enemy.GetSpawningTick() <= this.Tick &&
                    this.Tick <= enemy.GetSpawningTick() + enemy.GetLifetime())
                {
                    visibleEnemies.Add(enemy);
                }
            }

            return visibleEnemies;
        }

    }
}
