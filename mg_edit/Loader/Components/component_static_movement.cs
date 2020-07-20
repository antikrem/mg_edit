using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mg_edit.Movement;

namespace mg_edit.Loader
{
    class ComponentStaticMovement : Component
    {

        // Static methods for updating entity
        private static void AddPolarTurn(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.AddMovementCommand(
                Int32.Parse(parameters[0]),
                new MovementPolarTurn(
                    Double.Parse(parameters[2]) / Int32.Parse(parameters[1]),
                    Int32.Parse(parameters[1])
                )
            );
        }

        private static void AddPolarAccelerateTo(string[] parameters, EntityDefinition entDef)
        {
            entDef.MovementSystem.AddMovementCommand(
                Int32.Parse(parameters[0]),
                new MovementPolarAccelerateTo(
                    Double.Parse(parameters[2]),
                    Int32.Parse(parameters[1])
                )
            );
        }

        public ComponentStaticMovement()
        {
            AddFunction("add_polar_turn", AddPolarTurn, 3);
            AddFunction("add_polar_accelerate_to", AddPolarAccelerateTo, 3);

        }

        public override string ComposeSaveDirective(EntityDefinition entDef)
        {
            string body = "+staticMovement";

            entDef.MovementSystem.MovementCommands.AsList()
                .ForEach(x => body = body + "\n" + x.Item2.ComposeSaveDefinition(x.Item1));

            return body;
        }
    }
}
