using System.Linq;
using System.Windows;
using mg_edit.TextEdit;
using System.ComponentModel;

namespace mg_edit.Dialogue
{
    /// <summary>
    /// Interaction logic for TemplateInstanceDialogue.xaml
    /// </summary>
    public partial class TemplateInstanceDialogue : Window
    {
        private TextEditWindow editWindow;

        public TemplateInstanceDialogue(TextEditWindow owner)
        {
            InitializeComponent();
            this.Owner = owner;
            editWindow = owner;
        }


        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            this.editWindow.ReenableAddTemplateButton();
        }
    }
}
