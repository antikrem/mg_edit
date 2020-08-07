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
    /// Interaction logic for MovementPolarTurnPanel.xaml
    /// </summary>
    public partial class MovementPolarTurnPanel : UserControl, IMovementPanel
    {
        MovementPolarTurn command;
        EntityDefinition entity;

        public MovementPolarTurnPanel(MovementQuanta command)
        {
            InitializeComponent();

            this.command = (MovementPolarTurn)command;

            // Populate entries
            TickBox.Text = this.command.StartingTick.ToString();
            DurationBox.Text = this.command.Duration.ToString();
            RateBox.Text = (this.command.Rate * this.command.Duration).ToString();
        }

        public void SetInternalEntityDefinition(EntityDefinition ent)
        {
            this.entity = ent;
        }

    }
}
