using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    class Script : Loadable
    {
        public string ScriptBody { get; set; }

        public void Extend(string text)
        {
            ScriptBody = ScriptBody + text + "\n";
        }

        public Script(List<int> SpawningCycles)
        {
            this.SpawningCycles = SpawningCycles;
        }
    }
}
