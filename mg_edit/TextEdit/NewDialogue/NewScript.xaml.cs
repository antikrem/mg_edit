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

namespace mg_edit.TextEdit.NewDialogue
{
    /// <summary>
    /// Interaction logic for NewScript.xaml
    /// </summary>
    public partial class NewScript : Window
    {
        public Script ScriptLoadable { get; set; } = null;

        public NewScript()
        {
            InitializeComponent();

            TickBox.Text = GameState.Get().Tick.ToString();
        }

        void AddScript_Click(object sender, RoutedEventArgs e)
        {
            List<int> spawningCycles = new List<int>();
            spawningCycles.Add(int.Parse(TickBox.Text));
            ScriptLoadable = new Script(spawningCycles);
            ScriptLoadable.ScriptBody = ScriptContent.Text;

            this.Close();
        }
    }
}
