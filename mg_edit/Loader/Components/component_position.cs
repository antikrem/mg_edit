using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    class ComponentPosition : Component
    {
        // Sets position
        override public void Initialise(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.GetStartingState().Position = (double.Parse(parameters[0]), double.Parse(parameters[1]));
        }

        public ComponentPosition()
        {
            
        }

        public override string ComposeSaveDirective(EntityDefinition entDef)
        {
            var startingPosition = entDef.MovementSystem.GetStartingState().Position;
            return "+postion " + startingPosition.Item1.ToString() + " " + startingPosition.Item2.ToString() + " 0";
        }
    }
}
