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
using mg_edit.TextEdit.TemplatePanelParameter;

namespace mg_edit.TextEdit.NewDialogue
{
    /// <summary>
    /// Interaction logic for NewEntity.xaml
    /// </summary>
    public partial class NewEntity : Window
    {

        // In devlopement entity definition
        EntityDefinition inDev;

        // Exported completed ent def
        public EntityDefinition Entity {get; set;}

        public NewEntity()
        {
            InitializeComponent();

            inDev = new EntityDefinition(new List<int> { GameState.Get().Tick });

            foreach (KeyValuePair<string, Template> entry in GameState.GetLevel().Templates)
            {
                TemplateComboBox.Items.Add(entry.Key);
            }

            Label label = new Label()
            {
                Content = GameState.Get().Tick.ToString()
            };
            TimingsPanel.Children.Add(label);
        }

        public void TemplateComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Template template = GameState.GetLevel().Templates[TemplateComboBox.SelectedValue.ToString()];
            TemplateInstance instance = new TemplateInstance(template);

            inDev.AddTemplate(instance);

            inDev.ReloadTemplates();
            inDev.ReloadMovement();

            TemplatePanel panel = new TemplatePanel(inDev, instance);
            TemplatePanels.Children.Add(panel);
        }

        public void Export_Click(object sender, RoutedEventArgs e)
        {
            Entity = inDev;
            this.Close();
        }
    }
}
