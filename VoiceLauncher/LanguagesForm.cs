using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class LanguagesForm : Form
    {
        private VoiceLauncherContext db;
        public LanguagesForm()
        {
            InitializeComponent();
            db = new VoiceLauncherContext();
        }

        private void Languages_Load(object sender, EventArgs e)
        {
            languageDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            db.Languages.OrderBy(v => v.LanguageName).Load();
            languageDataGridView.DataSource = db.Languages.Local.ToBindingList();
            languageDataGridView.Refresh();
        }

        private void languageBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            languageBindingSource.EndEdit();
            db.SaveChanges();
            this.languageDataGridView.Refresh();
            this.Text = $"Saved Successfully at {DateTime.Now.ToShortTimeString()}";

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (languageBindingSource.Current != null)
            {
                var current = (Language)languageBindingSource.Current;
                languageBindingSource.RemoveCurrent();
                db.Languages.Local.Remove(current);
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            languageDataGridView.ClearSelection();
            int rowIndex = languageDataGridView.Rows.Count - 1;
            languageDataGridView.Select();
            languageBindingSource.MoveLast();
            languageDataGridView.Rows[rowIndex].Cells[1].Selected = true;
        }
    }
}
