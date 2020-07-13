using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit.Loader
{
    // Interface for objects that can be in a load table
    class Loadable
    {
        // Internal list of spawning cycles
        private List<int> _spawningCycles = null;

        // List of cycles
        protected List<int> SpawningCycles
        {
            get { return _spawningCycles; }
            set { _spawningCycles = new List<int>(value); }
        }
    }
}
