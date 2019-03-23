using KillApplications.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KillApplications
{
    public partial class KillApplicationsForm : Form
    {
        private LocalDbContext _localDbContext = new LocalDbContext();
        private BindingSource bindingSource = new BindingSource();
        public KillApplicationsForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Seed database if necessary
            SeedDatabase();
            // Create the list to use as the custom source. 
            var source = new AutoCompleteStringCollection();
            var processes = Process.GetProcesses();


            var applicationName = "";
            foreach (var process in processes.Where(p => p.MainWindowTitle !=  null  && p.MainWindowTitle.Length > 1).OrderBy(p => p.ProcessName))
            {
                applicationName = GetApplicationName(process.ProcessName);
                bindingSource.Add(new ProcessClass(process.Id, process.ProcessName, process.MainWindowTitle, applicationName));
                source.Add(process.ProcessName);
            }
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
            dataGridView1.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 11);
            dataGridView1.DataSource = bindingSource;
            DataGridViewColumn column = new DataGridViewButtonColumn();
            column.DataPropertyName = "Kill";
            column.Name = "Kill";
            column.DefaultCellStyle.ForeColor = Color.DarkRed;
            column.DefaultCellStyle.BackColor = Color.Black;


            dataGridView1.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Id";
            column.Name = "Id";
            dataGridView1.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "ProcessName";
            column.Name = "Process Name";
            dataGridView1.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Title";
            column.Name = "Application Title";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "ApplicationName";
            column.Name = "Application Name";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column);
            // Create and initialize the text box.
            var textBox = this.textBox1;
                textBox1.AutoCompleteCustomSource = source;
            textBox1.AutoCompleteMode =
                              AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource =
                              AutoCompleteSource.CustomSource;
            dataGridView1.AutoResizeColumns();
            label1.Dock = DockStyle.None;
            textBox1.Dock = DockStyle.None;
            dataGridView1.Dock = DockStyle.None;
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            this.AutoSize=true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
        }

        private void SeedDatabase()
        {
            var testApplication = _localDbContext.Applications.FirstOrDefault(a => a.ApplicationName == "Microsoft Access");
            if (testApplication == null)
            {
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "nsbrowse", ApplicationName = "Command Browser", Display = true,Kill="Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "natspeak", ApplicationName = "Dragon Professional Individual", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "dragonbar", ApplicationName = "DragonBar", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "chrome", ApplicationName = "Google Chrome", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "iexplore", ApplicationName = "Internet Explorer", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "KBPro", ApplicationName = "KnowBrainer", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "MSACCESS", ApplicationName = "Microsoft Access", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "EXCEL", ApplicationName = "Microsoft Excel", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "notepad", ApplicationName = "Microsoft Notepad", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "outlook", ApplicationName = "Microsoft Outlook", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "WINWORD", ApplicationName = "Microsoft Word", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "upwork", ApplicationName = "Time Tracker", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "VP", ApplicationName = "Voice Computer", Display = true, Kill = "Kill" });
                _localDbContext.Applications.Add(new Models.Application { ProcessName = "firefox", ApplicationName = "Mozilla Firefox", Display = true });
                _localDbContext.SaveChanges();
            }
        }

        private string GetApplicationName(string processName)
        {
            string applicationName = _localDbContext.Applications.Where(a => a.ProcessName == processName).FirstOrDefault()?.ApplicationName;
            return applicationName;
        }

        private class ProcessClass
        {
            public ProcessClass(int id, string processName, string title, string applicationName)
            {
                Id = id;
                ProcessName = processName;
                Title = title;
                ApplicationName = applicationName;
                Kill = "Kill";
            }
            public int Id { get; set; }
            public string ProcessName { get; set; }
            public string Title { get; set; }
            public string ApplicationName { get; set; }
            public string Kill  { get; set; }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             RefreshDataGridView();
            //this.dataGridView1.Focus();
        }

        private void RefreshDataGridView()
        {
            var processes = Process.GetProcesses();
            var applicationName = "";
            if (textBox1.Text != null && textBox1.Text.Length > 2)
            {
                bindingSource.Clear();
                foreach (var process in processes.Where(p => p.ProcessName.ToLower().Contains(textBox1.Text.ToLower()) && p.MainWindowTitle != null && p.MainWindowTitle.Length > 1).OrderBy(p => p.ProcessName))
                {
                    applicationName = GetApplicationName(process.ProcessName);
                    bindingSource.Add(new ProcessClass(process.Id, process.ProcessName, process.MainWindowTitle, applicationName));
                }
            }
            else
            {
                bindingSource.Clear();
                foreach (var process in processes.Where(p => p.MainWindowTitle != null && p.MainWindowTitle.Length > 1).OrderBy(p => p.ProcessName))
                {
                    applicationName = GetApplicationName(process.ProcessName);
                    bindingSource.Add(new ProcessClass(process.Id, process.ProcessName, process.MainWindowTitle, applicationName));
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex==0)
            {
                var rows = dataGridView1.Rows;
                var cells = rows[e.RowIndex].Cells;
                int processId =(int)cells[1].Value;
                Process process = Process.GetProcessById(processId);
                process.Kill();
                RefreshDataGridView();
            }
        }
    }
}
