using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mg_edit.Movement;

namespace mg_edit.Components
{
    class ComponentStaticMovementCreator : ComponentCreator
    {

        // Static methods for updating entity
        private static void AddPolarTurn(string[] parameters, Entity entity)
        {
            entity.GetMovementSystem().AddMovementCommand(
                Int32.Parse(parameters[0]),
                new MovementPolarTurn(
                    Double.Parse(parameters[2]) / Int32.Parse(parameters[1]),
                    Int32.Parse(parameters[1])
                )
            );
        }


        public ComponentStaticMovementCreator()
        {
            AddFunction("add_polar_turn", AddPolarTurn, 3);
            

        }


    }
}
