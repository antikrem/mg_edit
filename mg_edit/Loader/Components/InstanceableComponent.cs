using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;

namespace mg_edit.Loader.Components
{
    interface InstanceableComponent
    {
        UserControl GetPanel(EntityDefinition entDef);
    }
}
