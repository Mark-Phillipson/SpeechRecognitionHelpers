using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KillApplications
{
    public partial class KillApplicationsForm : Form
    {
        private readonly BindingSource bindingSource = new BindingSource();
        public KillApplicationsForm()
        {
            InitializeComponent();
        }
        private void KillApplications_Load(object sender, EventArgs e)
        {
            // Create the list to use as the custom source. 
            AutoCompleteStringCollection source = SetUpAutoCompleteAndBindingSource();
            SetUpDataGrid();
            // Create and initialize the text box.
            var textBox = textBoxFilter;
            textBoxFilter.AutoCompleteCustomSource = source;
            textBoxFilter.AutoCompleteMode =
                              AutoCompleteMode.Suggest;
            textBoxFilter.AutoCompleteSource =
                              AutoCompleteSource.CustomSource;
            dataGridView1.AutoResizeColumns();
            label1.Dock = DockStyle.None;
            textBoxFilter.Dock = DockStyle.None;
            dataGridView1.Dock = DockStyle.None;
            // no smaller than design time size
            MinimumSize = new Size(Width, Height);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
        }

        private AutoCompleteStringCollection SetUpAutoCompleteAndBindingSource()
        {
            var source = new AutoCompleteStringCollection();
            var processes = Process.GetProcesses();

            int counter = 0;
            var applicationName = "";
            foreach (var process in processes.Where(p => p.MainWindowTitle != null && p.MainWindowTitle.Length > 1).OrderBy(p => p.ProcessName))
            {
                counter++;
                applicationName = GetApplicationName(process.ProcessName);
                bindingSource.Add(new ProcessClass(process.Id, process.ProcessName, process.MainWindowTitle, applicationName, counter));
                source.Add(process.ProcessName);
            }
            return source;
        }

        private void SetUpDataGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.DefaultCellStyle.BackColor = Color.Black;
            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.Font = new Font("Calibri", 11);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 1);
            dataGridView1.DataSource = bindingSource;
            var buttonColumn = new DataGridViewButtonColumn
            {
                DataPropertyName = "Kill",
                Name = "Kill Buttons",
                FlatStyle = FlatStyle.Standard
            };
            buttonColumn.DefaultCellStyle.SelectionBackColor = Color.Red;
            dataGridView1.Columns.Add(buttonColumn);
            DataGridViewColumn column = new DataGridViewColumn();
            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "Id"
            };
            dataGridView1.Columns.Add(column);
            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProcessName",
                Name = "Process Name"
            };
            dataGridView1.Columns.Add(column);
            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Title",
                Name = "Application Title",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            dataGridView1.Columns.Add(column);
            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ApplicationName",
                Name = "Application Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            dataGridView1.Columns.Add(column);
        }

        private string GetApplicationName(string processName)
        {
            var applicationName = "";
            switch (processName)
            {
                case "nsbrowse":
                    applicationName = "Command Browser";
                    break;
                case "natspeak":
                    applicationName = "Dragon Professional Individual";
                    break;
                case "dragonbar":
                    applicationName = "Dragon Bar";
                    break;
                case "chrome":
                    applicationName = "Google Chrome";
                    break;
                case "KBPro":
                    applicationName = "KnowBrainer";
                    break;
                case "MSACCESS":
                    applicationName = "Microsoft Access";
                    break;
                case "EXCEL":
                    applicationName = "Microsoft Excel";
                    break;
                case "notepad":
                    applicationName = "Microsoft Notepad";
                    break;
                case "outlook":
                    applicationName = "Microsoft Outlook";
                    break;
                case "WINWORD":
                    applicationName = "Microsoft Word";
                    break;
                case "upwork":
                    applicationName = "Upwork Time Tracker";
                    break;
                case "firefox":
                    applicationName = "Mozilla Firefox";
                    break;
                default:
                    break;
            }
            return applicationName;
        }

        private void TextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            RefreshDataGridView(textBoxFilter.Text);
        }

        private void RefreshDataGridView(string filterValue)
        {
            var processes = Process.GetProcesses();
            var applicationName = "";
            if (filterValue != null && filterValue.Length > 0)
            {
                bindingSource.Clear();
                int counter = 0;
                foreach (var process in processes.Where(p => p.ProcessName.ToLower().Contains(filterValue.ToLower()) && p.MainWindowTitle != null && p.MainWindowTitle.Length > 1).OrderBy(p => p.ProcessName))
                {
                    counter++;
                    applicationName = GetApplicationName(process.ProcessName);
                    bindingSource.Add(new ProcessClass(process.Id, process.ProcessName, process.MainWindowTitle, applicationName, counter));
                }
            }
            else if (filterValue == null || filterValue.Length == 0)
            {
                bindingSource.Clear();
                int counter = 0;
                foreach (var process in processes.Where(p => p.MainWindowTitle != null && p.MainWindowTitle.Length > 1).OrderBy(p => p.ProcessName))
                {
                    counter++;
                    applicationName = GetApplicationName(process.ProcessName);
                    bindingSource.Add(new ProcessClass(process.Id, process.ProcessName, process.MainWindowTitle, applicationName, counter));
                }
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var rows = dataGridView1.Rows;
                var cells = rows[e.RowIndex].Cells;
                int processId = (int)cells[1].Value;
                Process process = Process.GetProcessById(processId);
                process.Kill();
                RefreshDataGridView(textBoxFilter.Text);
                textBoxFilter.Focus();
            }
        }
    }
}
