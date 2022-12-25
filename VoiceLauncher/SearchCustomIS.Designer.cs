namespace VoiceLauncher
{
    partial class SearchCustomIS
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchCustomIS));
            this.voiceLauncherDataSet = new VoiceLauncher.VoiceLauncherDataSet();
            this.customIntelliSenseTableAdapter = new VoiceLauncher.VoiceLauncherDataSetTableAdapters.CustomIntelliSenseTableAdapter();
            this.tableAdapterManager = new VoiceLauncher.VoiceLauncherDataSetTableAdapters.TableAdapterManager();
            this.customIntelliSenseBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.customIntelliSenseListBox = new System.Windows.Forms.ListBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.customIntelliSenseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBoxResults = new System.Windows.Forms.GroupBox();
            this.textBoxRemarks = new System.Windows.Forms.TextBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonCopyText = new System.Windows.Forms.Button();
            this.buttonInsert = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.voiceLauncherDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource)).BeginInit();
            this.groupBoxResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // voiceLauncherDataSet
            // 
            this.voiceLauncherDataSet.DataSetName = "VoiceLauncherDataSet";
            this.voiceLauncherDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customIntelliSenseTableAdapter
            // 
            this.customIntelliSenseTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CustomIntelliSenseTableAdapter = this.customIntelliSenseTableAdapter;
            this.tableAdapterManager.UpdateOrder = VoiceLauncher.VoiceLauncherDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // customIntelliSenseBindingSource1
            // 
            this.customIntelliSenseBindingSource1.AllowNew = false;
            this.customIntelliSenseBindingSource1.DataMember = "CustomIntelliSense";
            this.customIntelliSenseBindingSource1.DataSource = this.voiceLauncherDataSet;
            // 
            // customIntelliSenseListBox
            // 
            this.customIntelliSenseListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customIntelliSenseListBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.customIntelliSenseListBox.DataSource = this.customIntelliSenseBindingSource1;
            this.customIntelliSenseListBox.DisplayMember = "Display_Value";
            this.customIntelliSenseListBox.Font = new System.Drawing.Font("Cascadia Code", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customIntelliSenseListBox.ForeColor = System.Drawing.SystemColors.Window;
            this.customIntelliSenseListBox.FormattingEnabled = true;
            this.customIntelliSenseListBox.ItemHeight = 27;
            this.customIntelliSenseListBox.Location = new System.Drawing.Point(0, 32);
            this.customIntelliSenseListBox.Name = "customIntelliSenseListBox";
            this.customIntelliSenseListBox.Size = new System.Drawing.Size(535, 463);
            this.customIntelliSenseListBox.TabIndex = 1;
            this.customIntelliSenseListBox.ValueMember = "ID";
            this.customIntelliSenseListBox.SelectedIndexChanged += new System.EventHandler(this.customIntelliSenseListBox_SelectedIndexChanged);
            this.customIntelliSenseListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.customIntelliSenseListBox_KeyDown);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearch.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxSearch.Font = new System.Drawing.Font("Cascadia Code", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSearch.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxSearch.Location = new System.Drawing.Point(0, 0);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(535, 31);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            this.textBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearch_KeyDown);
            // 
            // groupBoxResults
            // 
            this.groupBoxResults.Controls.Add(this.textBoxRemarks);
            this.groupBoxResults.Controls.Add(this.textBoxResult);
            this.groupBoxResults.Controls.Add(this.buttonCopyText);
            this.groupBoxResults.Controls.Add(this.buttonInsert);
            this.groupBoxResults.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxResults.Location = new System.Drawing.Point(0, 484);
            this.groupBoxResults.Name = "groupBoxResults";
            this.groupBoxResults.Size = new System.Drawing.Size(535, 236);
            this.groupBoxResults.TabIndex = 6;
            this.groupBoxResults.TabStop = false;
            this.groupBoxResults.Text = "Results";
            this.groupBoxResults.Enter += new System.EventHandler(this.groupBoxResults_Enter);
            // 
            // textBoxRemarks
            // 
            this.textBoxRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRemarks.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxRemarks.Font = new System.Drawing.Font("Cascadia Code", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRemarks.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxRemarks.Location = new System.Drawing.Point(194, 14);
            this.textBoxRemarks.Multiline = true;
            this.textBoxRemarks.Name = "textBoxRemarks";
            this.textBoxRemarks.Size = new System.Drawing.Size(335, 35);
            this.textBoxRemarks.TabIndex = 8;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxResult.Font = new System.Drawing.Font("Cascadia Code", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResult.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxResult.Location = new System.Drawing.Point(0, 55);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(535, 178);
            this.textBoxResult.TabIndex = 7;
            // 
            // buttonCopyText
            // 
            this.buttonCopyText.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonCopyText.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCopyText.Location = new System.Drawing.Point(91, 17);
            this.buttonCopyText.Name = "buttonCopyText";
            this.buttonCopyText.Size = new System.Drawing.Size(97, 32);
            this.buttonCopyText.TabIndex = 6;
            this.buttonCopyText.Text = "Copy Text";
            this.buttonCopyText.UseVisualStyleBackColor = true;
            this.buttonCopyText.Click += new System.EventHandler(this.buttonCopyText_Click_1);
            // 
            // buttonInsert
            // 
            this.buttonInsert.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonInsert.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInsert.Location = new System.Drawing.Point(10, 17);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(75, 32);
            this.buttonInsert.TabIndex = 5;
            this.buttonInsert.Text = "Insert";
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click_1);
            // 
            // SearchCustomIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(535, 720);
            this.Controls.Add(this.groupBoxResults);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.customIntelliSenseListBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchCustomIS";
            this.Text = "SearchCustomIS";
            this.Load += new System.EventHandler(this.SearchCustomIS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.voiceLauncherDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource)).EndInit();
            this.groupBoxResults.ResumeLayout(false);
            this.groupBoxResults.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VoiceLauncherDataSet voiceLauncherDataSet;
        private VoiceLauncherDataSetTableAdapters.CustomIntelliSenseTableAdapter customIntelliSenseTableAdapter;
        private VoiceLauncherDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingSource customIntelliSenseBindingSource1;
        private System.Windows.Forms.ListBox customIntelliSenseListBox;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.BindingSource customIntelliSenseBindingSource;
        private System.Windows.Forms.GroupBox groupBoxResults;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonCopyText;
        private System.Windows.Forms.Button buttonInsert;
        private System.Windows.Forms.TextBox textBoxRemarks;
    }
}