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
    public partial class EntityDefinitionLoadPanel : UserControl, ILoadablePanel
    {
        private Loadable loadable;

        public EntityDefinitionLoadPanel(Loadable entDef)
        {
            InitializeComponent();

            // Initialise
            this.loadable = entDef;
            ScriptBody.Text = "Hello";
        }
    }
}
