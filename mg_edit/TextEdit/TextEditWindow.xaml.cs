using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using mg_edit.Dialogue;
using mg_edit.Loader;
using mg_edit.TextEdit.NewDialogue;
using mg_edit.TextEdit.TemplatePanelParameter;
using mg_edit.Helper;

namespace mg_edit.TextEdit
{
    public partial class TextEditWindow : Window
    {

        MainWindow mainWindow;

        public TextEditWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.Owner = mainWindow;
            this.mainWindow = mainWindow;
        }

        // Updates this text editor video with a given text file
        public void UpdateText(string filepath)
        {
            string contents = File.ReadAllText(filepath + LoadParser.LOAD_TABLE_FILE);
        }

        // Handle to reload the level
        public void ReloadLevel(object sender, RoutedEventArgs e)
        {
            // Check if a loader exists 
            if (GameState.Get().Loader is object)
            {
                mainWindow.ReloadLevel();
            }
            else
            {
                LoadLevel(sender, e);
            }
            
        }

        // Handle to load a level
        public void LoadLevel(object sender, RoutedEventArgs e)
        {
            // Set up path 
            var levelLoadDialogue = new LevelLoadDialogue();
            levelLoadDialogue.ShowDialog();

            // If valid load level
            if (levelLoadDialogue.Path is string)
            {
                GameState.Get().LevelFolder = levelLoadDialogue.Path;
                mainWindow.LoadLevel();
            }

            this.DrawLoadablePanels();
        }

        // Handle to save current file
        public void SaveLevel(object sender, RoutedEventArgs e)
        {
            GameState.Get().Loader.SaveLevel();
        }

        // Takes all loadables and displays loadable panels
        public void DrawLoadablePanels()
        {
            LoadablePanels.Children.Clear();
            LoadParser level = GameState.GetLevel();

            if (level is null)
            {
                return;
            }

            IntComparison.QSort(level.Loadables);

            foreach (Loadable loadable in level.Loadables)
            {
                LoadablePanels.Children.Add((UIElement)loadable.GetLoadablePanel());
            }
        }

        // Handle to begin adding template dialogue
        public void AddEntity_Click(object sender, RoutedEventArgs e)
        {
            // Show dialogue
            NewEntity window = new NewEntity();
            window.ShowDialog();

            if (window.Entity is object)
            {
                GameState.GetLevel().AddLoadable(window.Entity);
                DrawLoadablePanels();
                GameState.Get().ReloadLevel();
            }
        }

        public void AddScript_Click(object sender, RoutedEventArgs e)
        {
            NewScript window = new NewScript();
            window.ShowDialog();

            if (window.ScriptLoadable is object)
            {
                GameState.GetLevel().AddLoadable(window.ScriptLoadable);
                DrawLoadablePanels();
            }
        }
    }
}
