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
using System.Windows.Navigation;
using System.Windows.Shapes;

using mg_edit.Loader;

namespace mg_edit.TextEdit
{
    /// <summary>
    /// Interaction logic for ScriptLoadWindow.xaml
    /// </summary>
    public partial class ScriptLoadPanel : UserControl, ILoadablePanel
    {
        private Loadable loadable;

        public ScriptLoadPanel(Script script)
        {
            InitializeComponent();

            // Initialise
            this.loadable = script;
            ScriptBody.Text = script.ScriptBody;

            // Add labels for timings
            foreach (int cycle in script.SpawningCycles)
            {
                Label label = new Label();
                label.Content = cycle.ToString();
                TimingsPanel.Children.Add(label);
            }
        }

        // Deletes this definition
        public void Delete_Click(object sender, RoutedEventArgs e)
        {
            GameState.GetLevel().RemoveLoadable(loadable);
            GameState.Get().TextEditWindow.DrawLoadablePanels();
        }
    }
}
