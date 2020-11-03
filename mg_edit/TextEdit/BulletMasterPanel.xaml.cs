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

        EntityDefinition entityDefinition;

        ComponentBulletMaster bulletMaster;

        public void UpdateName(object sender, RoutedEventArgs e)
        {
            bulletMaster.Name = MasterNameTextBox.Text;
        }

        public void UpdateTimings(object sender, RoutedEventArgs e)
        {
            int cycle = 0;

            int.TryParse(TimingsTextBox.Text, out cycle);

            bulletMaster.Delay = cycle;
        }

        public void UpdateAdditionalParameters(object sender, RoutedEventArgs e)
        {
            if (ParametersTextBox.Text == null)
            {
                bulletMaster.AdditionalParameters = null;
            }
            else
            {
                bulletMaster.AdditionalParameters = ParametersTextBox.Text.Split(',').Select(parameter => parameter.Trim()).ToArray();
            }
        }

        public BulletMasterPanel(EntityDefinition entityDefinition)
        {
            InitializeComponent();

            this.entityDefinition = entityDefinition;
            this.bulletMaster = (ComponentBulletMaster)this.entityDefinition.Components["+bulletMaster"];

            this.MasterNameTextBox.Text = this.bulletMaster.Name;
            this.TimingsTextBox.Text = this.bulletMaster.Delay.ToString();
            if (!(this.bulletMaster.AdditionalParameters is null))
            {
                this.ParametersTextBox.Text = String.Join(", ", this.bulletMaster.AdditionalParameters);
            }
            
            this.MasterNameTextBox.TextChanged += UpdateName;
            this.TimingsTextBox.TextChanged += UpdateTimings;
            this.ParametersTextBox.TextChanged += UpdateAdditionalParameters;

        }
    }
}
