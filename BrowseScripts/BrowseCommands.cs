using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BrowseScripts
{
    public partial class BrowseCommands : Form
    {
        readonly DataSet dataSet = new DataSet();
        readonly BindingSource bindingSourceCommands = new BindingSource();
        readonly BindingSource bindingSourceCommand = new BindingSource();
        readonly BindingSource BindingSourceContent = new BindingSource();
        readonly BindingSource bindingSourceLists = new BindingSource();
        readonly BindingSource bindingSourceList = new BindingSource();
        XDocument document;
        string filename = "";
        public XDocument Document { get => document; set => document = value; }

        public BrowseCommands()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridViewCommands.EnableHeadersVisualStyles = false;
            dataGridViewCommand.EnableHeadersVisualStyles = false;
            dataGridViewLists.EnableHeadersVisualStyles = false;
            dataGridViewList.EnableHeadersVisualStyles = false;
            dataGridViewLists.Visible = false;
            dataGridViewList.Visible = false;

            //int millisecondsDelay = Properties.Settings.Default.Delay;
            filename = Properties.Settings.Default.LastFileOpened;
            if (!File.Exists(filename))
            {
                filename = null;
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Roaming\KnowBrainer\";
                    openFileDialog.Filter = "XML KnowBrainer Command Files (*.xml)|*.xml|All Files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Please select a KnowBrainer XML commands file to browse.";
                    while (true)
                    {
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            filename = openFileDialog.FileName;
                        }
                        if (filename == null || filename.Length == 0)
                        {
                            Application.Exit();
                            return;
                        }
                        try
                        {
                            Properties.Settings.Default.LastFileOpened = filename;
                            Properties.Settings.Default.Save();
                            Document = LoadXMLDocument(filename);
                            break;
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("The XML file does not appear to be in the expected format. Please select a valid file and try again. " + exception.Message, "Problem with File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            openFileDialog.FileName = null;
                            filename = null;
                        }
                    }
                }
            }
            else
            {
                Document = LoadXMLDocument(filename);
            }
            bindingSourceCommands.DataSource = dataSet;
            bindingSourceCommands.DataMember = "Commands";
            bindingSourceCommands.Sort = "scope ASC, moduleDescription ASC, windowTitle ASC";
            dataGridViewCommands.AutoGenerateColumns = true;
            dataGridViewCommands.DataSource = bindingSourceCommands;
            dataGridViewCommands.AutoResizeColumns();
            dataGridViewCommands.AllowUserToAddRows = false;
            dataGridViewCommands.AllowUserToDeleteRows = false;
            dataGridViewCommands.Columns[0].HeaderText = "Scope";
            dataGridViewCommands.Columns[1].HeaderText = "Module";
            dataGridViewCommands.Columns[2].HeaderText = "Company";
            dataGridViewCommands.Columns[2].Width = 130;
            dataGridViewCommands.Columns[3].HeaderText = "Module Description";
            dataGridViewCommands.Columns[4].HeaderText = "Window Title";
            dataGridViewCommands.Columns[4].Width = 70;
            dataGridViewCommands.Columns[5].HeaderText = "Window Class";
            dataGridViewCommands.Columns[5].Width = 70;
            dataGridViewCommands.RowHeadersVisible = false;
            for (int i = 0; i < 6; i++)
            {
                dataGridViewCommands.Columns[i].ReadOnly = i != 1 ? true : false;
            }
            bindingSourceCommand.DataSource = dataSet;
            bindingSourceCommand.DataMember = "Command";
            bindingSourceCommand.Sort = "name";

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

            dataGridViewCommand.RowHeadersVisible = false;
            var currentRow = bindingSourceCommands.Current;
            bindingSourceCommand.Filter = "Commands_Id =" + ((DataRowView)currentRow).Row.ItemArray[0];

            listViewCommandsAvailable.Visible = false;
            listViewCommandsAvailable.View = View.Tile;
            listViewCommandsAvailable.Columns.Add("Name", 90);
            listViewCommandsAvailable.ForeColor = System.Drawing.Color.White;
            listViewCommandsAvailable.BackColor = System.Drawing.Color.Black;
            listViewCommandsAvailable.Font = new System.Drawing.Font("Cascadia Code", 11, System.Drawing.FontStyle.Bold);

            SetUpTiles(currentRow);
            BindingSourceContent.DataSource = dataSet;
            BindingSourceContent.DataMember = "Content";
            currentRow = bindingSourceCommand.Current;
            BindingSourceContent.Filter = "Command_Id =" + ((DataRowView)currentRow).Row.ItemArray[1];
            currentRow = BindingSourceContent.Current;
            if (currentRow != null)
            {
                textBoxContent.Text = ((DataRowView)currentRow).Row.ItemArray[1].ToString();
                textBoxType.Text = ((DataRowView)currentRow).Row.ItemArray[0].ToString();
            }

            bindingSourceLists.DataSource = dataSet;
            bindingSourceLists.DataMember = "List";
            dataGridViewLists.AutoGenerateColumns = true;
            dataGridViewLists.DataSource = bindingSourceLists;
            dataGridViewLists.AutoResizeColumns();
            dataGridViewLists.Columns[0].HeaderText = "List Name";
            dataGridViewLists.Columns[0].Width = 140;
            dataGridViewLists.RowHeadersVisible = false;




            bindingSourceList.DataSource = dataSet;
            bindingSourceList.DataMember = "value";
            dataGridViewList.AutoGenerateColumns = true;
            dataGridViewList.DataSource = bindingSourceList;
            dataGridViewList.RowHeadersVisible = false;

            dataGridViewList.AutoResizeColumns();
            dataGridViewList.Columns[0].HeaderText = "List Values";
            dataGridViewList.Columns[0].Width = 140;
            string[] arguments;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() < 2)// I.e. no commandline arguments
            {
                checkBoxFilterAll.Checked = false;
                textBoxFilter.Text = "Access";
                textBoxCommandFilter.Text = "Import";
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

        private XDocument LoadXMLDocument(string filename)
        {
            XDocument document = XDocument.Load(filename);
            var commands = document.Descendants("Command").Count();
            var lists = document.Descendants("List").Count();
            Text = $"Browse KnowBrainer Commands (Commands: {commands} Lists: {lists}) {filename}";
            dataSet.ReadXmlSchema(filename);
            dataSet.ReadXml(filename);
            return document;
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
            if (currentRowDRV == null || currentRowDRV.Row.ItemArray[2]?.ToString().Length == 0)
            {
                return;
            }
            if (currentRow != null)
            {
                BindingSourceContent.Filter = "Command_Id =" + ((DataRowView)currentRow).Row.ItemArray[1];
                FilterLists((DataRowView)currentRow);
            }
            currentRow = BindingSourceContent.Current;
            if (currentRow != null)
            {
                textBoxContent.Text = ((DataRowView)currentRow).Row.ItemArray[1].ToString();
                textBoxType.Text = ((DataRowView)currentRow).Row.ItemArray[0].ToString();
            }
            if (checkBoxFilterAll.Checked == true)
            {
                currentRow = bindingSourceCommand.Current;
                if (currentRow != null)
                {
                    bindingSourceCommands.Filter = "Commands_Id =" + ((System.Data.DataRowView)currentRow).Row.ItemArray[5];
                }
            }
        }

        private void FilterLists(DataRowView currentRow)
        {
            dataGridViewLists.Visible = false;
            dataGridViewList.Visible = false;
            if (currentRow.Row.ItemArray[2]?.ToString().Length == 0)
            {
                return;
            }
            var commandName = (string)((DataRowView)currentRow).Row.ItemArray[2];
            if (commandName.Contains("<") && commandName.Contains(">"))
            {
                var filter = "";
                var position1 = commandName.IndexOf("<");
                var position2 = commandName.IndexOf(">");
                var temporary = commandName;
                while (position2 > 0)
                {
                    var listName = temporary.Substring(position1 + 1, position2 - position1 - 1);
                    filter = filter + (filter.Length > 0 ? " Or " : "") + "name = '" + listName + "'";
                    if (temporary.Length > position2 + 2)
                    {
                        temporary = temporary.Substring(position2 + 2);
                        position1 = temporary.IndexOf("<");
                        position2 = temporary.IndexOf(">");
                    }
                    else
                    {
                        temporary = "";
                        position2 = 0;
                    }
                }
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

        private void ButtonCopyCode_Click(object sender, EventArgs e)
        {
            textBoxContent.Focus();
            textBoxContent.Copy();
            textBoxFilter.Focus();
        }

        private void TextBoxListFilter_TextChanged(object sender, EventArgs e)
        {
            if (textBoxListFilter.Text != null && textBoxListFilter.Text.Length > 0)
            {
                var filter = "name Like '%" + textBoxListFilter.Text + "%'";
                bindingSourceLists.Filter = filter;
            }
        }

        private void ButtonExportCommand_Click(object sender, EventArgs e)
        {
            var currentRow = bindingSourceCommand.Current;
            if (currentRow != null)
            {
                var description = ((System.Data.DataRowView)currentRow).Row.ItemArray[0].ToString();
                var command = ((System.Data.DataRowView)currentRow).Row.ItemArray[2].ToString();
                var group = ((System.Data.DataRowView)currentRow).Row.ItemArray[3].ToString();
                currentRow = bindingSourceCommands.Current;
                var scope = ((System.Data.DataRowView)currentRow).Row.ItemArray[1].ToString();
                var moduleDescription = ((System.Data.DataRowView)currentRow).Row.ItemArray[4].ToString();
                var windowTitle = ((System.Data.DataRowView)currentRow).Row.ItemArray[5].ToString();
                var commandText = $"This command is used for {scope} {moduleDescription} {windowTitle} {Environment.NewLine}" +
                    $"The spoken command name is: '{command}' {description} {group}{Environment.NewLine}{Environment.NewLine}" +
                    $"The {textBoxType.Text} code is as follows:{Environment.NewLine}{Environment.NewLine}" +
                    $"{textBoxContent.Text}";
                //if (command.Contains("<") && command.Contains(">"))
                //{

                //}
                Clipboard.SetText(commandText);

                //XDocument documentExport = new XDocument(
                //     new XComment("Exported KnowBrainer Command"),
                //     new XElement("KnowBrainerCommands",
                //     from element in document.Elements("Commands")
                //     where
                //     (from add in element.Elements("Command")
                //      where
                //      (string)add.Attribute("name") == command
                //      select add)
                //     .Any()
                //     select element
                // ));

                //documentExport.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\{command}.xml");
                //Application.Exit();
            }
        }

        private void buttonViewScript_Click(object sender, EventArgs e)
        {
            ExampleCallingScript form = new ExampleCallingScript();
            form.ShowDialog();
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

        private void buttonOpenXmlFile_Click(object sender, EventArgs e)
        {
            if (filename.Length > 0 && File.Exists(filename))
            {
                try
                {
                    Process.Start(filename);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Exception!", MessageBoxButtons.OK);
                }
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

        private void buttonCommandsAvailable_Click(object sender, EventArgs e)
        {
            SetUpTiles(bindingSourceCommands.Current);
            listViewCommandsAvailable.Dock = DockStyle.Fill;
            listViewCommandsAvailable.Visible = true;
            listViewCommandsAvailable.BringToFront();
        }
    }
}
