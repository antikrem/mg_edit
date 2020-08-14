using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mg_edit.Loader;

namespace mg_edit.TextEdit.TemplatePanelParameter
{
    public interface ITemplateParameter
    {
        // Method called to set the name, Entity and 
        void InitialiseTemplate(string name, EntityDefinition ent, TemplateInstance template, int target);

        // Returns the number of parameters associated with this panel
        int GetParameterCount();

        // Returns default parameters for this parameter
        List<string> GetDefaultParameters();
    }
}
