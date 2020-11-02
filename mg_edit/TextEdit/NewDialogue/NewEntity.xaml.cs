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
using mg_edit.Loader.Components;
using mg_edit.TextEdit.TemplatePanelParameter;

namespace mg_edit.TextEdit.NewDialogue
{

    /// <summary>
    /// Interaction logic for NewEntity.xaml
    /// </summary>
    public partial class NewEntity : Window
    {
        // lookup for components
        private Dictionary<string, Type> COMPONENT_LOOKUP = new Dictionary<string, Type>()
        {
            { "bulletMaster" , typeof(ComponentBulletMaster)}
        };

        private bool badClose = true;

        // Exported completed ent def
        private EntityDefinition entity;

        public NewEntity()
        {
            InitializeComponent();

            entity = new EntityDefinition(new List<int> { GameState.Get().Tick });


            // Add templates
            foreach (KeyValuePair<string, Template> entry in GameState.GetLevel().Templates)
            {
                TemplateComboBox.Items.Add(entry.Key);
            }

            // Add Components
            foreach (KeyValuePair<string, Type> entry in COMPONENT_LOOKUP)
            {
                ComponentComboBox.Items.Add(entry.Key);
            }

            Label label = new Label()
            {
                Content = GameState.Get().Tick.ToString()
            };
            TimingsPanel.Children.Add(label);


            GameState.GetLevel().AddLoadable(entity);
            GameState.Get().ReloadEntity(entity);
            GameState.Get().TextEditWindow.DrawLoadablePanels();
        }

        public void TemplateComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Template template = GameState.GetLevel().Templates[TemplateComboBox.SelectedValue.ToString()];
            TemplateInstance instance = new TemplateInstance(template);

            entity.AddTemplate(instance);

            entity.ReloadTemplates();
            entity.ReloadMovement();

            TemplatePanel panel = new TemplatePanel(entity, instance);
            TemplatePanels.Children.Add(panel);

            entity.ForceNewPanel = true;
            GameState.Get().ReloadEntity(entity);
            GameState.Get().TextEditWindow.DrawLoadablePanels();
        }

        public void ComponentComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string componentName = ComponentComboBox.SelectedValue.ToString();
            Component component = (Component)Activator.CreateInstance(COMPONENT_LOOKUP[componentName]);

            entity.AddComponent(componentName, component);

            entity.ReloadTemplates();
            entity.ReloadMovement();

            TemplatePanels.Children.Add(((InstanceableComponent)component).GetPanel(entity));

            entity.ForceNewPanel = true;
            GameState.Get().ReloadEntity(entity);
            GameState.Get().TextEditWindow.DrawLoadablePanels();
        }

        public void Export_Click(object sender, RoutedEventArgs e)
        {
            entity.ForceNewPanel = true;
            GameState.Get().ReloadEntity(entity);
            GameState.Get().TextEditWindow.DrawLoadablePanels();

            badClose = false;

            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (badClose)
            {
                GameState.GetLevel().RemoveLoadable(entity);

                GameState.Get().TextEditWindow.DrawLoadablePanels();
            }
        }
    }
}
