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
using mg_edit.TextEdit.NewDialogue;

namespace mg_edit.TextEdit
{
    /// <summary>
    /// Interaction logic for ScriptLoadWindow.xaml
    /// </summary>
    public partial class EntityDefinitionLoadPanel : UserControl, ILoadablePanel
    {
        private EntityDefinition entDef;

        // Draw timings panel
        private void DrawTimingsPanel()
        {
            TimingsPanel.Children.Clear();

            // Add labels for timings
            foreach (int cycle in entDef.SpawningCycles)
            {
                Label label = new Label();
                label.Content = cycle.ToString();
                label.MouseDown += TimingsPanelChildren_Click;
                TimingsPanel.Children.Add(label);
            }

            Label plusLabel = new Label();
            plusLabel.Content = "+";
            plusLabel.MouseDown += TimingsPanelChildren_Click;
            TimingsPanel.Children.Add(plusLabel);
        }

        public EntityDefinitionLoadPanel(Loadable entDef)
        {
            InitializeComponent();

            // Initialise
            this.entDef = (EntityDefinition)entDef;

            DrawTimingsPanel();

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

        // Deletes this definition
        public void Delete_Click(object sender, RoutedEventArgs e)
        {
            GameState.GetLevel().RemoveLoadable(entDef);

            GameState.Get().TextEditWindow.DrawLoadablePanels();

            GameState.Get().MainWindow.UpdateEntityView(true);
        }

        // Deletes a label
        public void TimingsPanelChildren_Click(object sender, RoutedEventArgs e)
        {
            Label label = (Label)sender;
            int cycle = 0;
            if (int.TryParse(label.Content.ToString(), out cycle))
            {
                entDef.SpawningCycles.Remove(cycle);
                TimingsPanel.Children.Remove(label);
                if (entDef.SpawningCycles.Count > 0)
                {
                    GameState.Get().ReloadEntity(entDef);
                }
                else
                {
                    GameState.GetLevel().RemoveLoadable(entDef);
                    GameState.Get().TextEditWindow.DrawLoadablePanels();
                }
                GameState.Get().MainWindow.UpdateEntityView(true);

            }
            else
            {
                NewSpawningCycle window = new NewSpawningCycle();
                window.ShowDialog();

                if (window.Cycle >= 0)
                {
                    entDef.SpawningCycles.Add(window.Cycle);
                    entDef.SpawningCycles.Sort();

                    GameState.Get().ReloadEntity(entDef);
                    GameState.Get().MainWindow.UpdateEntityView(true);

                    DrawTimingsPanel();
                }
            }
            
        }
    }
}
