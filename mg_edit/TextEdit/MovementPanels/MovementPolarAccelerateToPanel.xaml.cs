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
using mg_edit.Movement;

namespace mg_edit.TextEdit.MovementPanels
{
    /// <summary>
    /// Interaction logic for MovementPolarAccelerateToPanel.xaml
    /// </summary>
    public partial class MovementPolarAccelerateToPanel : UserControl, IMovementPanel
    {
        MovementPolarAccelerateTo command;
        EntityDefinition entity;

        public MovementPolarAccelerateToPanel(MovementQuanta command)
        {
            InitializeComponent();

            this.command = (MovementPolarAccelerateTo)command;

            // Populate entries
            TickBox.Text = this.command.StartingTick.ToString();
            DurationBox.Text = this.command.Duration.ToString();
            EndSpeed.Text = this.command.EndingSpeed.ToString();
        }

        public void SetInternalEntityDefinition(EntityDefinition ent)
        {
            this.entity = ent;
        }
    }
}
