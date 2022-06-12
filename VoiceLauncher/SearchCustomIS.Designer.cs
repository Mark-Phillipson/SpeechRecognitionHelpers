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
            this.textBoxResult = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.voiceLauncherDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource)).BeginInit();
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
            this.customIntelliSenseListBox.Location = new System.Drawing.Point(0, 30);
            this.customIntelliSenseListBox.Name = "customIntelliSenseListBox";
            this.customIntelliSenseListBox.Size = new System.Drawing.Size(535, 382);
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
            // textBoxResult
            // 
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxResult.Font = new System.Drawing.Font("Cascadia Code", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResult.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxResult.Location = new System.Drawing.Point(0, 418);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(535, 147);
            this.textBoxResult.TabIndex = 3;
            // 
            // SearchCustomIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(535, 566);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.customIntelliSenseListBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchCustomIS";
            this.Text = "SearchCustomIS";
            this.Load += new System.EventHandler(this.SearchCustomIS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.voiceLauncherDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource)).EndInit();
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
        private System.Windows.Forms.TextBox textBoxResult;
    }
}