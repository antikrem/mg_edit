using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mg_edit.Loader;

namespace mg_edit.TextEdit.MovementPanels
{
    public interface IMovementPanel
    {

        // Sets internal entity definition
        void SetInternalEntityDefinition(EntityDefinition ent);
    }

}
