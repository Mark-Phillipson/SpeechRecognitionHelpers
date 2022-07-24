namespace BrowseScripts
{
    partial class ManageLists
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewLists = new System.Windows.Forms.DataGridView();
            this.dataGridViewList = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxListFilter = new System.Windows.Forms.TextBox();
            this.buttonSaveChanges = new System.Windows.Forms.Button();
            this.buttonAddNew = new System.Windows.Forms.Button();
            this.textBoxNewListItem = new System.Windows.Forms.TextBox();
            this.textBoxNewList = new System.Windows.Forms.TextBox();
            this.buttonAddNewList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewLists
            // 
            this.dataGridViewLists.AllowUserToAddRows = false;
            this.dataGridViewLists.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewLists.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewLists.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewLists.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.dataGridViewLists.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLists.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLists.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridViewLists.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewLists.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridViewLists.GridColor = System.Drawing.Color.White;
            this.dataGridViewLists.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewLists.MultiSelect = false;
            this.dataGridViewLists.Name = "dataGridViewLists";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Cascadia Code", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Red;
            this.dataGridViewLists.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewLists.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewLists.Size = new System.Drawing.Size(412, 716);
            this.dataGridViewLists.TabIndex = 98;
            this.dataGridViewLists.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewLists_RowsAdded);
            this.dataGridViewLists.SelectionChanged += new System.EventHandler(this.dataGridViewLists_SelectionChanged);
            // 
            // dataGridViewList
            // 
            this.dataGridViewList.AllowUserToAddRows = false;
            this.dataGridViewList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            this.dataGridViewList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.dataGridViewList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewList.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridViewList.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridViewList.GridColor = System.Drawing.Color.White;
            this.dataGridViewList.Location = new System.Drawing.Point(412, 0);
            this.dataGridViewList.MultiSelect = false;
            this.dataGridViewList.Name = "dataGridViewList";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.NullValue = "--";
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Cascadia Code", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Red;
            this.dataGridViewList.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewList.Size = new System.Drawing.Size(560, 716);
            this.dataGridViewList.StandardTab = true;
            this.dataGridViewList.TabIndex = 99;
            this.dataGridViewList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLists_CellValueChanged);
            this.dataGridViewList.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewList_DefaultValuesNeeded);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(978, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 26);
            this.label3.TabIndex = 0;
            this.label3.Text = "&List Filter";
            // 
            // textBoxListFilter
            // 
            this.textBoxListFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.textBoxListFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxListFilter.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxListFilter.ForeColor = System.Drawing.Color.White;
            this.textBoxListFilter.Location = new System.Drawing.Point(1086, 11);
            this.textBoxListFilter.Name = "textBoxListFilter";
            this.textBoxListFilter.Size = new System.Drawing.Size(310, 34);
            this.textBoxListFilter.TabIndex = 1;
            this.textBoxListFilter.TextChanged += new System.EventHandler(this.textBoxListFilter_TextChanged);
            // 
            // buttonSaveChanges
            // 
            this.buttonSaveChanges.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSaveChanges.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveChanges.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonSaveChanges.Location = new System.Drawing.Point(1084, 631);
            this.buttonSaveChanges.Name = "buttonSaveChanges";
            this.buttonSaveChanges.Size = new System.Drawing.Size(314, 72);
            this.buttonSaveChanges.TabIndex = 6;
            this.buttonSaveChanges.Text = "Save Changes";
            this.buttonSaveChanges.UseVisualStyleBackColor = false;
            this.buttonSaveChanges.Click += new System.EventHandler(this.buttonSaveChanges_Click);
            // 
            // buttonAddNew
            // 
            this.buttonAddNew.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonAddNew.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddNew.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonAddNew.Location = new System.Drawing.Point(1084, 145);
            this.buttonAddNew.Name = "buttonAddNew";
            this.buttonAddNew.Size = new System.Drawing.Size(314, 50);
            this.buttonAddNew.TabIndex = 3;
            this.buttonAddNew.Text = "Add New List Item";
            this.buttonAddNew.UseVisualStyleBackColor = false;
            this.buttonAddNew.Click += new System.EventHandler(this.buttonAddNew_Click);
            // 
            // textBoxNewListItem
            // 
            this.textBoxNewListItem.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBoxNewListItem.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNewListItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxNewListItem.Location = new System.Drawing.Point(1084, 106);
            this.textBoxNewListItem.Name = "textBoxNewListItem";
            this.textBoxNewListItem.Size = new System.Drawing.Size(314, 34);
            this.textBoxNewListItem.TabIndex = 2;
            // 
            // textBoxNewList
            // 
            this.textBoxNewList.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBoxNewList.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNewList.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxNewList.Location = new System.Drawing.Point(1084, 347);
            this.textBoxNewList.Name = "textBoxNewList";
            this.textBoxNewList.Size = new System.Drawing.Size(314, 34);
            this.textBoxNewList.TabIndex = 4;
            // 
            // buttonAddNewList
            // 
            this.buttonAddNewList.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonAddNewList.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddNewList.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonAddNewList.Location = new System.Drawing.Point(1084, 386);
            this.buttonAddNewList.Name = "buttonAddNewList";
            this.buttonAddNewList.Size = new System.Drawing.Size(314, 50);
            this.buttonAddNewList.TabIndex = 5;
            this.buttonAddNewList.Text = "Add New List";
            this.buttonAddNewList.UseVisualStyleBackColor = false;
            this.buttonAddNewList.Click += new System.EventHandler(this.buttonAddNewList_Click);
            // 
            // ManageLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1408, 716);
            this.Controls.Add(this.buttonAddNewList);
            this.Controls.Add(this.textBoxNewList);
            this.Controls.Add(this.textBoxNewListItem);
            this.Controls.Add(this.buttonAddNew);
            this.Controls.Add(this.buttonSaveChanges);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxListFilter);
            this.Controls.Add(this.dataGridViewList);
            this.Controls.Add(this.dataGridViewLists);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "ManageLists";
            this.Text = "ManageLists";
            this.Load += new System.EventHandler(this.ManageLists_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewLists;
        private System.Windows.Forms.DataGridView dataGridViewList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxListFilter;
        private System.Windows.Forms.Button buttonSaveChanges;
        private System.Windows.Forms.Button buttonAddNew;
        private System.Windows.Forms.TextBox textBoxNewListItem;
        private System.Windows.Forms.TextBox textBoxNewList;
        private System.Windows.Forms.Button buttonAddNewList;
    }
}