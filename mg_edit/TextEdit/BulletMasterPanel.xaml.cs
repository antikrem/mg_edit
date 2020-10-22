using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using mg_edit.Loader;

namespace mg_edit.TextEdit
{
    /// <summary>
    /// Interaction logic for BulletMasterPanel.xaml
    /// </summary>
    public partial class BulletMasterPanel : UserControl
    {


        public BulletMasterPanel(EntityDefinition entityDefinition)
        {
            InitializeComponent();

            this.MasterNameTextBox.Text = ((ComponentBulletMaster)entityDefinition.Components["+bulletMaster"]).Name;

            this.TimingsTextBox.Text = ((ComponentBulletMaster)entityDefinition.Components["+bulletMaster"]).Delay.ToString();
        }
    }
}
