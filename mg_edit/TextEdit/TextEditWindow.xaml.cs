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
            LevelTextBox.AppendText(contents);
        }

        // Reloads the level
        // Saves current file
        public void ReloadLevel(object sender, RoutedEventArgs e)
        {
            mainWindow.LoadLevel();
        }

    }
}
