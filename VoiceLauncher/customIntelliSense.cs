using SpeechRecognitionHelpersLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class CustomIntelliSenseForm : Form
    {
        private VoiceLauncherContext db;
        bool formIsClosed = false;
        public int? LanguageId { get; internal set; } = 1;
        public int? CategoryId { get; internal set; } = 39;
        public string SearchTerm { get; set; }
        public CustomIntelliSenseForm()
        {
            InitializeComponent();
            db = new VoiceLauncherContext();
            BackColor = Color.FromArgb(100, 100, 100);
            ForeColor = Color.White;

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            menuStrip1.BackColor = Color.FromArgb(38, 38, 38);
            menuStrip1.ForeColor = Color.White;
            menuStrip1.Renderer = new MyRenderer();
            db.Configuration.ProxyCreationEnabled = false;
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            CustomTheme.SetDataGridViewTheme(customIntelliSenseDataGridView, "Tahoma");
            SetDataSourceForGrid();
            customIntelliSenseDataGridView.Columns[2].HeaderText = "Category";
            customIntelliSenseDataGridView.Columns[3].HeaderText = "Display Value";
            customIntelliSenseDataGridView.Columns[4].HeaderText = "Value to Send or Code to Run";
            customIntelliSenseDataGridView.Columns[5].Visible = false;
            customIntelliSenseDataGridView.Columns[7].Visible = false;
            customIntelliSenseDataGridView.Columns[9].HeaderText = "Delivery Type";
            foreach (DataGridViewColumn column in customIntelliSenseDataGridView.Columns)
            {
                column.DefaultCellStyle.Font = new Font("Cascadia Code", 10.5F, GraphicsUnit.Pixel);
                if (column.Name.Contains("4"))
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }

        private void SetDataSourceForGrid(bool showAll = false)
        {
            db.CustomIntelliSenses.OrderBy(v => v.Language.LanguageName).ThenBy(v => v.Category.CategoryName).ThenBy(o => o.Display_Value).Load();
            IEnumerable<CustomIntelliSense> filteredData;
            if (SearchTerm != null)
            {
                filteredData = db.CustomIntelliSenses.Local.ToBindingList()
                        .Where(v => v.Display_Value.ToLower().Contains(SearchTerm.ToLower()));
                this.Text = $"Custom IntelliSense Filter: {SearchTerm}";
            }
            else
            {
                this.Text = $"Custom IntelliSense Filter: No Filter Applied";
                filteredData = db.CustomIntelliSenses.Local.ToBindingList()
                .Where(v => v.Category.ID == CategoryId && v.Language.ID == LanguageId).OrderBy(o => o.Display_Value);
            }
            this.customIntelliSenseBindingSource.DataSource = filteredData.Count() > 0 ? filteredData : filteredData.ToArray();

            customIntelliSenseDataGridView.Refresh();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            formIsClosed = true;
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
                MessageBox.Show(anError.Exception.Message);
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
            cboBoxColumn.DataSource = db.Languages.Local.Where(v => v.Active).ToList();
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
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.MinimumWidth = 100;
                }
                else
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            this.customIntelliSenseDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //I have turned this off because it sometimes causes the screen to freeze up but would be nice to have if it worked
            //this.customIntelliSenseDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (customIntelliSenseBindingSource.Current != null)
            {
                var current = (CustomIntelliSense)this.customIntelliSenseBindingSource.Current;
                this.customIntelliSenseBindingSource.RemoveCurrent();
                if (!string.IsNullOrEmpty(this.toolStripTextBoxSearch.Text))
                {
                    db.CustomIntelliSenses.Local.Remove(current);
                }
                db.CustomIntelliSenses.Remove(current);
                db.SaveChanges();
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            CustomIntelliSenseSingleRecord customIntelliSenseSingleRecord = new CustomIntelliSenseSingleRecord();
            customIntelliSenseSingleRecord.CurrentId = (int)0;
            var clipboard = Clipboard.GetText();
            customIntelliSenseSingleRecord.DefaultValueToSend = clipboard;
            var languageId = (int)customIntelliSenseDataGridView.CurrentRow.Cells[1].Value;
            customIntelliSenseSingleRecord.LanguageId = languageId;
            var categoryId = (int)customIntelliSenseDataGridView.CurrentRow.Cells[2].Value;
            customIntelliSenseSingleRecord.CategoryId = categoryId;
            customIntelliSenseSingleRecord.ShowDialog();

        }

        public DataGridViewRow CloneWithValues(DataGridViewRow row)
        {
            DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
            for (Int32 index = 0; index < row.Cells.Count; index++)
            {
                clonedRow.Cells[index].Value = row.Cells[index].Value;
            }
            return clonedRow;
        }


        private void customIntelliSenseDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            //e.Row.Cells["dataGridViewTextBoxColumn1"].Value = LanguageId;
            //e.Row.Cells["dataGridViewTextBoxColumn6"].Value = CategoryId;
            //e.Row.Cells["dataGridViewTextBoxColumnDeliveryType"].Value = "Send Keys";

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
            if (formIsClosed)
            {
                return;
            }
            IEnumerable<CustomIntelliSense> filteredData = null;
            if (string.IsNullOrEmpty(this.toolStripTextBoxSearch.Text))
            {
                filteredData = db.CustomIntelliSenses.Local.ToBindingList();
            }
            else if (toolStripTextBoxSearch.Text.Length > 0)
            {
                filteredData = db.CustomIntelliSenses.Local.ToBindingList().Where(v => v.Display_Value.Contains(this.toolStripTextBoxSearch.Text) || v.SendKeys_Value.Contains(this.toolStripTextBoxSearch.Text) || v.Category.CategoryName.Contains(this.toolStripTextBoxSearch.Text) || v.Language.LanguageName.Contains(this.toolStripTextBoxSearch.Text));
            }
            if (filteredData == null)
            {
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
            if (customIntelliSenseDataGridView.CurrentRow == null || customIntelliSenseDataGridView.CurrentRow.Cells == null || customIntelliSenseDataGridView.CurrentRow.Cells[2].Value == null)
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

        private void toolStripButtonDuplicate_Click(object sender, EventArgs e)
        {
            int rowIndex = customIntelliSenseDataGridView.Rows.Count - 1;
            if (customIntelliSenseBindingSource.Current != null)
            {
                //customIntelliSenseBindingSource.MoveFirst();
                //customIntelliSenseBindingSource.MoveLast();
                var current = (CustomIntelliSense)this.customIntelliSenseBindingSource.Current;
                CustomIntelliSense customIntelliSenseNew = new CustomIntelliSense
                {
                    CategoryID = current.CategoryID,
                    Command_Type = current.Command_Type,
                    ComputerID = current.ComputerID,
                    DeliveryType = current.DeliveryType,
                    Display_Value = "TBC!",
                    LanguageID = current.LanguageID,
                    Remarks = current.Remarks,
                    SendKeys_Value = current.SendKeys_Value
                };

                db.CustomIntelliSenses.Local.Add(customIntelliSenseNew);
                try
                {
                    //db.SaveChanges();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                IEnumerable<CustomIntelliSense> filteredData = null;
                filteredData = db.CustomIntelliSenses.Local.ToBindingList();
                this.customIntelliSenseBindingSource.DataSource = filteredData.Count() > 0 ? filteredData : filteredData.ToArray();
            }
            else
            {
                customIntelliSenseDataGridView.ClearSelection();
                customIntelliSenseDataGridView.Rows[rowIndex].Cells[1].Selected = true;

            }

        }

        private void CustomIntelliSenseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            db.Dispose();
        }

        private void toolStripButtonRemoveFilter_Click(object sender, EventArgs e)
        {
            customIntelliSenseBindingSource.DataSource = db.CustomIntelliSenses.Local.ToBindingList();
            foreach (DataGridViewColumn column in customIntelliSenseDataGridView.Columns)
            {
                if (column.Name != "dataGridViewTextBoxColumn4")
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.MinimumWidth = 100;
                }
                else
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }

        }
    }
}
