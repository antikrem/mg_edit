using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Components
{
    class ComponentMovement : Component
    {

        // Static methods for updating entity

        private static void SetSpeed(string[] parameters, Entity entity)
        {
            entity.SetPolarSpeed(double.Parse(parameters[0]));

        }

        private static void SetAngle(string[] parameters, Entity entity)
        {
            entity.SetPolarAngle(double.Parse(parameters[0]));
        }

        //private static void 

        public ComponentMovement()
        {
            AddFunction("set_speed", SetSpeed, 1);
            AddFunction("set_angle", SetAngle, 1);
        }

        
    }
}
