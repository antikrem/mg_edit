using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit
{
    // Base class used to represent a Component
    abstract class Component
    {
        // Look up for correct string handler
        private Dictionary<string, (int, Delegate)> lookup = new Dictionary<string, (int, Delegate)>();

        // Adds function with no specific number of frames
        protected void AddFunction(string name, Action<string[], Entity> function)
        {
            lookup[name] = (-1, function);
        }

        // Adds function with specific number of frames
        protected void AddFunction(string name, Action<string[], Entity> function, int parameters)
        {
            lookup[name] = (parameters, function);
        }

        // Initialises entity
        virtual public void Initialise(string[] parameters, Entity ent)
        {
            // Does nothing unless override
        }

        // Updates given entity by invoking correct lookup
        public void UpdateEntity(string line, Entity ent)
        {
            line = line.Substring(1);
            string executer = line.Substring(1, line.IndexOf('(') - 1);

            string parametersCombined = line.Substring(line.IndexOf('(') + 1, line.Length - 2 - line.IndexOf('('));
            string[] parameters = parametersCombined.Split(',');

            // Check for correct length of parameters
            if (lookup[executer].Item1 < 0 || lookup[executer].Item1 == parameters.Length)
            {
                lookup[executer].Item2.DynamicInvoke(parameters, ent);
            }
            
        }

    }
}
