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

        // List of parameters
        public List<string> ParameterNames { get; }

        public Template(string name, string contents)
        {
            Name = name;
            Contents = contents;
        }

        public Template(string name, string contents, List<string> parameterNames)
        {
            Name = name;
            Contents = contents;
            ParameterNames = parameterNames;
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
