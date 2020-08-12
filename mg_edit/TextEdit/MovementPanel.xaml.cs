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
using mg_edit.TextEdit.MovementPanels;

namespace mg_edit.TextEdit
{
    /// <summary>
    /// Interaction logic for MovementPanel.xaml
    /// </summary>
    public partial class MovementPanel : UserControl
    {
        EntityDefinition entity;

        static private IMovementPanel CreateMovementPanel(MovementQuanta command)
        {
            if (command is MovementPolarAccelerateTo)
            {
                return new MovementPolarAccelerateToPanel(command);
            }
            else if (command is MovementPolarTurn)
            {
                return new MovementPolarTurnPanel(command);
            }
            else
            {
                return null;
            }
                
        }

        // Redraw movement panel
        public void Redraw()
        {
            MovementStackPanel.Children.Clear();
            // Draw each panel
            foreach (MovementQuanta mov in entity.MovementSystem.MovementCommands)
            {
                IMovementPanel panel = CreateMovementPanel(mov);
                panel.SetInternalEntityDefinition(entity);
                MovementStackPanel.Children.Add((UserControl)panel);
            }
        }

        public MovementPanel(EntityDefinition entityDefinition)
        {
            InitializeComponent();

            this.entity = entityDefinition;

            Redraw();
        }

        // Open command window
        public void AddCommand_Click(object sender, RoutedEventArgs e)
        {
            NewMovementCommandWindow window = new NewMovementCommandWindow(entity);
            window.ShowDialog();

            
            Redraw();

        }
    }
}
