using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    class ComponentPositionCreator : ComponentCreator
    {
        // Sets position
        override public void Initialise(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.GetStartingState().Position = (Int32.Parse(parameters[0]), Int32.Parse(parameters[1]));
        }

        public ComponentPositionCreator()
        {
            
        }

        public override string ComposeSaveDirective(EntityDefinition entDef)
        {
            var startingPosition = entDef.MovementSystem.GetStartingState().Position;
            return "+postion " + startingPosition.Item1.ToString() + " " + startingPosition.Item2.ToString() + " 0";
        }
    }
}
