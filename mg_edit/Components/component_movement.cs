﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Components
{
    class ComponentMovement : Component
    {

        // Static methods for updating entity

        // Set cartesian speed of entity
        private static void SetPolarSpeed(string[] parameters, Entity entity)
        {
            entity.GetMovementSystem().SetPolarSpeed(double.Parse(parameters[0]));
        }

        // Set cartesian angle of entity
        private static void SetPolarAngle(string[] parameters, Entity entity)
        {
            entity.GetMovementSystem().SetPolarAngle(double.Parse(parameters[0]));
        }

        // Set cartesian speed change of entity
        private static void SetPolarSpeedChange(string[] parameters, Entity entity)
        {
            entity.GetMovementSystem().SetPolarSpeedChange(double.Parse(parameters[0]));
        }

        // Set cartesian angle change of entity
        private static void SetPolarAngleChange(string[] parameters, Entity entity)
        {
            entity.GetMovementSystem().SetPolarAngleChange(double.Parse(parameters[0]));
        }

        // Set cartesian speed cap of entity
        private static void SetPolarSpeedCap(string[] parameters, Entity entity)
        {
            entity.GetMovementSystem().SetPolarSpeedChange(double.Parse(parameters[0]));
        }

        // Set cartesian angle cap of entity
        private static void SetPolarAngleCap(string[] parameters, Entity entity)
        {
            entity.GetMovementSystem().SetPolarAngleChange(double.Parse(parameters[0]));
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

        
    }
}
