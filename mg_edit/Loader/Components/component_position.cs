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
            double x = 0;
            double y = 0;

            double.TryParse(parameters[0], out x);
            double.TryParse(parameters[1], out y);


            entDef.MovementSystem.GetStartingState().Position = (Math.Floor(x), Math.Floor(y));
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
