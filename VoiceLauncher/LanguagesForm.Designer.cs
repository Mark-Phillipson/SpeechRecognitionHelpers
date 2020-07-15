namespace VoiceLauncher
{
    partial class LanguagesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguagesForm));
            this.languageBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.languageBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.languageDataGridView = new System.Windows.Forms.DataGridView();
            this.customIntelliSensesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.customIntelliSensesDataGridView = new System.Windows.Forms.DataGridView();
            this.languageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.languageIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sendKeysValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commandTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deliveryTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.languageBindingNavigator)).BeginInit();
            this.languageBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.languageDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSensesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSensesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.languageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // languageBindingNavigator
            // 
            this.languageBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.languageBindingNavigator.BindingSource = this.languageBindingSource;
            this.languageBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.languageBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.languageBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.languageBindingNavigatorSaveItem});
            this.languageBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.languageBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.languageBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.languageBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.languageBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.languageBindingNavigator.Name = "languageBindingNavigator";
            this.languageBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.languageBindingNavigator.Size = new System.Drawing.Size(965, 25);
            this.languageBindingNavigator.TabIndex = 0;
            this.languageBindingNavigator.Text = "bindingNavigator1";
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
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
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
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
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
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // languageBindingNavigatorSaveItem
            // 
            this.languageBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.languageBindingNavigatorSaveItem.Enabled = false;
            this.languageBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("languageBindingNavigatorSaveItem.Image")));
            this.languageBindingNavigatorSaveItem.Name = "languageBindingNavigatorSaveItem";
            this.languageBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.languageBindingNavigatorSaveItem.Text = "Save Data";
            // 
            // languageDataGridView
            // 
            this.languageDataGridView.AutoGenerateColumns = false;
            this.languageDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.languageDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewCheckBoxColumn1});
            this.languageDataGridView.DataSource = this.languageBindingSource;
            this.languageDataGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.languageDataGridView.Location = new System.Drawing.Point(0, 25);
            this.languageDataGridView.Name = "languageDataGridView";
            this.languageDataGridView.Size = new System.Drawing.Size(300, 425);
            this.languageDataGridView.TabIndex = 1;
            // 
            // customIntelliSensesBindingSource
            // 
            this.customIntelliSensesBindingSource.DataMember = "CustomIntelliSenses";
            this.customIntelliSensesBindingSource.DataSource = this.languageBindingSource;
            // 
            // customIntelliSensesDataGridView
            // 
            this.customIntelliSensesDataGridView.AutoGenerateColumns = false;
            this.customIntelliSensesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customIntelliSensesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.languageIDDataGridViewTextBoxColumn,
            this.displayValueDataGridViewTextBoxColumn,
            this.sendKeysValueDataGridViewTextBoxColumn,
            this.commandTypeDataGridViewTextBoxColumn,
            this.categoryIDDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.deliveryTypeDataGridViewTextBoxColumn});
            this.customIntelliSensesDataGridView.DataSource = this.customIntelliSensesBindingSource;
            this.customIntelliSensesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customIntelliSensesDataGridView.Location = new System.Drawing.Point(300, 25);
            this.customIntelliSensesDataGridView.Name = "customIntelliSensesDataGridView";
            this.customIntelliSensesDataGridView.Size = new System.Drawing.Size(665, 425);
            this.customIntelliSensesDataGridView.TabIndex = 2;
            // 
            // languageBindingSource
            // 
            this.languageBindingSource.DataSource = typeof(VoiceLauncher.Models.Language);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "LanguageName";
            this.dataGridViewTextBoxColumn2.HeaderText = "LanguageName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Active";
            this.dataGridViewCheckBoxColumn1.HeaderText = "Active";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.Visible = false;
            // 
            // languageIDDataGridViewTextBoxColumn
            // 
            this.languageIDDataGridViewTextBoxColumn.DataPropertyName = "LanguageID";
            this.languageIDDataGridViewTextBoxColumn.HeaderText = "LanguageID";
            this.languageIDDataGridViewTextBoxColumn.Name = "languageIDDataGridViewTextBoxColumn";
            this.languageIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // displayValueDataGridViewTextBoxColumn
            // 
            this.displayValueDataGridViewTextBoxColumn.DataPropertyName = "Display_Value";
            this.displayValueDataGridViewTextBoxColumn.HeaderText = "Display_Value";
            this.displayValueDataGridViewTextBoxColumn.Name = "displayValueDataGridViewTextBoxColumn";
            // 
            // sendKeysValueDataGridViewTextBoxColumn
            // 
            this.sendKeysValueDataGridViewTextBoxColumn.DataPropertyName = "SendKeys_Value";
            this.sendKeysValueDataGridViewTextBoxColumn.HeaderText = "SendKeys_Value";
            this.sendKeysValueDataGridViewTextBoxColumn.Name = "sendKeysValueDataGridViewTextBoxColumn";
            // 
            // commandTypeDataGridViewTextBoxColumn
            // 
            this.commandTypeDataGridViewTextBoxColumn.DataPropertyName = "Command_Type";
            this.commandTypeDataGridViewTextBoxColumn.HeaderText = "Command_Type";
            this.commandTypeDataGridViewTextBoxColumn.Name = "commandTypeDataGridViewTextBoxColumn";
            // 
            // categoryIDDataGridViewTextBoxColumn
            // 
            this.categoryIDDataGridViewTextBoxColumn.DataPropertyName = "CategoryID";
            this.categoryIDDataGridViewTextBoxColumn.HeaderText = "CategoryID";
            this.categoryIDDataGridViewTextBoxColumn.Name = "categoryIDDataGridViewTextBoxColumn";
            this.categoryIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.categoryIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            // 
            // deliveryTypeDataGridViewTextBoxColumn
            // 
            this.deliveryTypeDataGridViewTextBoxColumn.DataPropertyName = "DeliveryType";
            this.deliveryTypeDataGridViewTextBoxColumn.HeaderText = "DeliveryType";
            this.deliveryTypeDataGridViewTextBoxColumn.Name = "deliveryTypeDataGridViewTextBoxColumn";
            // 
            // LanguagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 450);
            this.Controls.Add(this.customIntelliSensesDataGridView);
            this.Controls.Add(this.languageDataGridView);
            this.Controls.Add(this.languageBindingNavigator);
            this.Name = "LanguagesForm";
            this.Text = "LanguagesForm";
            this.Load += new System.EventHandler(this.LanguagesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.languageBindingNavigator)).EndInit();
            this.languageBindingNavigator.ResumeLayout(false);
            this.languageBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.languageDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSensesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customIntelliSensesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.languageBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource languageBindingSource;
        private System.Windows.Forms.BindingNavigator languageBindingNavigator;
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
        private System.Windows.Forms.ToolStripButton languageBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView languageDataGridView;
        private System.Windows.Forms.BindingSource customIntelliSensesBindingSource;
        private System.Windows.Forms.DataGridView customIntelliSensesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn languageIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sendKeysValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commandTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn categoryIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deliveryTypeDataGridViewTextBoxColumn;
    }
}