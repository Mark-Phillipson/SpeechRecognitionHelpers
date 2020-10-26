namespace ControlWSR
{
	partial class AvailableCommandsForm
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
			this.richTextBoxAvailableCommands = new System.Windows.Forms.RichTextBox();
			this.textBoxResults = new System.Windows.Forms.TextBox();
			this.TestingBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// richTextBoxAvailableCommands
			// 
			this.richTextBoxAvailableCommands.Font = new System.Drawing.Font("Cascadia Code SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBoxAvailableCommands.Location = new System.Drawing.Point(1, 21);
			this.richTextBoxAvailableCommands.Name = "richTextBoxAvailableCommands";
			this.richTextBoxAvailableCommands.ReadOnly = true;
			this.richTextBoxAvailableCommands.Size = new System.Drawing.Size(565, 521);
			this.richTextBoxAvailableCommands.TabIndex = 0;
			this.richTextBoxAvailableCommands.Text = "";
			// 
			// textBoxResults
			// 
			this.textBoxResults.Font = new System.Drawing.Font("Cascadia Code SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxResults.Location = new System.Drawing.Point(572, 20);
			this.textBoxResults.Multiline = true;
			this.textBoxResults.Name = "textBoxResults";
			this.textBoxResults.ReadOnly = true;
			this.textBoxResults.Size = new System.Drawing.Size(396, 390);
			this.textBoxResults.TabIndex = 1;
			// 
			// TestingBtn
			// 
			this.TestingBtn.Location = new System.Drawing.Point(807, 500);
			this.TestingBtn.Name = "TestingBtn";
			this.TestingBtn.Size = new System.Drawing.Size(86, 32);
			this.TestingBtn.TabIndex = 2;
			this.TestingBtn.Text = "Testing";
			this.TestingBtn.UseVisualStyleBackColor = true;
			this.TestingBtn.Click += new System.EventHandler(this.TestingBtn_Click);
			// 
			// AvailableCommandsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(976, 544);
			this.Controls.Add(this.TestingBtn);
			this.Controls.Add(this.textBoxResults);
			this.Controls.Add(this.richTextBoxAvailableCommands);
			this.Name = "AvailableCommandsForm";
			this.Text = "Available Commands";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AvailableCommandsForm_FormClosing);
			this.Load += new System.EventHandler(this.AvailableCommandsForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBoxAvailableCommands;
		private System.Windows.Forms.TextBox textBoxResults;
		private System.Windows.Forms.Button TestingBtn;
	}
}

