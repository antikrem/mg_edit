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
            LevelTextBox.Clear();
            string contents = File.ReadAllText(filepath + LoadParser.LOAD_TABLE_FILE);
            LevelTextBox.AppendText(contents);
        }

        // Handle to reload the level
        public void ReloadLevel(object sender, RoutedEventArgs e)
        {
            // Check if a loader exists 
            if (GameState.Get().Loader is object)
            {
                mainWindow.ReloadLevel(LevelTextBox.Text);
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
        }

        // Handle to save current file
        public void SaveLevel(object sender, RoutedEventArgs e)
        {
            ReloadLevel(null, null);
            GameState.Get().Loader.SaveLevel();
        }

        // Handle to begin adding template dialogue
        public void AddTemplate(object sender, RoutedEventArgs e)
        {
            // Shoe dialogue
            var templateInstancer = new TemplateInstanceDialogue(this);
            AddTemplateButton.IsEnabled = false;
            templateInstancer.Show();
        }

        // Renables add template button
        public void ReenableAddTemplateButton()
        {
            AddTemplateButton.IsEnabled = true;
        }

    }
}
