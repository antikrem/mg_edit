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

        // Reloads the level
        // Saves current file
        public void ReloadLevel(object sender, RoutedEventArgs e)
        {
            mainWindow.ReloadLevel(LevelTextBox.Text);
        }

        // Loads the level
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

    }
}
