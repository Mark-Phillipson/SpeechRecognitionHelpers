using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BrowseScripts
{
    public partial class ManageLists : Form
    {
        readonly DataSet dataSet = new DataSet();
        readonly BindingSource bindingSourceLists = new BindingSource();
        readonly BindingSource bindingSourceList = new BindingSource();
        private string _filename = "";
        public string Filter { get; set; }
        public ManageLists()
        {
            InitializeComponent();
        }
        private bool LoadDataAndSetUp(string filename)
        {
            if (filename == "")
            {
                filename = Properties.Settings.Default.LastFileOpened;
            }
            if (!File.Exists(filename))
            {
                filename = FileManagement.OpenXMLFile();
            }
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                return false;
            }
            var validDocument = FileManagement.LoadXMLDocument(filename, dataSet, this);
            if (validDocument == false)
            {
                NotifyErrorAndQuit();
                return false;
            }
            try
            {
                SetupLists();
            }
            catch (Exception)
            {
                NotifyErrorAndQuit();
                return false;
            }
            Properties.Settings.Default.LastFileOpened = filename;
            Properties.Settings.Default.Save();
            _filename = filename;
            return true;
        }
        private void SetupLists()
        {
            bindingSourceLists.DataSource = dataSet;
            bindingSourceLists.DataMember = "List";
            dataGridViewLists.EnableHeadersVisualStyles = true;
            dataGridViewLists.AutoGenerateColumns = true;
            dataGridViewLists.DataSource = bindingSourceLists;
            dataGridViewLists.Columns[0].HeaderText = "List Name";
            dataGridViewLists.AutoResizeColumns();
            dataGridViewLists.Columns[0].Width = 280;
            //dataGridViewLists.RowTemplate.Height = 35;
            dataGridViewLists.RowHeadersVisible = true;
            dataGridViewLists.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewLists.RowHeadersDefaultCellStyle.BackColor = Color.Black;

            bindingSourceList.DataSource = dataSet;
            bindingSourceList.DataMember = "value";
            dataGridViewList.EnableHeadersVisualStyles = true;
            dataGridViewList.AutoGenerateColumns = true;
            dataGridViewList.DataSource = bindingSourceList;
            dataGridViewList.RowHeadersVisible = true;
            dataGridViewList.Columns[0].HeaderText = "List Values";
            dataGridViewList.AutoResizeColumns();
            dataGridViewList.Columns[0].Width = 480;
            //dataGridViewList.RowTemplate.Height = 35;
            dataGridViewList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewList.RowHeadersDefaultCellStyle.BackColor = Color.Black;
        }
        private void NotifyErrorAndQuit()
        {
            MessageBox.Show($"The XML {_filename} file does not appear to be in the expected format. Please relaunch the application and select a valid file. ", "Problem with File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Properties.Settings.Default.LastFileOpened = "";
            Properties.Settings.Default.Save();
            Application.Exit();
        }
        private void ManageLists_Load(object sender, EventArgs e)
        {
            var loadedSuccessfully = LoadDataAndSetUp(_filename);
            if (!loadedSuccessfully)
            {
                Application.Exit();
                return;
            }
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                textBoxListFilter.Text = Filter;
                var filter = "name Like '%" + textBoxListFilter.Text + "%'";
                bindingSourceLists.Filter = filter;
            }
            textBoxListFilter.Focus();
        }

        private void dataGridViewLists_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void textBoxListFilter_TextChanged(object sender, EventArgs e)
        {
            if (textBoxListFilter.Text != null && textBoxListFilter.Text.Length > 0)
            {
                var filter = "name Like '%" + textBoxListFilter.Text + "%'";
                bindingSourceLists.Filter = filter;
            }
        }

        private void dataGridViewLists_SelectionChanged(object sender, EventArgs e)
        {
            var currentRow = bindingSourceLists.Current;
            if (currentRow != null)
            {
                bindingSourceList.Filter = "List_Id =" + ((System.Data.DataRowView)currentRow).Row.ItemArray[0];
            }
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            var settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.Indent = true;
            try
            {
                using (XmlWriter xmlWriter=XmlWriter.Create(_filename, settings))
                {

                }
                using (XmlWriter xmlFileToWrite = XmlWriter.Create(_filename, new XmlWriterSettings()))
                {

                    dataSet.WriteXml(xmlFileToWrite);
                    xmlFileToWrite.Close();
                    MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()," Manage Lists Error ");
            }

        }

        private void dataGridViewLists_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        }

        private void dataGridViewList_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            var currentRow = bindingSourceLists.Current;
            object foreignKey=0;
            if (currentRow!= null )
            {
                foreignKey= ((System.Data.DataRowView)currentRow).Row.ItemArray[0];
            }
            var dataTable =dataSet.Tables["value"];
            DataRow row;
            row=dataTable.NewRow();

            row["List_Id"] = foreignKey;
            row["value_Text"] = textBoxNewListItem.Text;
            dataTable.Rows.Add(row);
        }

        private void buttonAddNewList_Click(object sender, EventArgs e)
        {
            var currentRow = bindingSourceList.Current;
            object foreignKey = 0;
            if (currentRow != null)
            {
                 //= ((System.Data.DataRowView)currentRow).Row.ItemArray[2];
            }
            var dataTable = dataSet.Tables["List"];
            DataRow row;
            row = dataTable.NewRow();

            row["Lists_Id"] = 0;
            row["name"] = textBoxNewList.Text;
            dataTable.Rows.Add(row);

        }
    }
}
