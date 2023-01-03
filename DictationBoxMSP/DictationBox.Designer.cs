namespace DictationBoxMSP
{
    partial class DictationBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DictationBoxForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonScreenCapture = new System.Windows.Forms.Button();
            this.buttonSaveToFile = new System.Windows.Forms.Button();
            this.buttonPasteText = new System.Windows.Forms.Button();
            this.buttonFrontSize = new System.Windows.Forms.Button();
            this.CopyOnlyButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ReplaceTextBox = new System.Windows.Forms.TextBox();
            this.ReplaceButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FindtextBox = new System.Windows.Forms.TextBox();
            this.FindButton = new System.Windows.Forms.Button();
            this.SearchButton = new System.Windows.Forms.Button();
            this.WindowButton = new System.Windows.Forms.Button();
            this.VariableButton = new System.Windows.Forms.Button();
            this.CamelButton = new System.Windows.Forms.Button();
            this.TransferButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonClear);
            this.panel1.Controls.Add(this.buttonOpenFile);
            this.panel1.Controls.Add(this.buttonBrowse);
            this.panel1.Controls.Add(this.buttonScreenCapture);
            this.panel1.Controls.Add(this.buttonSaveToFile);
            this.panel1.Controls.Add(this.buttonPasteText);
            this.panel1.Controls.Add(this.buttonFrontSize);
            this.panel1.Controls.Add(this.CopyOnlyButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ReplaceTextBox);
            this.panel1.Controls.Add(this.ReplaceButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.FindtextBox);
            this.panel1.Controls.Add(this.FindButton);
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Controls.Add(this.WindowButton);
            this.panel1.Controls.Add(this.VariableButton);
            this.panel1.Controls.Add(this.CamelButton);
            this.panel1.Controls.Add(this.TransferButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(899, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 626);
            this.panel1.TabIndex = 15;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.buttonOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOpenFile.ForeColor = System.Drawing.Color.White;
            this.buttonOpenFile.Location = new System.Drawing.Point(100, 486);
            this.buttonOpenFile.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(94, 38);
            this.buttonOpenFile.TabIndex = 14;
            this.buttonOpenFile.Text = "Open File";
            this.buttonOpenFile.UseVisualStyleBackColor = false;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.buttonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBrowse.ForeColor = System.Drawing.Color.White;
            this.buttonBrowse.Location = new System.Drawing.Point(101, 394);
            this.buttonBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(94, 38);
            this.buttonBrowse.TabIndex = 10;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = false;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonScreenCapture
            // 
            this.buttonScreenCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScreenCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.buttonScreenCapture.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonScreenCapture.ForeColor = System.Drawing.Color.White;
            this.buttonScreenCapture.Location = new System.Drawing.Point(101, 440);
            this.buttonScreenCapture.Margin = new System.Windows.Forms.Padding(4);
            this.buttonScreenCapture.Name = "buttonScreenCapture";
            this.buttonScreenCapture.Size = new System.Drawing.Size(94, 38);
            this.buttonScreenCapture.TabIndex = 12;
            this.buttonScreenCapture.Text = "Capture";
            this.buttonScreenCapture.UseVisualStyleBackColor = false;
            this.buttonScreenCapture.Click += new System.EventHandler(this.buttonScreenCapture_Click);
            // 
            // buttonSaveToFile
            // 
            this.buttonSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveToFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.buttonSaveToFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSaveToFile.ForeColor = System.Drawing.Color.White;
            this.buttonSaveToFile.Location = new System.Drawing.Point(6, 486);
            this.buttonSaveToFile.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSaveToFile.Name = "buttonSaveToFile";
            this.buttonSaveToFile.Size = new System.Drawing.Size(94, 38);
            this.buttonSaveToFile.TabIndex = 13;
            this.buttonSaveToFile.Text = "Save to File";
            this.buttonSaveToFile.UseVisualStyleBackColor = false;
            this.buttonSaveToFile.Click += new System.EventHandler(this.buttonSaveToFile_Click);
            // 
            // buttonPasteText
            // 
            this.buttonPasteText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPasteText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.buttonPasteText.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonPasteText.ForeColor = System.Drawing.Color.White;
            this.buttonPasteText.Location = new System.Drawing.Point(7, 302);
            this.buttonPasteText.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPasteText.Name = "buttonPasteText";
            this.buttonPasteText.Size = new System.Drawing.Size(94, 38);
            this.buttonPasteText.TabIndex = 7;
            this.buttonPasteText.Text = "Paste Text";
            this.buttonPasteText.UseVisualStyleBackColor = false;
            this.buttonPasteText.Click += new System.EventHandler(this.buttonPasteText_Click);
            // 
            // buttonFrontSize
            // 
            this.buttonFrontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFrontSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.buttonFrontSize.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonFrontSize.ForeColor = System.Drawing.Color.White;
            this.buttonFrontSize.Location = new System.Drawing.Point(7, 256);
            this.buttonFrontSize.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFrontSize.Name = "buttonFrontSize";
            this.buttonFrontSize.Size = new System.Drawing.Size(189, 38);
            this.buttonFrontSize.TabIndex = 6;
            this.buttonFrontSize.Text = "Font Size";
            this.buttonFrontSize.UseVisualStyleBackColor = false;
            this.buttonFrontSize.Click += new System.EventHandler(this.ButtonFrontSize_Click);
            // 
            // CopyOnlyButton
            // 
            this.CopyOnlyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyOnlyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.CopyOnlyButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CopyOnlyButton.ForeColor = System.Drawing.Color.White;
            this.CopyOnlyButton.Location = new System.Drawing.Point(7, 348);
            this.CopyOnlyButton.Margin = new System.Windows.Forms.Padding(4);
            this.CopyOnlyButton.Name = "CopyOnlyButton";
            this.CopyOnlyButton.Size = new System.Drawing.Size(189, 38);
            this.CopyOnlyButton.TabIndex = 8;
            this.CopyOnlyButton.Text = "Copy Only";
            this.CopyOnlyButton.UseVisualStyleBackColor = false;
            this.CopyOnlyButton.Click += new System.EventHandler(this.CopyOnlyButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Replace Text";
            // 
            // ReplaceTextBox
            // 
            this.ReplaceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplaceTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.ReplaceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ReplaceTextBox.ForeColor = System.Drawing.Color.White;
            this.ReplaceTextBox.Location = new System.Drawing.Point(7, 154);
            this.ReplaceTextBox.Multiline = true;
            this.ReplaceTextBox.Name = "ReplaceTextBox";
            this.ReplaceTextBox.Size = new System.Drawing.Size(189, 48);
            this.ReplaceTextBox.TabIndex = 4;
            // 
            // ReplaceButton
            // 
            this.ReplaceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplaceButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.ReplaceButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ReplaceButton.ForeColor = System.Drawing.Color.White;
            this.ReplaceButton.Location = new System.Drawing.Point(8, 210);
            this.ReplaceButton.Margin = new System.Windows.Forms.Padding(4);
            this.ReplaceButton.Name = "ReplaceButton";
            this.ReplaceButton.Size = new System.Drawing.Size(189, 38);
            this.ReplaceButton.TabIndex = 5;
            this.ReplaceButton.Text = "Replace";
            this.ReplaceButton.UseVisualStyleBackColor = false;
            this.ReplaceButton.Click += new System.EventHandler(this.ReplaceButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Find Text";
            // 
            // FindtextBox
            // 
            this.FindtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindtextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.FindtextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FindtextBox.ForeColor = System.Drawing.Color.White;
            this.FindtextBox.Location = new System.Drawing.Point(6, 26);
            this.FindtextBox.Multiline = true;
            this.FindtextBox.Name = "FindtextBox";
            this.FindtextBox.Size = new System.Drawing.Size(189, 48);
            this.FindtextBox.TabIndex = 1;
            this.FindtextBox.TextChanged += new System.EventHandler(this.FindtextBox_TextChanged);
            // 
            // FindButton
            // 
            this.FindButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.FindButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.FindButton.ForeColor = System.Drawing.Color.White;
            this.FindButton.Location = new System.Drawing.Point(7, 82);
            this.FindButton.Margin = new System.Windows.Forms.Padding(4);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(189, 38);
            this.FindButton.TabIndex = 2;
            this.FindButton.Text = "Find";
            this.FindButton.UseVisualStyleBackColor = false;
            this.FindButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SearchButton.ForeColor = System.Drawing.Color.White;
            this.SearchButton.Location = new System.Drawing.Point(7, 394);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(4);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(94, 38);
            this.SearchButton.TabIndex = 9;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = false;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // WindowButton
            // 
            this.WindowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.WindowButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.WindowButton.ForeColor = System.Drawing.Color.White;
            this.WindowButton.Location = new System.Drawing.Point(7, 440);
            this.WindowButton.Margin = new System.Windows.Forms.Padding(4);
            this.WindowButton.Name = "WindowButton";
            this.WindowButton.Size = new System.Drawing.Size(94, 38);
            this.WindowButton.TabIndex = 11;
            this.WindowButton.Text = "Window";
            this.WindowButton.UseVisualStyleBackColor = false;
            this.WindowButton.Click += new System.EventHandler(this.WindowButton_Click);
            // 
            // VariableButton
            // 
            this.VariableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VariableButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.VariableButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.VariableButton.ForeColor = System.Drawing.Color.White;
            this.VariableButton.Location = new System.Drawing.Point(101, 532);
            this.VariableButton.Margin = new System.Windows.Forms.Padding(4);
            this.VariableButton.Name = "VariableButton";
            this.VariableButton.Size = new System.Drawing.Size(94, 38);
            this.VariableButton.TabIndex = 16;
            this.VariableButton.Text = "Variable";
            this.VariableButton.UseVisualStyleBackColor = false;
            this.VariableButton.Click += new System.EventHandler(this.VariableButton_Click);
            // 
            // CamelButton
            // 
            this.CamelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CamelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.CamelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CamelButton.ForeColor = System.Drawing.Color.White;
            this.CamelButton.Location = new System.Drawing.Point(7, 532);
            this.CamelButton.Margin = new System.Windows.Forms.Padding(4);
            this.CamelButton.Name = "CamelButton";
            this.CamelButton.Size = new System.Drawing.Size(94, 38);
            this.CamelButton.TabIndex = 15;
            this.CamelButton.Text = "Camel";
            this.CamelButton.UseVisualStyleBackColor = false;
            this.CamelButton.Click += new System.EventHandler(this.CamelButton_Click);
            // 
            // TransferButton
            // 
            this.TransferButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TransferButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.TransferButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.TransferButton.ForeColor = System.Drawing.Color.White;
            this.TransferButton.Location = new System.Drawing.Point(7, 578);
            this.TransferButton.Margin = new System.Windows.Forms.Padding(4);
            this.TransferButton.Name = "TransferButton";
            this.TransferButton.Size = new System.Drawing.Size(189, 38);
            this.TransferButton.TabIndex = 17;
            this.TransferButton.Text = "Transfer";
            this.TransferButton.UseVisualStyleBackColor = false;
            this.TransferButton.Click += new System.EventHandler(this.TransferButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(899, 626);
            this.panel2.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.AutoWordSelection = true;
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Cascadia Code", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(25);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(899, 626);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClear.ForeColor = System.Drawing.Color.White;
            this.buttonClear.Location = new System.Drawing.Point(101, 302);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(94, 38);
            this.buttonClear.TabIndex = 18;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = false;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // DictationBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.ClientSize = new System.Drawing.Size(1099, 626);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DictationBoxForm";
            this.Text = "Custom Dictation Box";
            this.Load += new System.EventHandler(this.DictationBoxForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonPasteText;
        private System.Windows.Forms.Button buttonFrontSize;
        private System.Windows.Forms.Button CopyOnlyButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ReplaceTextBox;
        private System.Windows.Forms.Button ReplaceButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FindtextBox;
        private System.Windows.Forms.Button FindButton;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button WindowButton;
        private System.Windows.Forms.Button VariableButton;
        private System.Windows.Forms.Button CamelButton;
        private System.Windows.Forms.Button TransferButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonSaveToFile;
        private System.Windows.Forms.Button buttonScreenCapture;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Button buttonClear;
    }
}

