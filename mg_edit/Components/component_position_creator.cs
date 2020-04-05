using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Components
{
    class ComponentPositionCreator : ComponentCreator
    {
        // Sets position
        override public void Initialise(string[] parameters, Entity ent)
        {
            ent.GetMovementSystem().SetPosition(Int32.Parse(parameters[0]), Int32.Parse(parameters[1]));
        }

        public ComponentPositionCreator()
        {
            
        }


    }
}
