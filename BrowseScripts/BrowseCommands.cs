using SpeechRecognitionHelpersLibrary;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrowseScripts
{
    public partial class BrowseCommands : Form
    {
        readonly DataSet dataSet = new DataSet();
        readonly BindingSource bindingSourceCommands = new BindingSource();
        readonly BindingSource bindingSourceCommand = new BindingSource();
        readonly BindingSource bindingSourceContent = new BindingSource();
        readonly BindingSource bindingSourceLists = new BindingSource();
        readonly BindingSource bindingSourceList = new BindingSource();
        private string _filename = "";

        public BrowseCommands()
        {
            InitializeComponent();
            menuStrip1.BackColor = Color.FromArgb(100, 100, 100);
            menuStrip1.ForeColor = Color.FromArgb(12, 12, 12);
            menuStrip1.Renderer = new MyRenderer();
        }

        private void BrowseCommands_Load(object sender, EventArgs e)
        {
            var loadedSuccessfully = LoadDataAndSetUp(_filename);
            if (!loadedSuccessfully)
            {
                Application.Exit();
                return;
            }
            SetUpFormAccordingToCommandlineArguments();
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
                SetupCommands();
            }
            catch (Exception)
            {
                NotifyErrorAndQuit();
                return false;
            }
            Properties.Settings.Default.LastFileOpened = filename;
            Properties.Settings.Default.Save();
            SetUpCommand();
            SetupLists();
            SetupAvailableCommands();
            SetupContent();
            _filename = filename;
            return true;
        }

        private void SetupContent()
        {
            var currentRow = bindingSourceCommands.Current;
            if (currentRow == null)
            {
                return;
            }
            bindingSourceCommand.Filter = "Commands_Id =" + ((DataRowView)currentRow).Row.ItemArray[Mapping.PrimaryKey_Commands];
            SetUpTiles(currentRow);
            bindingSourceContent.DataSource = dataSet;
            bindingSourceContent.DataMember = "Content";
            currentRow = bindingSourceCommand.Current;
            bindingSourceContent.Filter = "Command_Id =" + ((DataRowView)currentRow).Row.ItemArray[Mapping.PrimaryKey_Command];
            currentRow = bindingSourceContent.Current;
            if (currentRow != null)
            {
                textBoxContent.Text = ((DataRowView)currentRow).Row.ItemArray[Mapping.Content].ToString();
                textBoxType.Text = ((DataRowView)currentRow).Row.ItemArray[Mapping.ScriptType].ToString();
            }
        }

        private void SetUpFormAccordingToCommandlineArguments()
        {
            string[] arguments;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() < 2)// I.e. no commandline arguments
            {
                checkBoxFilterAll.Checked = false;
                textBoxFilter.Text = "global";
                textBoxCommandFilter.Text = "move";
            }
            else if (args.Count() == 2)
            {
                arguments = Environment.GetCommandLineArgs();
                checkBoxFilterAll.Checked = true;
                textBoxCommandFilter.Text = arguments[1].Substring(1);
            }
            else
            {
                arguments = Environment.GetCommandLineArgs();
                checkBoxFilterAll.Checked = false;
                textBoxFilter.Text = arguments[1].Substring(1);
                textBoxCommandFilter.Text = arguments[2].Substring(1);
            }
        }

        private void SetupAvailableCommands()
        {
            listViewCommandsAvailable.Visible = false;
            listViewCommandsAvailable.View = View.List;
            listViewCommandsAvailable.Columns.Add("Name", 400);
            listViewCommandsAvailable.ForeColor = System.Drawing.Color.White;
            listViewCommandsAvailable.BackColor = System.Drawing.Color.Black;
            listViewCommandsAvailable.Font = new System.Drawing.Font("Cascadia Code", 9, System.Drawing.FontStyle.Bold);
        }

        private void SetupLists()
        {
            bindingSourceLists.DataSource = dataSet;
            bindingSourceLists.DataMember = "List";
            dataGridViewLists.EnableHeadersVisualStyles = false;
            dataGridViewLists.Visible = false;
            dataGridViewLists.AutoGenerateColumns = true;
            dataGridViewLists.DataSource = bindingSourceLists;
            dataGridViewLists.AutoResizeColumns();
            dataGridViewLists.Columns[0].HeaderText = "List Name";
            dataGridViewLists.Columns[0].Width = 140;
            dataGridViewLists.RowHeadersVisible = false;

            bindingSourceList.DataSource = dataSet;
            bindingSourceList.DataMember = "value";
            dataGridViewList.EnableHeadersVisualStyles = false;
            dataGridViewList.Visible = false;
            dataGridViewList.AutoGenerateColumns = true;
            dataGridViewList.DataSource = bindingSourceList;
            dataGridViewList.RowHeadersVisible = false;
            dataGridViewList.AutoResizeColumns();
            dataGridViewList.Columns[0].HeaderText = "List Values";
            dataGridViewList.Columns[0].Width = 140;

        }

        private void SetUpCommand()
        {
            bindingSourceCommand.DataSource = dataSet;
            bindingSourceCommand.DataMember = "Command";
            bindingSourceCommand.Sort = "name";
            dataGridViewCommand.RowHeadersVisible = false;
            dataGridViewCommand.EnableHeadersVisualStyles = false;
            dataGridViewCommand.AutoGenerateColumns = true;
            dataGridViewCommand.DataSource = bindingSourceCommand;
            dataGridViewCommand.Columns[0].HeaderText = "Description";
            dataGridViewCommand.Columns[0].Width = 90;
            dataGridViewCommand.Columns[1].HeaderText = "Name";
            dataGridViewCommand.Columns[1].Width = 305;
            dataGridViewCommand.Columns[2].HeaderText = "Group";
            dataGridViewCommand.Columns[2].Width = 200;
            dataGridViewCommand.Columns[3].HeaderText = "Enabled?";
            dataGridViewCommand.Columns[3].Width = 70;
            for (int i = 0; i < 4; i++)
            {
                dataGridViewCommand.Columns[i].ReadOnly = i != 1 ? true : false;
            }
        }

        private void SetupCommands()
        {
            bindingSourceCommands.DataSource = dataSet;
            bindingSourceCommands.DataMember = "Commands";
            bindingSourceCommands.Sort = "scope ASC, moduleDescription ASC, windowTitle ASC";
            dataGridViewCommands.EnableHeadersVisualStyles = false;
            dataGridViewCommands.AutoGenerateColumns = true;
            dataGridViewCommands.DataSource = bindingSourceCommands;
            dataGridViewCommands.AllowUserToAddRows = false;
            dataGridViewCommands.AllowUserToDeleteRows = false;
            dataGridViewCommands.Columns[0].HeaderText = "Scope";
            dataGridViewCommands.Columns[1].HeaderText = "Module";
            dataGridViewCommands.Columns[2].HeaderText = "Company";
            dataGridViewCommands.Columns[2].Width = 140;
            dataGridViewCommands.Columns[3].HeaderText = "Module Description";
            dataGridViewCommands.Columns[3].Width = 140;
            dataGridViewCommands.Columns[4].HeaderText = "Window Title";
            dataGridViewCommands.Columns[4].Width = 70;
            dataGridViewCommands.Columns[5].HeaderText = "Window Class";
            dataGridViewCommands.Columns[5].Width = 70;
            dataGridViewCommands.RowHeadersVisible = false;
            for (int i = 0; i < 6; i++)
            {
                dataGridViewCommands.Columns[i].ReadOnly = i != 1 ? true : false;
            }

        }

        private void NotifyErrorAndQuit()
        {
            MessageBox.Show($"The XML {_filename} file does not appear to be in the expected format. Please relaunch the application and select a valid file. ", "Problem with File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Properties.Settings.Default.LastFileOpened = "";
            Properties.Settings.Default.Save();
            Application.Exit();
        }

        private void SetUpTiles(object currentRow)
        {
            listViewCommandsAvailable.Clear();

            DataView dataView = new DataView(dataSet.Tables["Command"]);
            dataView.Sort = "Name ASC";
            ListViewItem listViewItem;
            var commandId = ((DataRowView)currentRow).Row.ItemArray[0].ToString();
            //MessageBox.Show(commandId);
            foreach (DataRowView rowView in dataView)
            {
                if (rowView["Commands_ID"].ToString() == commandId)
                {
                    listViewItem = new ListViewItem(new string[] { rowView["Name"].ToString() });
                    listViewCommandsAvailable.Items.Add(listViewItem);
                }
            }
        }

        private void TextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            string filter;
            if (textBoxFilter.Text != null && textBoxFilter.Text.Length > 0)
            {
                filter = "scope Like '%" + textBoxFilter.Text + "%' or module Like '%" + textBoxFilter.Text + "%' " +
                    " or company Like '%" + textBoxFilter.Text + "%' or moduleDescription Like '%" + textBoxFilter.Text + "%' " +
                    " or windowTitle Like '%" + textBoxFilter.Text + "%'";
                bindingSourceCommands.Filter = filter;
                SetUpTiles(bindingSourceCommands.Current);
            }
            else
            {
                filter = "";
                bindingSourceCommands.Filter = filter;
            }
            textBoxFilterValue.Text = filter;
        }

        private void DataGridViewCommands_SelectionChanged(object sender, EventArgs e)
        {
            if (checkBoxFilterAll.Checked == false)
            {
                var currentRow = bindingSourceCommands.Current;
                if (currentRow != null)
                {
                    bindingSourceCommand.Filter = "Commands_Id =" + ((System.Data.DataRowView)currentRow).Row.ItemArray[0];
                }
            }
        }

        private void DataGridViewCommand_SelectionChanged(object sender, EventArgs e)
        {
            object currentRow;
            try
            {
                currentRow = bindingSourceCommand.Current;
            }
            catch (Exception)
            {
                return;
            }
            var currentRowDRV = (DataRowView)currentRow;
            // Check there Is a name for the script If not return
            if (currentRowDRV == null || currentRowDRV.Row.ItemArray[Mapping.ScriptName]?.ToString().Length == 0)
            {
                return;
            }
            if (currentRow != null)
            {
                bindingSourceContent.Filter = "Command_Id =" + ((DataRowView)currentRow).Row.ItemArray[Mapping.PrimaryKey_Command];
                FilterLists((DataRowView)currentRow);
            }
            currentRow = bindingSourceContent.Current;
            if (currentRow != null)
            {
                textBoxContent.Text = ((DataRowView)currentRow).Row.ItemArray[Mapping.Content].ToString();
                textBoxType.Text = ((DataRowView)currentRow).Row.ItemArray[Mapping.ScriptType].ToString();
            }
            if (checkBoxFilterAll.Checked == true)
            {
                currentRow = bindingSourceCommand.Current;
                if (currentRow != null)
                {
                    bindingSourceCommands.Filter = "Commands_Id =" + ((System.Data.DataRowView)currentRow).Row.ItemArray[Mapping.CommandsFK];
                }
            }
        }

        private void FilterLists(DataRowView currentRow)
        {
            dataGridViewLists.Visible = false;
            dataGridViewList.Visible = false;
            if (currentRow.Row.ItemArray[Mapping.ScriptName]?.ToString().Length == 0)
            {
                return;
            }
            var commandName = (string)((DataRowView)currentRow).Row.ItemArray[Mapping.ScriptName];
            if (ListManagement.HasLists(commandName))
            {
                var filter = "";
                filter = ListManagement.BuildListFilter(commandName, filter);
                bindingSourceLists.Filter = filter;
                if (bindingSourceLists.List.Count > 0)
                {
                    dataGridViewLists.Visible = true;
                    dataGridViewList.Visible = true;
                }
            }
        }

        private void TextBoxCommandFilter_TextChanged(object sender, EventArgs e)
        {
            var filter = "";
            if (listViewCommandsAvailable.Visible == true && textBoxCommandFilter.Text != null && textBoxCommandFilter.Text.Length > 0)
            {
                ListViewItem foundItem = listViewCommandsAvailable.FindItemWithText(textBoxCommandFilter.Text, false, 0, true);
                if (foundItem != null)
                {
                    listViewCommandsAvailable.TopItem = foundItem;
                    foreach (ListViewItem item in listViewCommandsAvailable.Items)
                    {
                        item.Selected = false;
                    }
                    listViewCommandsAvailable.Items[foundItem.Index].Focused = true;
                    listViewCommandsAvailable.Items[foundItem.Index].Selected = true;
                }
            }
            else
            {
                if (textBoxCommandFilter.Text != null && textBoxCommandFilter.Text.Length > 0)
                {
                    if (checkBoxFilterAll.Checked == false)
                    {
                        var currentRow = bindingSourceCommands.Current;
                        if (currentRow != null)
                        {
                            filter = "Commands_Id =" + ((System.Data.DataRowView)currentRow).Row.ItemArray[0];
                            filter = filter + " And (description Like '%" + textBoxCommandFilter.Text + "%' or name Like '%" + textBoxCommandFilter.Text + "%' " +
                                                                    " or group Like '%" + textBoxCommandFilter.Text + "%')";
                            bindingSourceCommand.Filter = filter;
                        }
                        SetUpTiles(currentRow);
                    }
                    else
                    {
                        filter = "description Like '%" + textBoxCommandFilter.Text + "%' or name Like '%" + textBoxCommandFilter.Text + "%' " +
                                            " or group Like '%" + textBoxCommandFilter.Text + "%'";
                        bindingSourceCommand.Filter = filter;
                    }
                }
                else
                {
                    bindingSourceCommand.Filter = "";
                    var currentRow = bindingSourceCommands.Current;
                    if (currentRow != null)
                    {
                        filter = "Commands_Id =" + ((System.Data.DataRowView)currentRow).Row.ItemArray[0];
                        bindingSourceCommand.Filter = filter;
                    }
                }
                textBoxFilterValue.Text = filter;
            }
        }

        private void CheckBoxFilterAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFilterAll.Checked == true)
            {
                textBoxFilter.Enabled = false;
                textBoxCommandFilter.Focus();
            }
            else
            {
                textBoxFilter.Enabled = true;
                textBoxFilter.Focus();
            }
        }

        private void DataGridViewLists_SelectionChanged(object sender, EventArgs e)
        {
            var currentRow = bindingSourceLists.Current;
            if (currentRow != null)
            {
                bindingSourceList.Filter = "List_Id =" + ((System.Data.DataRowView)currentRow).Row.ItemArray[0];
            }
        }

        private void TextBoxListFilter_TextChanged(object sender, EventArgs e)
        {
            if (textBoxListFilter.Text != null && textBoxListFilter.Text.Length > 0)
            {
                var filter = "name Like '%" + textBoxListFilter.Text + "%'";
                bindingSourceLists.Filter = filter;
            }
        }

        private void dataGridViewLists_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridViewList_DoubleClick(object sender, EventArgs e)
        {
            MaximiseControl(dataGridViewList);
        }

        private void MaximiseControl(DataGridView dataGridView)
        {
            if (dataGridView.Dock == DockStyle.Fill)
            {
                dataGridView.Dock = DockStyle.None;
            }
            else
            {
                dataGridView.Dock = DockStyle.Fill;
                dataGridView.BringToFront();
            }
        }

        private void dataGridViewCommands_DoubleClick(object sender, EventArgs e)
        {
            MaximiseControl(dataGridViewCommands);
        }

        private void dataGridViewCommand_DoubleClick(object sender, EventArgs e)
        {
            MaximiseControl(dataGridViewCommand);
        }

        private void dataGridViewLists_DoubleClick(object sender, EventArgs e)
        {
            MaximiseControl(dataGridViewLists);
        }

        private void textBoxContent_DoubleClick(object sender, EventArgs e)
        {
            if (textBoxContent.Dock == DockStyle.Fill)
            {
                textBoxContent.Dock = DockStyle.None;
            }
            else
            {
                textBoxContent.Dock = DockStyle.Fill;
                textBoxContent.BringToFront();
            }
        }

        private void listViewCommandsAvailable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewCommandsAvailable.Dock == DockStyle.Fill)
            {
                listViewCommandsAvailable.Dock = DockStyle.None;
                listViewCommandsAvailable.Visible = false;
            }
        }

        private void selectDifferentXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newFileName = FileManagement.OpenXMLFile();
            var result = LoadDataAndSetUp(newFileName);
            if (result == false)
            {
                Application.Exit();
            }
        }

        private void commandsAvailableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpTiles(bindingSourceCommands.Current);
            listViewCommandsAvailable.Dock = DockStyle.Fill;
            listViewCommandsAvailable.Visible = true;
            listViewCommandsAvailable.BringToFront();
        }

        private void openXMLFileWithExternalAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_filename.Length > 0 && File.Exists(_filename))
            {
                try
                {
                    Process.Start(_filename);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Exception!", MessageBoxButtons.OK);
                }
            }
        }

        private void viewCallingScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExampleCallingScript form = new ExampleCallingScript();
            form.ShowDialog();
        }

        private void exportScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentRow = bindingSourceCommand.Current;
            if (currentRow != null)
            {
                var description = ((DataRowView)currentRow).Row.ItemArray[Mapping.Description].ToString();
                var command = ((DataRowView)currentRow).Row.ItemArray[Mapping.ScriptName].ToString();
                var group = ((DataRowView)currentRow).Row.ItemArray[Mapping.Group].ToString();
                currentRow = bindingSourceCommands.Current;
                var scope = ((DataRowView)currentRow).Row.ItemArray[Mapping.Scope].ToString();
                var module = ((DataRowView)currentRow).Row.ItemArray[Mapping.Module].ToString();
                var moduleDescription = ((DataRowView)currentRow).Row.ItemArray[Mapping.ModuleDescription].ToString();
                var windowTitle = ((DataRowView)currentRow).Row.ItemArray[Mapping.WindowTitle].ToString();
                var commandText = $"This command is used for {scope} {moduleDescription} {windowTitle} {Environment.NewLine}" +
                    $"The spoken command name is: '{command}' {description} {group}{Environment.NewLine}{Environment.NewLine}" +
                    $"The {textBoxType.Text} code is as follows:{Environment.NewLine}{Environment.NewLine}" +
                    $"{textBoxContent.Text}";
                Clipboard.SetText(commandText);
                FileManagement.ExportSingleCommand(dataSet, scope, module, command);
            }
            else
            {
                MessageBox.Show("Please select a Script first then click Export Script", "Script Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void copyCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxContent.Focus();
            textBoxContent.Copy();
            textBoxFilter.Focus();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}
