using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mg_edit.TextEdit;

namespace mg_edit.Loader
{
    public class Script : Loadable
    {
        public string ScriptBody { get; set; }

        public void Extend(string text)
        {
            ScriptBody = ScriptBody + text + "\n";
        }

        protected override ILoadablePanel GenerateLoadPanel()
        {
            return new ScriptLoadPanel(this);
        }

        public Script(List<int> SpawningCycles)
        {
            this.SpawningCycles = SpawningCycles;
        }

        public override string ConstructSaveDirective()
        {
            string body = "";
            List<string> scriptBody = ScriptBody.Trim().Split('\n').ToList();
            scriptBody.ForEach(x => body = body + "<<" + x + "\n");
            return body;
        }
    }
}
