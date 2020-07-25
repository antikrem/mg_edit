using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mg_edit.Loader;

namespace mg_edit.TextEdit.TemplatePanelParameter
{
    interface ITemplateParameter
    {
        // Method called when cursor position is updated
        void UpdateCursorPosition((double, double) position);

        // Method called to set the name, Entity and 
        void InitialiseTemplate(string name, EntityDefinition ent, TemplateInstance template, int target);

    }
}
