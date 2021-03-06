﻿using System;
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
using mg_edit.TextEdit.MovementPanels;
using mg_edit.Movement;

namespace mg_edit.TextEdit.NewDialogue
{
    /// <summary>
    /// Interaction logic for NewMovementCommandWindow.xaml
    /// </summary>
    public partial class NewMovementCommandWindow : Window
    {
        MovementQuanta command;
        EntityDefinition entity;

        public NewMovementCommandWindow(EntityDefinition ent)
        {
            InitializeComponent();

            this.entity = ent;
            CommandDropDown.Items.Add("AccelerateTo");
            CommandDropDown.Items.Add("Turn");
        }

        void CommandDropDown_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UserControl panel;
            int tick = Math.Max(0, GameState.Get().Tick - entity.SpawningCycles[0]);

            switch ((string)CommandDropDown.SelectedItem)
            {
                case "AccelerateTo":
                    command = new MovementPolarAccelerateTo(tick);
                    panel = new MovementPolarAccelerateToPanel(command);
                    break;
                case "Turn":
                    command = new MovementPolarTurn(tick);
                    panel = new MovementPolarTurnPanel(command);
                    break;
                default:
                    return;
            }

            ((IMovementPanel)panel).SetInternalEntityDefinition(entity);
            entity.MovementSystem.AddMovementCommand(tick, command);

            Grid.SetRow(panel, 1);
            Grid.SetColumn(panel, 0);
            Grid.SetColumnSpan(panel, 2);
            BodyGrid.Children.Add(panel);
        }

        void AddButton_Click(object sender, RoutedEventArgs e)
        {           
            entity.ReloadMovement();
            GameState.Get().MainWindow.UpdateEntityView(true);
            this.Close();
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
