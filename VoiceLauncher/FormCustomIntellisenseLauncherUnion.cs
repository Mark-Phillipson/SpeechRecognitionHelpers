using DictationBoxMSP;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class FormCustomIntellisenseLauncherUnion : Form
    {
        public FormCustomIntellisenseLauncherUnion()
        {
            InitializeComponent();
            db = new VoiceLauncherContext();
        }
        readonly VoiceLauncherContext db;
        bool formIsClosed = false;
        public string SearchTerm { get; set; }
        private void FormCustomIntellisenseLauncherUnion_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            FontFamily fontFamily = new FontFamily("Cascadia Code");
            Font font = new Font(fontFamily, (float)11, FontStyle.Bold, GraphicsUnit.Point);
            var style = new DataGridViewCellStyle
            { BackColor = Color.Black, ForeColor = Color.White, Font = font };
            customIntellisenseLauncherUnionsDataGridView.DefaultCellStyle = style;
            customIntellisenseLauncherUnionsDataGridView.ColumnHeadersDefaultCellStyle = style;
            customIntellisenseLauncherUnionsDataGridView.RowHeadersDefaultCellStyle = style;
            customIntellisenseLauncherUnionsDataGridView.RowsDefaultCellStyle = style;
            customIntellisenseLauncherUnionsDataGridView.EnableHeadersVisualStyles = false;

            customIntellisenseLauncherUnionsBindingNavigator.BackColor = Color.Black;
            customIntellisenseLauncherUnionsBindingNavigator.ForeColor = Color.White;
            customIntellisenseLauncherUnionsDataGridView.BackgroundColor = Color.Black;
            customIntellisenseLauncherUnionsDataGridView.ForeColor = Color.White;
            bindingNavigatorPositionItem.BackColor = Color.Black;
            bindingNavigatorPositionItem.ForeColor = Color.White;

            FilterTextBox.BackColor = Color.Black;
            FilterTextBox.ForeColor = Color.White;
            FilterTextBox.BorderStyle = BorderStyle.FixedSingle;
            customIntellisenseLauncherUnionsDataGridView.RowTemplate.Height = 30;
            customIntellisenseLauncherUnionsDataGridView.RowTemplate.MinimumHeight = 30;
            SetUpDataSource();
            foreach (DataGridViewColumn column in customIntellisenseLauncherUnionsDataGridView.Columns)
            {
                if (column.Name.Contains("Launch"))
                {
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.ReadOnly = false;
                    column.Selected = true;

                }
                else
                {
                    column.ReadOnly = true;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (column.Name.Contains("3")) // SendkeysValue
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                customIntellisenseLauncherUnionsDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            
            
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            formIsClosed = true;
            db.Dispose();
            base.OnClosing(e);
        }
        private void SetUpDataSource()
        {
            db.CustomIntellisenseLauncherUnions.OrderBy(v => v.Category).ThenBy(v => v.DisplayValue).Load();
            Text = $"{Text} Filter: {SearchTerm}";

            if (SearchTerm.ToLower() == "all")
            {
                customIntellisenseLauncherUnionsBindingSource.DataSource = db.CustomIntellisenseLauncherUnions.Local.ToBindingList();
            }
            else
            {
                var filteredData = db.CustomIntellisenseLauncherUnions.Local.ToBindingList()
                    .Where(v => v.DisplayValue.ToLower().Contains(SearchTerm.ToLower()) || v.SendkeysValue.ToLower().Contains(SearchTerm.ToLower()));
                customIntellisenseLauncherUnionsBindingSource.DataSource = filteredData.Count() > 0 ? filteredData : filteredData.ToArray();
            }
            customIntellisenseLauncherUnionsDataGridView.Refresh();
            setRowNumber(customIntellisenseLauncherUnionsDataGridView);
        }

        private void customIntellisenseLauncherUnionsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == customIntellisenseLauncherUnionsDataGridView.Columns["Launch"].Index)
            {
                var commandline = (string)customIntellisenseLauncherUnionsDataGridView.Rows[e.RowIndex].Cells[6].Value;
                var category = (string)customIntellisenseLauncherUnionsDataGridView.Rows[e.RowIndex].Cells[4].Value;
                try
                {
                    if (category == "Folders")
                    {
                        Process.Start("explorer.exe", commandline);
                    }
                    else
                    {
                        Process.Start(commandline);
                    }
                    Application.Exit();
                }
                catch (Exception exception)
                {
                    if (exception.Message == "The system cannot find the file specified")
                    {
                        Clipboard.SetText(commandline);
                        DisplayMessage displayExpectedMessage = new DisplayMessage($"Command Line: {commandline}\r\rCOPIED!");

                        displayExpectedMessage.ShowDialog();
                        Application.Exit();
                        return;
                    }
                    DisplayMessage displayMessage = new DisplayMessage($"Error Message: {exception.Message}");
                    displayMessage.ShowDialog();

                    
                }
            }
        }

        private void customIntellisenseLauncherUnionsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs anError)
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

        private void FilterTextBox_Leave(object sender, EventArgs e)
        {
            if (formIsClosed)
            {
                return;
            }
            if (FilterTextBox.Text != null && FilterTextBox.Text.Length > 0)
            {
                SearchTerm = FilterTextBox.Text;
                SetUpDataSource();
            }
        }

        private void customIntellisenseLauncherUnionsDataGridView_Paint(object sender, PaintEventArgs e)
        {

        }
        private void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }
    }
}
