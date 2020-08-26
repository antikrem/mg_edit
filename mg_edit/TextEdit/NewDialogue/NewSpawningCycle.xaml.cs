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

namespace mg_edit.TextEdit.NewDialogue
{
    /// <summary>
    /// Interaction logic for NewSpawningCycle.xaml
    /// </summary>
    public partial class NewSpawningCycle : Window
    {
        int _cycle = -1;
        public int Cycle
        {
            get
            {
                return _cycle;
            }
            set
            {
                _cycle = value;
            }
        }

        public NewSpawningCycle()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(CycleBox.Text, out _cycle);
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
