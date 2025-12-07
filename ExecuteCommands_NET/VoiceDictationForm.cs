using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace DictationBoxMSP
{
    public class VoiceDictationForm : Form
    {
        private TextBox txtInput = null!;
        private Button btnSubmit = null!;
        private Button btnCancel = null!;
        private Button btnStart = null!;
        private System.Windows.Forms.Timer autoSubmitTimer = null!;
        private System.Windows.Forms.Timer startDictationTimer = null!;
        private int timeoutMs = 0;

        public string ResultText => txtInput.Text ?? string.Empty;

        public VoiceDictationForm(int timeoutMs = 20000, bool autoStartDictation = true)
        {
            this.timeoutMs = timeoutMs;
            InitializeComponents();
            ApplySharedStyles();
            if (autoStartDictation && timeoutMs >= 0)
            {
                this.Shown += VoiceDictationForm_Shown;
            }
        }

        private void InitializeComponents()
        {
            this.txtInput = new TextBox() { Multiline = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical };
            this.btnStart = new Button() { Text = "Start Dictation", Height = 40, AutoSize = false };
            this.btnSubmit = new Button() { Text = "Submit", Height = 48 };
            this.btnCancel = new Button() { Text = "Cancel", Height = 48 };
            this.autoSubmitTimer = new System.Windows.Forms.Timer();
            this.startDictationTimer = new System.Windows.Forms.Timer();

            // Bottom panel to hold buttons
            var bottomPanel = new Panel() { Dock = DockStyle.Bottom, Height = 56 };
            bottomPanel.Padding = new Padding(8);
            bottomPanel.BackColor = DisplayMessage.SharedBackColor;

            // Flow layout for buttons
            var flow = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, AutoSize = false };
            flow.Controls.Add(btnCancel);
            flow.Controls.Add(btnSubmit);
            flow.Controls.Add(btnStart);
            btnStart.Width = 150; btnSubmit.Width = 100; btnCancel.Width = 100;
            bottomPanel.Controls.Add(flow);

            this.Controls.Add(txtInput);
            this.Controls.Add(bottomPanel);

            this.Text = "Voice Dictation";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 400);

            btnSubmit.Click += BtnSubmit_Click;
            btnCancel.Click += BtnCancel_Click;
            btnStart.Click += BtnStart_Click;
            this.FormClosing += VoiceDictationForm_FormClosing;
            this.KeyPreview = true;
            this.KeyDown += VoiceDictationForm_KeyDown;
        }

        private void ApplySharedStyles()
        {
            try
            {
                this.BackColor = DisplayMessage.SharedBackColor;
                this.ForeColor = DisplayMessage.SharedForeColor;
                var baseFont = DisplayMessage.SharedFont ?? SystemFonts.MessageBoxFont;
                var larger = new Font(baseFont.FontFamily, Math.Max(baseFont.Size * 1.8f, baseFont.Size + 8f), baseFont.Style);
                this.Font = larger;
                txtInput.Font = larger;
                txtInput.BackColor = DisplayMessage.SharedBackColor;
                txtInput.ForeColor = DisplayMessage.SharedForeColor;
            }
            catch { }
        }

        private void VoiceDictationForm_Shown(object? sender, EventArgs e)
        {
            try
            {
                this.BringToFront();
                this.Activate();
                txtInput.Focus();
                txtInput.Select();
            }
            catch { }

            // Start dictation shortly after shown so focus is established
            startDictationTimer.Interval = 300;
            startDictationTimer.Tick += StartDictationTimer_Tick;
            startDictationTimer.Start();

            if (timeoutMs > 0)
            {
                autoSubmitTimer.Interval = timeoutMs;
                autoSubmitTimer.Tick += AutoSubmitTimer_Tick;
                autoSubmitTimer.Start();
            }
        }

        private void StartDictationTimer_Tick(object? sender, EventArgs e)
        {
            try { startDictationTimer.Stop(); } catch { }
            StartDictation();
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            StartDictation();
        }

        private void StartDictation()
        {
            try
            {
                txtInput.Focus();
                txtInput.Select();
                var sim = new InputSimulator();
                sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_H);
            }
            catch { }
        }

        private void AutoSubmitTimer_Tick(object? sender, EventArgs e)
        {
            autoSubmitTimer.Stop();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnSubmit_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void VoiceDictationForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try { autoSubmitTimer.Stop(); } catch { }
        }

        private void VoiceDictationForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
