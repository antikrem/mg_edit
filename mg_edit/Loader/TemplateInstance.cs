using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    class TemplateInstance
    {
        // Name of template this references
        public string Name { get; }

        // String of parameters
        public List<string> Parameters { set; get; }

        // Construct with name
        public TemplateInstance(string name)
        {
            this.Name = name;
        }

        // Construct with name and parameters
        public TemplateInstance(string name, List<string> parameters)
        {
            this.Name = name;
            this.Parameters = parameters;
        }

    }
}
