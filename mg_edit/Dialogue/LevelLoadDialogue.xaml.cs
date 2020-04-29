using System;
using System.IO;
using System.Windows;
using mg_edit.Helper;

namespace mg_edit.Dialogue
{
    /// <summary>
    /// Interaction logic for LevelLoadDialogue.xaml
    /// </summary>
    public partial class LevelLoadDialogue : Window
    {
        // Set to not null when a valid value is set
        public string Path { get; set; } = null;

        public LevelLoadDialogue()
        {
            InitializeComponent();

            // Load default level
            using (INIParser settings = new INIParser("settings.ini"))
            {
                //CampaignTextBox.Text;
                CampaignTextBox.Text = settings.Get("level_default", "campaign", "");
                LevelTextBox.Text = settings.Get("level_default", "level", "");
            }

        }

        public void HandleLevelLoad(object sender, RoutedEventArgs e)
        {
            // Check for correct level input
            int level;
            if (!Int32.TryParse(LevelTextBox.Text, out level) || level < 0)
            {
                MessageBox.Show("Invalid level (must be positive integer)", "Error");
                return;
            }

            // Check path is valid
            string trialPath = "campaigns/" + CampaignTextBox.Text + "/" + level.ToString() + "/";

            if (!Directory.Exists(trialPath))
            {
                MessageBox.Show("Invalid level, level folder cannot be found at: " + trialPath, "Error");
                return;
            }

            Path = trialPath;

            using (INIParser settings = new INIParser("settings.ini"))
            {
                settings.Set("level_default", "campaign", CampaignTextBox.Text);
                settings.Set("level_default", "level", level.ToString());
            }

            this.Close();
        }

    }
}
