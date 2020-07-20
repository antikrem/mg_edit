using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    class ComponentBulletMaster : Component
    {
        // Name of bullet master
        private string Name { get; set; }

        // Name of bullet master
        private int Delay { get; set; } = 0;

        // Sets position
        override public void Initialise(string[] parameters, EntityDefinition entDef)
        {
            Name = parameters[0];
            if (parameters.Length > 1)
            {
                Delay = Int32.Parse(parameters[1]);
            }
        }

        public ComponentBulletMaster()
        {

        }

        public override string ComposeSaveDirective(EntityDefinition entDef)
        {
            return "+bulletMaster " + Name + (Delay != 0 ? " " + Delay.ToString() : "");
        }
    }
}
