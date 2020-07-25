using System;
using System.IO;
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
    /// Interaction logic for TemplateParameterCursorPosition.xaml
    /// </summary>
    public partial class TemplateParameterCursorPosition : UserControl, ITemplateParameter
    {
        private (double, double) posFromView = (0, 0);

        private EntityDefinition ent;
        private TemplateInstance template;
        int firstTarget;

        public TemplateParameterCursorPosition()
        {
            InitializeComponent();
        }

        public void UpdateCursorPosition((double, double) position)
        {
            posFromView = position;
        }

        public void UpdateFromView(object sender, RoutedEventArgs e)
        {
            TextboxXPosition.Text = posFromView.Item1.ToString();
            TextboxYPosition.Text = posFromView.Item2.ToString();
        }

        public void UpdatePosition(object sender, RoutedEventArgs e)
        {
            template.SetParameter(firstTarget, TextboxXPosition.Text);
            template.SetParameter(firstTarget+1, TextboxYPosition.Text);
        }

        public void InitialiseTemplate(string name, EntityDefinition ent, TemplateInstance template, int firstTarget)
        {
            FieldName.Content = name;

            this.ent = ent;
            this.template = template;
            this.firstTarget = firstTarget;

            UpdatePosition(null, null);

            ent.Reload();
        }
    }
}
