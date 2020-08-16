using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    class ComponentMovement : Component
    {

        // Static methods for updating entity

        // Set cartesian speed of entity
        private static void SetPolarSpeed(string[] parameters, EntityDefinition entDef)
        {
            double value = 0;
            double.TryParse(parameters[0], out value);
            entDef.MovementSystem.GetStartingState().Speed = value;
        }

        // Set cartesian angle of entity
        private static void SetPolarAngle(string[] parameters, EntityDefinition entDef)
        {
            double value = 0;
            double.TryParse(parameters[0], out value);
            entDef.MovementSystem.GetStartingState().Angle = value;
        }

        // Set cartesian speed change of entity
        private static void SetPolarSpeedChange(string[] parameters, EntityDefinition entDef)
        {
            double value = 0;
            double.TryParse(parameters[0], out value);
            entDef.MovementSystem.GetStartingState().SpeedChange = value;
        }

        // Set cartesian angle change of entity
        private static void SetPolarAngleChange(string[] parameters, EntityDefinition entDef)
        {
            double value = 0;
            double.TryParse(parameters[0], out value);
            entDef.MovementSystem.GetStartingState().AngleChange = value;
        }

        // Set cartesian speed cap of entity
        private static void SetPolarSpeedCap(string[] parameters, EntityDefinition entDef)
        {
            double value = 0;
            double.TryParse(parameters[0], out value);
            entDef.MovementSystem.GetStartingState().SpeedCap = value;
        }

        // Set cartesian angle cap of entity
        private static void SetPolarAngleCap(string[] parameters, EntityDefinition entDef)
        {
            double value = 0;
            double.TryParse(parameters[0], out value);
            entDef.MovementSystem.GetStartingState().AngleCap = value;
        }

        public ComponentMovement()
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
