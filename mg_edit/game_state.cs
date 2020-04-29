using System;
using System.Collections.Generic;
using mg_edit.TextEdit;

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

        // Additional padding to end of level
        private const int LEVEL_LENGTH_PADDING = 200;

        // Length of level in ticks
        private int levelLength = LEVEL_LENGTH_PADDING;

        // Current tick
        public int Tick {get; set;}

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

            // Set level length
            this.levelLength = loader.GetLevelLength() + LEVEL_LENGTH_PADDING;

            // Sucessfully loaded
            return true;

        load_fail:
            Console.WriteLine("Failed to load level " + level);
            return false;
            
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

        // Updates all entities
        public void UpdateAllEntities()
        {
            enemies.ForEach(enemy => enemy.UpdatePositions());
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
