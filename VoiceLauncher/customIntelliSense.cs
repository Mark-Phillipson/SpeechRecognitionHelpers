using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class CustomIntelliSenseForm : Form
    {
        private VoiceLauncherContext db;
        public int? LanguageId { get; internal set; } = 1;
        public int? CategoryId { get; internal set; } = 39;
        public string SearchTerm { get; set; }
        public CustomIntelliSenseForm()
        {
            InitializeComponent();
            db = new VoiceLauncherContext();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            menuStrip1.BackColor = Color.Black;
            menuStrip1.ForeColor = Color.White;
            customIntelliSenseDataGridView.EnableHeadersVisualStyles = false;
            SetDataSourceForGrid();
            db.Configuration.ProxyCreationEnabled = false;
            customIntelliSenseDataGridView.Columns[2].HeaderText = "Category";
            customIntelliSenseDataGridView.Columns[3].HeaderText = "Display Value";
            customIntelliSenseDataGridView.Columns[4].HeaderText = "Value to Send or Code to Run";
            customIntelliSenseDataGridView.Columns[5].Visible = false;
            customIntelliSenseDataGridView.Columns[7].Visible = false;
            customIntelliSenseDataGridView.Columns[9].HeaderText = "Delivery Type";
        }

        private void SetDataSourceForGrid(bool showAll = false)
        {
            if (showAll)
            {
                db.CustomIntelliSenses.OrderBy(v => v.Language.LanguageName).ThenBy(v => v.Category.CategoryName).ThenBy(o => o.Display_Value).Load();
            }
            else if (SearchTerm != null)
            {
                db.CustomIntelliSenses.Where(v => v.Display_Value.ToLower().Contains(SearchTerm.ToLower())).Load();
            }
            else
            {
                db.CustomIntelliSenses.Where(v => v.Category.ID == CategoryId && v.Language.ID == LanguageId).OrderBy(o => o.Display_Value).Load();
            }
            this.customIntelliSenseBindingSource.DataSource = db.CustomIntelliSenses.Local
                .ToBindingList();

            customIntelliSenseDataGridView.Refresh();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.db.Dispose();
        }

        private void customIntelliSenseBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            customIntelliSenseBindingSource.EndEdit();
            db.SaveChanges();
            this.customIntelliSenseDataGridView.Refresh();
            this.Text = $"Saved Successfully at {DateTime.Now.ToShortTimeString()}";
        }

        private void customIntelliSenseDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            //MessageBox.Show("Error happened " + anError.Context.ToString());

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }


        private void customIntelliSense_Load(object sender, EventArgs e)
        {

            this.customIntelliSenseDataGridView.Columns[0].Visible = false;
            this.customIntelliSenseDataGridView.Columns[7].Visible = false;
            this.customIntelliSenseDataGridView.Columns[8].Visible = false;
            this.customIntelliSenseDataGridView.Columns[10].Visible = false;
            this.customIntelliSenseDataGridView.Columns[11].Visible = false;
            this.customIntelliSenseDataGridView.Columns[12].Visible = false;
            // reference the combobox column
            DataGridViewComboBoxColumn cboBoxColumn = (DataGridViewComboBoxColumn)customIntelliSenseDataGridView.Columns[1];
            db.Languages.OrderBy(o => o.LanguageName).Load();
            cboBoxColumn.DataSource = db.Languages.Local.ToBindingList();
            cboBoxColumn.DisplayMember = "LanguageName";  // the Name property in Choice class
            cboBoxColumn.ValueMember = "ID";  // ditto for the Value property        }
            var style = new DataGridViewCellStyle() { BackColor = Color.Black, ForeColor = Color.White, SelectionBackColor = Color.Black, SelectionForeColor = Color.Red };
            cboBoxColumn.DisplayStyleForCurrentCellOnly = false;
            cboBoxColumn.DefaultCellStyle = style;
            cboBoxColumn.FlatStyle = FlatStyle.Popup;
            cboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            cboBoxColumn = (DataGridViewComboBoxColumn)customIntelliSenseDataGridView.Columns["dataGridViewTextBoxColumn6"];
            db.Categories.Where(v => v.CategoryType == "IntelliSense Command").OrderBy(o => o.CategoryName).Load();
            cboBoxColumn.DataSource = db.Categories.Local.ToBindingList();
            cboBoxColumn.DisplayMember = "CategoryName";
            cboBoxColumn.ValueMember = "ID";
            cboBoxColumn.DisplayStyleForCurrentCellOnly = false;
            cboBoxColumn.DefaultCellStyle = style;
            cboBoxColumn.FlatStyle = FlatStyle.Popup;
            cboBoxColumn = (DataGridViewComboBoxColumn)customIntelliSenseDataGridView.Columns["dataGridViewTextBoxColumnDeliveryType"];
            cboBoxColumn.Items.Add("Copy and Paste");
            cboBoxColumn.Items.Add("Send Keys");
            cboBoxColumn.Items.Add("Executed as Script");
            cboBoxColumn.Items.Add("Clipboard Only");
            cboBoxColumn.DisplayStyleForCurrentCellOnly = false;
            cboBoxColumn.DefaultCellStyle = style;
            cboBoxColumn.FlatStyle = FlatStyle.Popup;
            var textBox = (DataGridViewTextBoxColumn)customIntelliSenseDataGridView.Columns["dataGridViewTextBoxColumn4"];

            foreach (DataGridViewColumn column in customIntelliSenseDataGridView.Columns)
            {
                if (column.Name != "dataGridViewTextBoxColumn4")
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            this.customIntelliSenseDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //I have turned this off because it sometimes causes the screen to freeze up but would be nice to have if it worked
            //this.customIntelliSenseDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }


        private void customIntelliSense_Activated(object sender, EventArgs e)
        {
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (customIntelliSenseBindingSource.Current != null)
            {
                var current = (CustomIntelliSense)this.customIntelliSenseBindingSource.Current;
                this.customIntelliSenseBindingSource.RemoveCurrent();
                if (string.IsNullOrEmpty(this.toolStripTextBoxSearch.Text))
                {
                    db.CustomIntelliSenses.Local.Remove(current);
                }
                else
                {
                    db.CustomIntelliSenses.Remove(current);
                }
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            customIntelliSenseDataGridView.ClearSelection();
            int rowIndex = customIntelliSenseDataGridView.Rows.Count - 1;
            //customIntelliSenseDataGridView.Rows[rowIndex].Selected = true;
            customIntelliSenseDataGridView.Rows[rowIndex].Cells[1].Selected = true;

        }

        private void customIntelliSenseDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["dataGridViewTextBoxColumn1"].Value = LanguageId;
            e.Row.Cells["dataGridViewTextBoxColumn6"].Value = CategoryId;
            e.Row.Cells["dataGridViewTextBoxColumnDeliveryType"].Value = "Send Keys";

        }
        private void toolStripTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
        }

        private void toolStripButtonShowAll_Click(object sender, EventArgs e)
        {
            Text = "Custom IntelliSense — All Records";
            SetDataSourceForGrid(true);
        }

        private void toolStripTextBoxFind_TextChanged(object sender, EventArgs e)
        {
        }

        private void Focus_Click(object sender, EventArgs e)
        {
            customIntelliSenseDataGridView.BeginEdit(true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Categories categoriesForm = new Categories();
            categoriesForm.ShowDialog();
        }

        private void toolStripTextBoxSearch_Leave(object sender, EventArgs e)
        {
            IEnumerable<CustomIntelliSense> filteredData = null;
            if (string.IsNullOrEmpty(this.toolStripTextBoxSearch.Text))
            {
                this.bindingNavigatorDeleteItem.Enabled = true;
                filteredData = db.CustomIntelliSenses.Local.ToBindingList();
            }
            else if (toolStripTextBoxSearch.Text.Length > 0)
            {
                this.bindingNavigatorDeleteItem.Enabled = false;
                filteredData = db.CustomIntelliSenses.Local.ToBindingList().Where(v => v.Display_Value.Contains(this.toolStripTextBoxSearch.Text) || v.SendKeys_Value.Contains(this.toolStripTextBoxSearch.Text) || v.Category.CategoryName.Contains(this.toolStripTextBoxSearch.Text) || v.Language.LanguageName.Contains(this.toolStripTextBoxSearch.Text));
            }
            if (filteredData == null)
            {
                this.bindingNavigatorDeleteItem.Enabled = true;
                return;
            }
            this.customIntelliSenseBindingSource.DataSource = filteredData.Count() > 0 ? filteredData : filteredData.ToArray();


        }

        private void toolStripTextBoxFind_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.toolStripTextBoxFind.Text) || this.toolStripTextBoxFind.Text.Length < 1)
            {
                return;
            }
            string searchValue = this.toolStripTextBoxFind.Text;
            int rowIndex = -1;
            bool firstOne = true;
            int firstRow = -1;
            int firstColumn = -1;
            foreach (DataGridViewRow row in customIntelliSenseDataGridView.Rows)
            {
                for (int i = 3; i < 5; i++)
                {
                    if (row.Cells[i].Value != null && row.Cells[i].Value.ToString().Contains(searchValue))
                    {
                        rowIndex = row.Index;
                        customIntelliSenseDataGridView.Rows[rowIndex].Selected = true;
                        if (firstOne)
                        {
                            firstRow = rowIndex;
                            firstColumn = i;
                            firstOne = false;
                        }
                    }
                }
            }
            if (firstOne == false)
            {
                customIntelliSenseDataGridView.CurrentCell = customIntelliSenseDataGridView.Rows[firstRow].Cells[firstColumn];
            }

        }

        private void singleCustomIntelliSenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomIntelliSenseSingleRecord customIntelliSenseSingleRecord = new CustomIntelliSenseSingleRecord();
            customIntelliSenseSingleRecord.CurrentId = (int)customIntelliSenseDataGridView.CurrentRow.Cells[0].Value;
            customIntelliSenseSingleRecord.ShowDialog();
        }

        private void languagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguagesForm languagesForm = new LanguagesForm();
            languagesForm.ShowDialog();
        }

        private void launchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LauncherForm launcherForm = new LauncherForm();
            launcherForm.ShowDialog();
        }

        private void customIntelliSenseDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (customIntelliSenseDataGridView.CurrentRow == null || customIntelliSenseDataGridView.CurrentRow.Cells == null)
            {
                return;
            }
            if (e.ColumnIndex == 4 && (int)customIntelliSenseDataGridView.CurrentRow.Cells[2].Value == 48)
            {
                if (e.Value != null)
                {
                    e.Value = new string('*', e.Value.ToString().Length);
                }
            }
        }


        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //((ComboBox)sender).BackColor = (Color)((ComboBox)sender).SelectedItem;
            //((ComboBox)sender).BackColor = Color.Black;
            //((ComboBox)sender).ForeColor = Color.White;
        }

        private void customIntelliSenseDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //ComboBox combo = e.Control as ComboBox;
            //if (combo != null)
            //{
            //    // Remove an existing event-handler, if present, to avoid 
            //    // adding multiple handlers when the editing control is reused.
            //    combo.SelectedIndexChanged -=
            //        new EventHandler(ComboBox_SelectedIndexChanged);

            //    // Add the event handler. 
            //    combo.SelectedIndexChanged +=
            //        new EventHandler(ComboBox_SelectedIndexChanged);
            //}

        }
    }
}
