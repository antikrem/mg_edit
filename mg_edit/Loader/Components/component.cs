﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mg_edit.Loader;
using mg_edit.Movement;

namespace mg_edit.Loader
{
    // Base class used to represent a Component
    public abstract class Component
    {
        // Look up for correct string handler
        private Dictionary<string, (int, Delegate)> lookup = new Dictionary<string, (int, Delegate)>();

        // Adds function with no specific number of frames
        protected void AddFunction(string name, Action<string[], EntityDefinition> function)
        {
            lookup[name] = (-1, function);
        }

        // Adds function with specific number of frames
        protected void AddFunction(string name, Action<string[], EntityDefinition> function, int parameters)
        {
            lookup[name] = (parameters, function);
        }

        // Initialises entity
        virtual public void Initialise(string[] parameters, EntityDefinition entDef)
        {
            // Does nothing unless override
        }

        // Updates given entity by invoking correct lookup
        public void UpdateEntity(string line, EntityDefinition entityDefinition)
        {
            line = line.Substring(1);
            string executer = line.Substring(1, line.IndexOf('(') - 1);

            string parametersCombined = line.Substring(line.IndexOf('(') + 1, line.Length - 2 - line.IndexOf('('));
            string[] parameters = parametersCombined.Split(',');

            // Check for correct length of parameters
            if (lookup[executer].Item1 < 0 || lookup[executer].Item1 == parameters.Length)
            {
                lookup[executer].Item2.DynamicInvoke(parameters, entityDefinition);
            }
            
        }

        // Translates a component name to correct Component type
        static public Component TranslateToComponentType(string name)
        {
            switch (name)
            {
                case "position":
                    return new ComponentPosition();

                case "movement":
                    return new ComponentMovement();

                case "staticMovement":
                    return new ComponentStaticMovement();

                case "bulletMaster":
                    return new ComponentBulletMaster();

                default:
                    return null;
            }
        }

        // Returns string representation of this component for saving
        abstract public string ComposeSaveDirective(EntityDefinition entDef);
    }
}
