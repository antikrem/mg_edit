﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using mg_edit.Loader.Components;
using mg_edit.TextEdit;

namespace mg_edit.Loader
{
    class ComponentBulletMaster : Component, InstanceableComponent
    {
        // Name of bullet master
        public string Name { get; set; } = "";

        // Name of bullet master
        public int Delay { get; set; } = 0;

        // Additional parameters
        public string[] AdditionalParameters { get; set; } = null;

        // Sets position
        override public void Initialise(string[] parameters, EntityDefinition entDef)
        {
            Name = parameters[0];
            if (parameters.Length > 1)
            {
                Delay = Int32.Parse(parameters[1]);
            }
            if (parameters.Length > 2)
            {
                AdditionalParameters = parameters.Skip(2).ToArray();
            }
        }

        public ComponentBulletMaster()
        {

        }

        public override string ComposeSaveDirective(EntityDefinition entDef)
        {
            string body = "+bulletMaster " + Name;

            body = body + (Delay != 0 ? " " + Delay.ToString() : "");

            if (!(AdditionalParameters is null))
            {
                body = body + " " + string.Join(" ", AdditionalParameters);
            }
            return body;
        }

        public UserControl GetPanel(EntityDefinition entDef)
        {
            return new BulletMasterPanel(entDef);
        }
    }
}
