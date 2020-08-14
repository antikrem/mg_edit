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
    /// Interaction logic for TemplateParameterDefault.xaml
    /// </summary>
    public partial class TemplateParameterDefault : UserControl, ITemplateParameter
    {
        private EntityDefinition ent;
        private TemplateInstance template;
        int firstTarget;

        public TemplateParameterDefault()
        {
            InitializeComponent();
        }

        public void InitialiseTemplate(string name, EntityDefinition ent, TemplateInstance template, int firstTarget)
        {
            FieldName.Content = name;

            this.ent = ent;
            this.template = template;

            this.firstTarget = firstTarget;
            ValueBox.Text = template.GetParameter(firstTarget);

            ValueBox.TextChanged += Update;
        }

        public void Update(object sender, RoutedEventArgs e)
        {
            template.SetParameter(firstTarget, ValueBox.Text);
        }

        public int GetParameterCount()
        {
            return 1;
        }

        public List<string> GetDefaultParameters()
        {
            return new List<string> { "" };
        }
    }
}
