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

using mg_edit.Loader;

namespace mg_edit.Dialogue
{
    /// <summary>
    /// Interaction logic for ShiftLevelDialogue.xaml
    /// </summary>
    public partial class ShiftLevelDialogue : Window
    {
        public ShiftLevelDialogue()
        {
            InitializeComponent();

            EndBox.Text = GameState.Get().GetLevelTotalLength().ToString();
        }

        public void Shift_Click(object sender, RoutedEventArgs e)
        {
            LoadParser level = GameState.GetLevel();

            int start = 0;
            int end = 0;
            int shift = 0;

            int.TryParse(StartBox.Text, out start);
            int.TryParse(EndBox.Text, out end);
            int.TryParse(ShiftBox.Text, out shift);

            foreach (Loadable loadable in level.Loadables)
            {
                for (int i = 0; i < loadable.SpawningCycles.Count; i++)
                {
                    int cycle = loadable.SpawningCycles[i];
                    if (start <= cycle && cycle <= end)
                    {
                        loadable.SpawningCycles[i] = cycle + shift;
                        loadable.ForceNewPanel = true;
                    }
                }
            }

            GameState.Get().MainWindow.ReloadLevel();
            GameState.Get().TextEditWindow.DrawLoadablePanels();

            this.Close();
        }

        public void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
