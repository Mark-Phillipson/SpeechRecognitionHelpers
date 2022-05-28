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
            this.customIntelliSenseBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.customIntelliSenseBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.customIntelliSenseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.voiceLauncherDataSet = new VoiceLauncher.VoiceLauncherDataSet();
            this.customIntelliSenseTableAdapter = new VoiceLauncher.VoiceLauncherDataSetTableAdapters.CustomIntelliSenseTableAdapter();
            this.tableAdapterManager = new VoiceLauncher.VoiceLauncherDataSetTableAdapters.TableAdapterManager();
            this.customIntelliSenseBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.customIntelliSenseListBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingNavigator)).BeginInit();
            this.customIntelliSenseBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voiceLauncherDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // customIntelliSenseBindingNavigator
            // 
            this.customIntelliSenseBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.customIntelliSenseBindingNavigator.BindingSource = this.customIntelliSenseBindingSource;
            this.customIntelliSenseBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.customIntelliSenseBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.customIntelliSenseBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.customIntelliSenseBindingNavigatorSaveItem});
            this.customIntelliSenseBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.customIntelliSenseBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.customIntelliSenseBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.customIntelliSenseBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.customIntelliSenseBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.customIntelliSenseBindingNavigator.Name = "customIntelliSenseBindingNavigator";
            this.customIntelliSenseBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.customIntelliSenseBindingNavigator.Size = new System.Drawing.Size(535, 25);
            this.customIntelliSenseBindingNavigator.TabIndex = 0;
            this.customIntelliSenseBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // customIntelliSenseBindingNavigatorSaveItem
            // 
            this.customIntelliSenseBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.customIntelliSenseBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("customIntelliSenseBindingNavigatorSaveItem.Image")));
            this.customIntelliSenseBindingNavigatorSaveItem.Name = "customIntelliSenseBindingNavigatorSaveItem";
            this.customIntelliSenseBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.customIntelliSenseBindingNavigatorSaveItem.Text = "Save Data";
            this.customIntelliSenseBindingNavigatorSaveItem.Click += new System.EventHandler(this.customIntelliSenseBindingNavigatorSaveItem_Click_1);
            // 
            // customIntelliSenseBindingSource
            // 
            this.customIntelliSenseBindingSource.DataMember = "CustomIntelliSense";
            this.customIntelliSenseBindingSource.DataSource = this.voiceLauncherDataSet;
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
            this.customIntelliSenseListBox.DataSource = this.customIntelliSenseBindingSource1;
            this.customIntelliSenseListBox.DisplayMember = "Display_Value";
            this.customIntelliSenseListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customIntelliSenseListBox.FormattingEnabled = true;
            this.customIntelliSenseListBox.Location = new System.Drawing.Point(0, 25);
            this.customIntelliSenseListBox.Name = "customIntelliSenseListBox";
            this.customIntelliSenseListBox.Size = new System.Drawing.Size(535, 425);
            this.customIntelliSenseListBox.TabIndex = 1;
            this.customIntelliSenseListBox.ValueMember = "ID";
            // 
            // SearchCustomIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 450);
            this.Controls.Add(this.customIntelliSenseListBox);
            this.Controls.Add(this.customIntelliSenseBindingNavigator);
            this.Name = "SearchCustomIS";
            this.Text = "SearchCustomIS";
            this.Load += new System.EventHandler(this.SearchCustomIS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingNavigator)).EndInit();
            this.customIntelliSenseBindingNavigator.ResumeLayout(false);
            this.customIntelliSenseBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voiceLauncherDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSenseBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VoiceLauncherDataSet voiceLauncherDataSet;
        private System.Windows.Forms.BindingSource customIntelliSenseBindingSource;
        private VoiceLauncherDataSetTableAdapters.CustomIntelliSenseTableAdapter customIntelliSenseTableAdapter;
        private VoiceLauncherDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator customIntelliSenseBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton customIntelliSenseBindingNavigatorSaveItem;
        private System.Windows.Forms.BindingSource customIntelliSenseBindingSource1;
        private System.Windows.Forms.ListBox customIntelliSenseListBox;
    }
}