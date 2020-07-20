using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class LanguagesForm : Form
    {
        private VoiceLauncherContext db;
        bool formIsClosed = false;
        public LanguagesForm()
        {
            InitializeComponent();
            db = new VoiceLauncherContext();
        }

        public void LanguagesForm_Load(object sender, EventArgs e)
        {
            db.Languages.Include(i => i.CustomIntelliSenses).Include(i => i.CustomIntelliSenses).OrderBy(v => v.LanguageName).Load();
            this.languageBindingSource.DataSource = db.Languages.Local.ToBindingList();
            languageDataGridView.Refresh();
            customIntelliSensesDataGridView.Refresh();
            CustomTheme.SetDataGridViewTheme(languageDataGridView, "Tahoma", 9);
            CustomTheme.SetDataGridViewTheme(customIntelliSensesDataGridView, "Tahoma", 9);
            BackColor = Color.FromArgb(100, 100, 100);
            ForeColor = Color.White;
            languageBindingNavigator.BackColor = Color.FromArgb(38, 38, 38);
            languageBindingNavigator.ForeColor = Color.White;
            foreach (DataGridViewColumn column in languageDataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            foreach (DataGridViewColumn column in customIntelliSensesDataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            DataGridViewComboBoxColumn cboBoxColumn = (DataGridViewComboBoxColumn)customIntelliSensesDataGridView.Columns[5];
            db.Categories.OrderBy(o => o.CategoryName).Load();
            cboBoxColumn.DataSource = db.Categories.Local.ToBindingList();
            cboBoxColumn.DisplayMember = "CategoryName";  // the Name property in Choice class
            cboBoxColumn.ValueMember = "ID";  // ditto for the Value property        }
            //customIntelliSensesDataGridView.Columns[0].HeaderText = "Language";
            customIntelliSensesDataGridView.Columns[2].HeaderText = "Display Value";
            customIntelliSensesDataGridView.Columns[3].HeaderText = "SendKeys Value";
            customIntelliSensesDataGridView.Columns[4].HeaderText = "Command Type";
            customIntelliSensesDataGridView.Columns[5].HeaderText = "Category";
            customIntelliSensesDataGridView.Columns[6].HeaderText = "Remarks";
            customIntelliSensesDataGridView.Columns[7].HeaderText = "Delivery Type";
            customIntelliSensesBindingSource.Sort = "CategoryID ASC, Display_Value ASC, SendKeys_Value ASC";

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            db.Dispose();
            formIsClosed = true;
            base.OnClosing(e);
        }
    }
}
