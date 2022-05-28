using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceLauncher
{
    public partial class SearchCustomIS : Form
    {
        public string SearchTerm { get; set; }
        public SearchCustomIS()
        {
            InitializeComponent();
        }

        private void customIntelliSenseBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.customIntelliSenseBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.voiceLauncherDataSet);

        }

        private void SearchCustomIS_Load(object sender, EventArgs e)
        {

            // TODO: This line of code loads data into the 'voiceLauncherDataSet.CustomIntelliSense' table. You can move, or remove it, as needed.
            if (!string .IsNullOrEmpty(SearchTerm))
            {
                customIntelliSenseBindingSource1.Filter = $"[Display_Value] like '%{SearchTerm}%'";
            }
            this.customIntelliSenseTableAdapter.Fill(this.voiceLauncherDataSet.CustomIntelliSense);
            //// TODO: This line of code loads data into the 'voiceLauncherDataSet.CustomIntelliSense' table. You can move, or remove it, as needed.
            //this.customIntelliSenseTableAdapter.Fill(this.voiceLauncherDataSet.CustomIntelliSense);

        }

        private void customIntelliSenseBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.customIntelliSenseBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.voiceLauncherDataSet);

        }

        private void fillByDisplayValueToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.customIntelliSenseTableAdapter.FillByDisplayValue(this.voiceLauncherDataSet.CustomIntelliSense);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
