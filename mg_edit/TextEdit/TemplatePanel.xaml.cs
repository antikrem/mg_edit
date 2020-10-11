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

namespace mg_edit.TextEdit.TemplatePanelParameter
{
    /// <summary>
    /// Interaction logic for TemplatePanel.xaml
    /// </summary>
    public partial class TemplatePanel : UserControl
    {

        private const int MAX_NAME_LENGTH = 10;

        // Returns the correct parameter panel for a given type
        static public ITemplateParameter CreateTemplateParameterPanel(string parameterType)
        {
            switch (parameterType)
            {
                case "cursor_position":
                    return new TemplateParameterCursorPosition();

                case "radial_select":
                    return new TemplateParameterCursorPosition();

                default:
                    return new TemplateParameterDefault();
            }
            
            
        }


        public TemplatePanel(EntityDefinition entDef, TemplateInstance template)
        {
            InitializeComponent();

            Title.Content = template.Template.Name.Replace("_", "__").Substring(0, MAX_NAME_LENGTH);

            if (template.Template.ParameterNames is null)
            {
                return;
            }

            int templateIndex = 0;
            for (int i = 0; i < template.Template.ParameterNames.Count; i++)
            {
                ITemplateParameter templateParameterPanel = CreateTemplateParameterPanel(template.Template.ParameterTypes[i]);
                templateParameterPanel.InitialiseTemplate(template.Template.ParameterNames[i], entDef, template, templateIndex);
                TemplateStackPanel.Children.Add((UserControl)templateParameterPanel);

                templateIndex += templateParameterPanel.GetParameterCount();
            }
        }
    }
}
