using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    class ComponentMovementCreator : ComponentCreator
    {

        // Static methods for updating entity

        // Set cartesian speed of entity
        private static void SetPolarSpeed(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.GetStartingState().Speed = double.Parse(parameters[0]);
        }

        // Set cartesian angle of entity
        private static void SetPolarAngle(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.GetStartingState().Angle = double.Parse(parameters[0]);
        }

        // Set cartesian speed change of entity
        private static void SetPolarSpeedChange(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.GetStartingState().SpeedChange = double.Parse(parameters[0]);
        }

        // Set cartesian angle change of entity
        private static void SetPolarAngleChange(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.GetStartingState().AngleChange = double.Parse(parameters[0]);
        }

        // Set cartesian speed cap of entity
        private static void SetPolarSpeedCap(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.GetStartingState().SpeedCap = double.Parse(parameters[0]);
        }

        // Set cartesian angle cap of entity
        private static void SetPolarAngleCap(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.GetStartingState().AngleCap = double.Parse(parameters[0]);
        }

        public ComponentMovementCreator()
        {
            AddFunction("set_speed", SetPolarSpeed, 1);
            AddFunction("set_angle", SetPolarAngle, 1);

            AddFunction("set_speed_change", SetPolarSpeedChange, 1);
            AddFunction("set_angle_change", SetPolarAngle, 1);

            AddFunction("set_speed_cap", SetPolarSpeedCap, 1);
            AddFunction("set_angle_cap", SetPolarAngleCap, 1);

        }

        public override string ComposeSaveDirective(EntityDefinition entDef)
        {
            return "+movement";
        }
    }
}
