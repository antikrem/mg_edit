using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    public class Template
    {
        // Name of template
        public string Name { get; }

        // Content of unsubbed template
        public string Contents { get; }

        // List of parameter names
        public List<string> ParameterNames { get; }

        // List of parameter types
        public List<string> ParameterTypes { get; }

        public Template(string name, string contents)
        {
            Name = name;
            Contents = contents;
        }

        public Template(string name, string contents, List<string> parameters)
        {
            Name = name;
            Contents = contents;
            
            ParameterNames = new List<string>();
            ParameterTypes = new List<string>();

            foreach (var parameter in parameters)
            {
                ParameterTypes.Add(parameter.Split(':')[0]);
                ParameterNames.Add(parameter.Split(':')[1]);
            }
        }

        // Returns an expansion of this Template given a saved line
        public string ExpandTemplate(string line)
        {
            var vec = line.Split(' ');

            string body = this.Contents;

            for (int i = 1; i < vec.Length; i++)
            {
                body = body.Replace("%" + i.ToString(), vec[i]);
            }
            body = body + line + "\n";

            return body;
        }
    }
}
