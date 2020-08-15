using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mg_edit.TextEdit.TemplatePanelParameter;

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
            this.Parameters = new List<string>();

            foreach (string parameter in Template.ParameterTypes)
            {
                ITemplateParameter panel = TemplatePanel.CreateTemplateParameterPanel(parameter);
                Parameters.AddRange(panel.GetDefaultParameters());
            }
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

        // Get parameter
        public string GetParameter(int position)
        {
            return Parameters[position];
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
