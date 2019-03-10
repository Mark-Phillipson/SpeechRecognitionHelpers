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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TransferButton = new System.Windows.Forms.Button();
            this.CamelButton = new System.Windows.Forms.Button();
            this.VariableButton = new System.Windows.Forms.Button();
            this.WindowButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.MenuText;
            this.richTextBox1.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Location = new System.Drawing.Point(16, 17);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(979, 599);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // TransferButton
            // 
            this.TransferButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TransferButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TransferButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.TransferButton.Location = new System.Drawing.Point(1003, 529);
            this.TransferButton.Margin = new System.Windows.Forms.Padding(4);
            this.TransferButton.Name = "TransferButton";
            this.TransferButton.Size = new System.Drawing.Size(87, 87);
            this.TransferButton.TabIndex = 1;
            this.TransferButton.Text = "Transfer";
            this.TransferButton.UseVisualStyleBackColor = false;
            this.TransferButton.Click += new System.EventHandler(this.TransferButton_Click);
            // 
            // CamelButton
            // 
            this.CamelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CamelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CamelButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.CamelButton.Location = new System.Drawing.Point(1003, 432);
            this.CamelButton.Margin = new System.Windows.Forms.Padding(4);
            this.CamelButton.Name = "CamelButton";
            this.CamelButton.Size = new System.Drawing.Size(87, 87);
            this.CamelButton.TabIndex = 2;
            this.CamelButton.Text = "Camel";
            this.CamelButton.UseVisualStyleBackColor = false;
            this.CamelButton.Click += new System.EventHandler(this.CamelButton_Click);
            // 
            // VariableButton
            // 
            this.VariableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VariableButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VariableButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.VariableButton.Location = new System.Drawing.Point(1003, 335);
            this.VariableButton.Margin = new System.Windows.Forms.Padding(4);
            this.VariableButton.Name = "VariableButton";
            this.VariableButton.Size = new System.Drawing.Size(87, 87);
            this.VariableButton.TabIndex = 3;
            this.VariableButton.Text = "Variable";
            this.VariableButton.UseVisualStyleBackColor = false;
            this.VariableButton.Click += new System.EventHandler(this.VariableButton_Click);
            // 
            // WindowButton
            // 
            this.WindowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WindowButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.WindowButton.Location = new System.Drawing.Point(1003, 238);
            this.WindowButton.Margin = new System.Windows.Forms.Padding(4);
            this.WindowButton.Name = "WindowButton";
            this.WindowButton.Size = new System.Drawing.Size(87, 87);
            this.WindowButton.TabIndex = 4;
            this.WindowButton.Text = "Window";
            this.WindowButton.UseVisualStyleBackColor = false;
            this.WindowButton.Click += new System.EventHandler(this.WindowButton_Click);
            // 
            // DictationBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1093, 623);
            this.Controls.Add(this.WindowButton);
            this.Controls.Add(this.VariableButton);
            this.Controls.Add(this.CamelButton);
            this.Controls.Add(this.TransferButton);
            this.Controls.Add(this.richTextBox1);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DictationBoxForm";
            this.Text = "Dictation Box MSP";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button TransferButton;
        private System.Windows.Forms.Button CamelButton;
        private System.Windows.Forms.Button VariableButton;
        private System.Windows.Forms.Button WindowButton;
    }
}

