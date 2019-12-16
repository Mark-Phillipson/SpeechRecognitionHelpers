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

        private void SetDataSourceForGrid()
        {
            //Language language = voiceLauncherContext.Languages.Where(v => v.ID == languageId).FirstOrDefault();
            //this.languageIDComboBox.SelectedItem = language;
            //Category category = voiceLauncherContext.Categories.Where(v => v.ID == categoryId).FirstOrDefault();
            //this.categoryIDComboBox.SelectedItem = category;
            db.CustomIntelliSenses.Where(v => v.Category.ID == categoryId && v.Language.ID == languageId).OrderBy(o => o.Display_Value).Load();
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

        private void customIntelliSenseDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
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
            //this.languageIDComboBox.DataSource = voiceLauncherContext.Languages.Local.ToBindingList();
            //this.languageIDComboBox.DisplayMember = "LanguageName";
            //this.languageIDComboBox.ValueMember = "ID";
            cboBoxColumn = (DataGridViewComboBoxColumn)customIntelliSenseDataGridView.Columns["dataGridViewTextBoxColumn6"];
            db.Categories.Where(v => v.CategoryType == "IntelliSense Command").OrderBy(o => o.CategoryName).Load();
            cboBoxColumn.DataSource = db.Categories.Local.ToBindingList();
            cboBoxColumn.DisplayMember = "CategoryName";
            cboBoxColumn.ValueMember = "ID";
            //this.categoryIDComboBox.DataSource = voiceLauncherContext.Categories.Local.ToBindingList();
            //this.categoryIDComboBox.DisplayMember = "CategoryName";
            //this.categoryIDComboBox.ValueMember = "ID";
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

        private void languageIDComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //ComboBox comboBox = (ComboBox)sender;
            //Language selectedLanguage = (Language)languageIDComboBox.SelectedItem;
            //languageId = selectedLanguage.ID;
            //SetDataSourceForGrid();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<CustomIntelliSense> filteredData = null;
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                filteredData = db.CustomIntelliSenses.Local.ToBindingList();
            }
            else if (textBox1.Text.Length > 4)
            {
                filteredData = db.CustomIntelliSenses.Local.ToBindingList().Where(v => v.Display_Value.Contains(this.textBox1.Text) || v.SendKeys_Value.Contains(this.textBox1.Text) || v.Category.CategoryName.Contains(this.textBox1.Text) || v.Language.LanguageName.Contains(this.textBox1.Text));
            }
            if (filteredData == null)
            {
                return;
            }
            this.customIntelliSenseBindingSource.DataSource = filteredData.Count() > 0 ? filteredData : filteredData.ToArray();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (customIntelliSenseBindingSource.Current != null)
            {
                var current = (CustomIntelliSense)this.customIntelliSenseBindingSource.Current;
                this.customIntelliSenseBindingSource.RemoveCurrent();
                if (!string.IsNullOrEmpty(this.textBox1.Text))
                {
                    db.CustomIntelliSenses.Local.Remove(current);
                }
            }
        }
    }
}
