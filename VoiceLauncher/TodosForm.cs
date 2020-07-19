using System;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class TodosForm : Form
    {
        public string SearchTerm { get; set; }
        readonly VoiceLauncherContext db;
        public TodosForm()
        {
            InitializeComponent();
            db = new VoiceLauncherContext();
        }

        private void TodosForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.DarkGray;
            this.ForeColor = Color.White;
            FontFamily fontFamily = new FontFamily("Calibri");
            Font font = new Font(fontFamily, (float)12, FontStyle.Bold, GraphicsUnit.Point);
            var style = new DataGridViewCellStyle
            { BackColor = Color.FromArgb(38, 38, 38), ForeColor = Color.White, Font = font };
            todosDataGridView.DefaultCellStyle = style;
            todosDataGridView.ColumnHeadersDefaultCellStyle = style;
            todosDataGridView.RowHeadersDefaultCellStyle = style;
            todosDataGridView.RowsDefaultCellStyle = style;
            todosDataGridView.EnableHeadersVisualStyles = false;

            todosBindingNavigator.BackColor = Color.Black;
            todosBindingNavigator.ForeColor = Color.White;
            todosDataGridView.BackgroundColor = Color.Black;
            todosDataGridView.ForeColor = Color.White;
            bindingNavigatorPositionItem.BackColor = Color.Black;
            bindingNavigatorPositionItem.ForeColor = Color.White;
            todosDataGridView.RowTemplate.Height = 50;
            todosDataGridView.RowTemplate.MinimumHeight = 30;
            foreach (DataGridViewColumn column in todosDataGridView.Columns)
            {
                if (column.Name.Contains("3"))
                {
                    //column.Width = 950;
                    column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                todosDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            SetUpADataSource();

        }

        private void SetUpADataSource()
        {
            Text = $"Todos Filter: {SearchTerm}";
            db.Todos.Where(v => v.Archived == false).OrderBy(v => v.Project).ThenBy(v => v.Title).Load();

            if (SearchTerm.ToLower() == "all")
            {
                todosBindingSource.DataSource = db.Todos.Local.ToBindingList();
            }
            else
            {
                var filteredData = db.Todos.Local.ToBindingList()
                    .Where(v => v.Title.Contains(SearchTerm) || v.Description.Contains(SearchTerm) || v.Project.Contains(SearchTerm));
                todosBindingSource.DataSource = filteredData.Count() > 0 ? filteredData : filteredData.ToArray();
            }
            todosDataGridView.Refresh();
        }

        private void todosDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
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

        private void todosBindingNavigator_Click(object sender, EventArgs e)
        {

        }

        private void todosBindingNavigator_RefreshItems(object sender, EventArgs e)
        {

        }

        private void todosDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (todosDataGridView.Columns[e.ColumnIndex].Name == "dataGridViewCheckBoxColumn1" && e.Value != null)
            {
                bool completed = (bool)e.Value;
                if (completed)
                {
                    e.CellStyle.BackColor = Color.Green;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Red;
                }
            }
        }

        private void toolStripButtonSaveData_Click(object sender, EventArgs e)
        {
            this.Validate();
            todosBindingSource.EndEdit();
            db.SaveChanges();
            this.todosDataGridView.Refresh();
            this.Text = $"Saved Successfully at {DateTime.Now.ToShortTimeString()}";

        }

        private void FilterTextBox_Leave(object sender, EventArgs e)
        {
            if (FilterTextBox.Text != null)
            {
                SearchTerm = FilterTextBox.Text;
                SetUpADataSource();
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SearchTerm = "All";
            SetUpADataSource();
        }
    }
}
