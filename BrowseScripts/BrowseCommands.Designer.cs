namespace BrowseScripts
{
    partial class BrowseCommands
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseCommands));
            this.dataGridViewCommands = new System.Windows.Forms.DataGridView();
            this.dataGridViewCommand = new System.Windows.Forms.DataGridView();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxFilterAll = new System.Windows.Forms.CheckBox();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCommandFilter = new System.Windows.Forms.TextBox();
            this.textBoxType = new System.Windows.Forms.TextBox();
            this.dataGridViewLists = new System.Windows.Forms.DataGridView();
            this.dataGridViewList = new System.Windows.Forms.DataGridView();
            this.buttonCopyCode = new System.Windows.Forms.Button();
            this.textBoxFilterValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxListFilter = new System.Windows.Forms.TextBox();
            this.buttonExportCommand = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommands)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCommands
            // 
            this.dataGridViewCommands.AllowUserToAddRows = false;
            this.dataGridViewCommands.AllowUserToDeleteRows = false;
            this.dataGridViewCommands.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommands.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridViewCommands.Location = new System.Drawing.Point(14, 43);
            this.dataGridViewCommands.Name = "dataGridViewCommands";
            this.dataGridViewCommands.ReadOnly = true;
            this.dataGridViewCommands.Size = new System.Drawing.Size(709, 365);
            this.dataGridViewCommands.TabIndex = 9;
            this.dataGridViewCommands.SelectionChanged += new System.EventHandler(this.DataGridViewCommands_SelectionChanged);
            // 
            // dataGridViewCommand
            // 
            this.dataGridViewCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewCommand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommand.Location = new System.Drawing.Point(14, 414);
            this.dataGridViewCommand.Name = "dataGridViewCommand";
            this.dataGridViewCommand.ReadOnly = true;
            this.dataGridViewCommand.Size = new System.Drawing.Size(709, 375);
            this.dataGridViewCommand.TabIndex = 11;
            this.dataGridViewCommand.SelectionChanged += new System.EventHandler(this.DataGridViewCommand_SelectionChanged);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFilter.Location = new System.Drawing.Point(157, 3);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(100, 26);
            this.textBoxFilter.TabIndex = 1;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.TextBoxFilter_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Command Type Filter";
            // 
            // checkBoxFilterAll
            // 
            this.checkBoxFilterAll.AutoSize = true;
            this.checkBoxFilterAll.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFilterAll.Location = new System.Drawing.Point(301, 5);
            this.checkBoxFilterAll.Name = "checkBoxFilterAll";
            this.checkBoxFilterAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxFilterAll.Size = new System.Drawing.Size(152, 22);
            this.checkBoxFilterAll.TabIndex = 2;
            this.checkBoxFilterAll.Text = "Filter All Commands";
            this.checkBoxFilterAll.UseVisualStyleBackColor = true;
            this.checkBoxFilterAll.CheckedChanged += new System.EventHandler(this.CheckBoxFilterAll_CheckedChanged);
            // 
            // textBoxContent
            // 
            this.textBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContent.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxContent.Location = new System.Drawing.Point(881, 43);
            this.textBoxContent.Multiline = true;
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.ReadOnly = true;
            this.textBoxContent.Size = new System.Drawing.Size(653, 747);
            this.textBoxContent.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(459, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Command &Filter";
            // 
            // textBoxCommandFilter
            // 
            this.textBoxCommandFilter.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCommandFilter.Location = new System.Drawing.Point(601, 3);
            this.textBoxCommandFilter.Name = "textBoxCommandFilter";
            this.textBoxCommandFilter.Size = new System.Drawing.Size(100, 26);
            this.textBoxCommandFilter.TabIndex = 4;
            this.textBoxCommandFilter.TextChanged += new System.EventHandler(this.TextBoxCommandFilter_TextChanged);
            // 
            // textBoxType
            // 
            this.textBoxType.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxType.Location = new System.Drawing.Point(881, 3);
            this.textBoxType.Name = "textBoxType";
            this.textBoxType.Size = new System.Drawing.Size(353, 26);
            this.textBoxType.TabIndex = 7;
            // 
            // dataGridViewLists
            // 
            this.dataGridViewLists.AllowUserToAddRows = false;
            this.dataGridViewLists.AllowUserToDeleteRows = false;
            this.dataGridViewLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLists.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridViewLists.Location = new System.Drawing.Point(729, 43);
            this.dataGridViewLists.Name = "dataGridViewLists";
            this.dataGridViewLists.ReadOnly = true;
            this.dataGridViewLists.Size = new System.Drawing.Size(149, 365);
            this.dataGridViewLists.TabIndex = 10;
            this.dataGridViewLists.SelectionChanged += new System.EventHandler(this.DataGridViewLists_SelectionChanged);
            // 
            // dataGridViewList
            // 
            this.dataGridViewList.AllowUserToAddRows = false;
            this.dataGridViewList.AllowUserToDeleteRows = false;
            this.dataGridViewList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewList.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridViewList.Location = new System.Drawing.Point(729, 414);
            this.dataGridViewList.Name = "dataGridViewList";
            this.dataGridViewList.ReadOnly = true;
            this.dataGridViewList.Size = new System.Drawing.Size(149, 375);
            this.dataGridViewList.TabIndex = 12;
            // 
            // buttonCopyCode
            // 
            this.buttonCopyCode.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCopyCode.Location = new System.Drawing.Point(1439, 3);
            this.buttonCopyCode.Name = "buttonCopyCode";
            this.buttonCopyCode.Size = new System.Drawing.Size(95, 26);
            this.buttonCopyCode.TabIndex = 8;
            this.buttonCopyCode.TabStop = false;
            this.buttonCopyCode.Text = "&Copy Code";
            this.buttonCopyCode.UseVisualStyleBackColor = true;
            this.buttonCopyCode.Click += new System.EventHandler(this.ButtonCopyCode_Click);
            // 
            // textBoxFilterValue
            // 
            this.textBoxFilterValue.Location = new System.Drawing.Point(96, 202);
            this.textBoxFilterValue.Multiline = true;
            this.textBoxFilterValue.Name = "textBoxFilterValue";
            this.textBoxFilterValue.Size = new System.Drawing.Size(323, 36);
            this.textBoxFilterValue.TabIndex = 14;
            this.textBoxFilterValue.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(707, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "&List Filter";
            // 
            // textBoxListFilter
            // 
            this.textBoxListFilter.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxListFilter.Location = new System.Drawing.Point(778, 3);
            this.textBoxListFilter.Name = "textBoxListFilter";
            this.textBoxListFilter.Size = new System.Drawing.Size(100, 26);
            this.textBoxListFilter.TabIndex = 6;
            this.textBoxListFilter.TextChanged += new System.EventHandler(this.TextBoxListFilter_TextChanged);
            // 
            // buttonExportCommand
            // 
            this.buttonExportCommand.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExportCommand.Location = new System.Drawing.Point(1276, 3);
            this.buttonExportCommand.Name = "buttonExportCommand";
            this.buttonExportCommand.Size = new System.Drawing.Size(130, 26);
            this.buttonExportCommand.TabIndex = 15;
            this.buttonExportCommand.TabStop = false;
            this.buttonExportCommand.Text = "Copy Current";
            this.buttonExportCommand.UseVisualStyleBackColor = true;
            this.buttonExportCommand.Click += new System.EventHandler(this.ButtonExportCommand_Click);
            // 
            // BrowseCommands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1546, 802);
            this.Controls.Add(this.buttonExportCommand);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxListFilter);
            this.Controls.Add(this.textBoxFilterValue);
            this.Controls.Add(this.buttonCopyCode);
            this.Controls.Add(this.dataGridViewList);
            this.Controls.Add(this.dataGridViewLists);
            this.Controls.Add(this.textBoxType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxCommandFilter);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.checkBoxFilterAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.dataGridViewCommand);
            this.Controls.Add(this.dataGridViewCommands);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "BrowseCommands";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Browse KnowBrainer Commands";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommands)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewCommands;
        private System.Windows.Forms.DataGridView dataGridViewCommand;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxFilterAll;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCommandFilter;
        private System.Windows.Forms.TextBox textBoxType;
        private System.Windows.Forms.DataGridView dataGridViewLists;
        private System.Windows.Forms.DataGridView dataGridViewList;
        private System.Windows.Forms.Button buttonCopyCode;
        private System.Windows.Forms.TextBox textBoxFilterValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxListFilter;
        private System.Windows.Forms.Button buttonExportCommand;
    }
}

