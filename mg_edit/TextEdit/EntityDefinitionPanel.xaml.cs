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
using mg_edit.TextEdit.TemplatePanelParameter;

namespace mg_edit.TextEdit
{
    /// <summary>
    /// Interaction logic for ScriptLoadWindow.xaml
    /// </summary>
    public partial class EntityDefinitionLoadPanel : UserControl, ILoadablePanel
    {
        private EntityDefinition entDef;

        public EntityDefinitionLoadPanel(Loadable entDef)
        {
            InitializeComponent();

            // Initialise
            this.entDef = (EntityDefinition)entDef;

            // Add labels for timings
            foreach (int cycle in entDef.SpawningCycles)
            {
                Label label = new Label();
                label.Content = cycle.ToString();
                TimingsPanel.Children.Add(label);
            }

            // Add movement panel
            if (this.entDef.MovementSystem is object)
            {
                Panels.Children.Add(new MovementPanel(this.entDef));
            }


            // Add labels for templates
            foreach (var template in this.entDef.Templates)
            {
                Panels.Children.Add(new TemplatePanel(this.entDef, template));
            }
        }
    }
}
