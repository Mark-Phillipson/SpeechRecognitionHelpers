using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class customIntelliSense : Form
    {
        private VoiceLauncherContext db;
        private int categoryId = 39;
        private int languageId = 1;
        public customIntelliSense()
        {
            InitializeComponent();
            db = new VoiceLauncherContext();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string[] arguments;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() < 2)
            {
                arguments = new string[] { args[0], "Visual Basic", "Statements" };
                //arguments = new string[] { args[0], "Not Applicable", "Words" };
            }
            else
            {
                arguments = Environment.GetCommandLineArgs();
            }
            string languageName = arguments[1].Replace("/", "").Trim();

            Language language = db.Languages.Where(v => v.LanguageName == languageName).FirstOrDefault();
            if (language == null)
            {
                throw (new Exception($" Language not found in commandline argument {arguments[1]}"));
            }
            languageId = language.ID;
            string categoryName = arguments[2].Replace("/", "").Trim();
            Category category = db.Categories.Where(v => v.CategoryName == categoryName).FirstOrDefault();
            if (category == null)
            {
                throw (new Exception($" the Category not found in commandline argument {arguments[2]}"));
            }
            categoryId = category.ID;
            this.Text = $"Custom IntelliSense {language.LanguageName} {category.CategoryName}";
            SetDataSourceForGrid();
            db.Configuration.ProxyCreationEnabled = false;
            customIntelliSenseDataGridView.Columns[2].HeaderText = "Category";
            customIntelliSenseDataGridView.Columns[3].HeaderText = "Display Value";
            customIntelliSenseDataGridView.Columns[4].HeaderText = "Value to Send or Code to Run";
            customIntelliSenseDataGridView.Columns[5].Visible = false;
            customIntelliSenseDataGridView.Columns[7].Visible = false;
            customIntelliSenseDataGridView.Columns[9].HeaderText = "Delivery Type";
        }

        private void SetDataSourceForGrid(bool showAll=false)
        {
            if (showAll)
            {
                db.CustomIntelliSenses.OrderBy(v => v.Language.LanguageName).ThenBy(v => v.Category.CategoryName).ThenBy(o => o.Display_Value).Load();
            }
            else
            {
                db.CustomIntelliSenses.Where(v => v.Category.ID == categoryId && v.Language.ID == languageId).OrderBy(o => o.Display_Value).Load();
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
            MessageBox.Show("Save Successful", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            cboBoxColumn = (DataGridViewComboBoxColumn)customIntelliSenseDataGridView.Columns["dataGridViewTextBoxColumn6"];
            db.Categories.Where(v => v.CategoryType == "IntelliSense Command").OrderBy(o => o.CategoryName).Load();
            cboBoxColumn.DataSource = db.Categories.Local.ToBindingList();
            cboBoxColumn.DisplayMember = "CategoryName";
            cboBoxColumn.ValueMember = "ID";
            cboBoxColumn = (DataGridViewComboBoxColumn)customIntelliSenseDataGridView.Columns["dataGridViewTextBoxColumnDeliveryType"];
            cboBoxColumn.Items.Add("Copy and Paste");
            cboBoxColumn.Items.Add("Send Keys");
            cboBoxColumn.Items.Add("Executed as Script");
            cboBoxColumn.Items.Add("Clipboard Only");

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
            this.customIntelliSenseDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

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
            customIntelliSenseDataGridView.Rows[rowIndex].Selected = true;
            customIntelliSenseDataGridView.Rows[rowIndex].Cells[0].Selected = true;

        }

        private void customIntelliSenseDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["dataGridViewTextBoxColumn1"].Value = languageId;
            e.Row.Cells["dataGridViewTextBoxColumn6"].Value = categoryId;
            e.Row.Cells["dataGridViewTextBoxColumnDeliveryType"].Value = "Send Keys";

        }
        private void toolStripTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<CustomIntelliSense> filteredData = null;
            if (string.IsNullOrEmpty(this.toolStripTextBoxSearch.Text))
            {
                this.bindingNavigatorDeleteItem.Enabled = true;
                filteredData = db.CustomIntelliSenses.Local.ToBindingList();
            }
            else if (toolStripTextBoxSearch.Text.Length > 4)
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

        private void toolStripButtonShowAll_Click(object sender, EventArgs e)
        {
            Text = "Custom IntelliSense — All Records";
            SetDataSourceForGrid(true);
        }
    }
}
