using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mg_edit.TextEdit;

using mg_edit.Helper;

namespace mg_edit.Loader
{
    // Interface for objects that can be in a load table
    abstract public class Loadable
    {
        // An associated double referenced LoadablePanel
        ILoadablePanel LoadPanel = null;

        // Internal list of spawning cycles
        private List<int> _spawningCycles = null;

        // List of cycles
        public List<int> SpawningCycles
        {
            get { return _spawningCycles; }
            set { _spawningCycles = new List<int>(value); }
        }

        // Abstract funtion that generates a new LoadPanel
        protected abstract ILoadablePanel GenerateLoadPanel();

        // Function that converts a Loadable into a LoadablePanel
        public ILoadablePanel GetLoadablePanel()
        {
            if (LoadPanel is null)
            {
                LoadPanel = this.GenerateLoadPanel();
            }

            return LoadPanel;
        }

        // Returns smallest spawning cycle or zero
        public int GetLowestSpawnCycle()
        {
            try
            {
                return SpawningCycles.Min();
            }
            catch
            {
                return 0;
            }
        }

        // Returns string representation for saving
        public abstract string ConstructSaveDirective();
    }
}
