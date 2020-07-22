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

        // Return substituted template
        public string GetSubbedTemplate(LoadParser parser)
        {
            string body = parser.Templates[Name];

            for (int i = 1; i <= Parameters.Count; i++)
            {
                body = body.Replace("%" + i.ToString(), Parameters[i-1]);
            }

            return body;
        }

        // Returns a string save representation
        public string ComposeSaveDirective()
        {
            string parameters = "";
            Parameters.ForEach(x => parameters = parameters + " " + x);
            return "#" + Name + parameters;
        }
    }
}
