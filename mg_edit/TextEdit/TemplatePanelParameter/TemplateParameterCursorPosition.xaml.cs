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
        private EntityDefinition ent;
        private TemplateInstance template;
        int firstTarget;

        public TemplateParameterCursorPosition()
        {
            InitializeComponent();
        }

        public void UpdateFromView(object sender, RoutedEventArgs e)
        {
            TextboxXPosition.Text = GameState.Get().CursorPosition.Item1.ToString();
            TextboxYPosition.Text = GameState.Get().CursorPosition.Item2.ToString();
            UpdatePosition(null, null);
        }

        public void UpdatePosition(object sender, RoutedEventArgs e)
        {
            template.SetParameter(firstTarget, TextboxXPosition.Text);
            template.SetParameter(firstTarget+1, TextboxYPosition.Text);

            ent.ReloadTemplates();
            ent.ReloadMovement();

            GameState.Get().MainWindow.UpdateEntityView(true);
        }

        public void InitialiseTemplate(string name, EntityDefinition ent, TemplateInstance template, int firstTarget)
        {
            FieldName.Content = name;

            this.ent = ent;
            this.template = template;

            this.firstTarget = firstTarget;
            TextboxXPosition.Text = template.GetParameter(firstTarget);
            TextboxYPosition.Text = template.GetParameter(firstTarget+1);

            UpdatePosition(null, null);

            TextboxXPosition.TextChanged += UpdatePosition;
            TextboxYPosition.TextChanged += UpdatePosition;
        }

        public int GetParameterCount()
        {
            return 2;
        }

        public List<string> GetDefaultParameters()
        {
            return new List<string> { GameState.Get().CursorPosition.Item1.ToString(), GameState.Get().CursorPosition.Item2.ToString() };
        }
    }
}
