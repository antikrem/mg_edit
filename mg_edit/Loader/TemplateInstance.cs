using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    public class TemplateInstance
    {
        // Reference to template
        public Template Template { get; }

        // String of parameters
        public List<string> Parameters { set; get; }

        // Construct with name
        public TemplateInstance(Template template)
        {
            this.Template = template;
        }

        // Construct with name and parameters
        public TemplateInstance(Template template, List<string> parameters)
        {
            this.Template = template;
            this.Parameters = parameters;
        }

        // Set parameter
        public void SetParameter(int position, string parameter)
        {
            this.Parameters[position] = parameter;
        }

        // Return substituted template
        public string GetSubbedTemplate()
        {
            string body = Template.Contents;

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
            return "#" + Template.Name + parameters;
        }
    }
}
