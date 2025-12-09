using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace DictationBoxMSP
{
    public class VoiceDictationForm : Form
    {
        private TextBox txtInput = null!;
        private Button btnCancel = null!;
        private Button btnStart = null!;
        private Button btnSendCommand = null!;
        private Button btnCopyText = null!;
        private Button btnSearchWeb = null!;
        private System.Windows.Forms.Timer autoSubmitTimer = null!;
        private System.Windows.Forms.Timer startDictationTimer = null!;
        private int timeoutMs = 0;

        public string ResultText => txtInput.Text ?? string.Empty;

        public VoiceDictationForm(int timeoutMs = -1, bool autoStartDictation = true)
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
            // Use '&' to indicate keyboard accelerators (mnemonics). These are shown as underlined
            // when Alt is pressed and allow keyboard activation (Alt+Key).
            this.btnStart = new Button() { Text = "Re-Start &Dictation", Height = 56, AutoSize = false };
            this.btnCancel = new Button() { Text = "&Cancel", Height = 56 };
            // Use Alt+S for Send Command (replaces the removed Submit button)
            this.btnSendCommand = new Button() { Text = "&Send Command", Height = 56 };
            this.btnCopyText = new Button() { Text = "Copy &Text", Height = 56 };
            this.btnSearchWeb = new Button() { Text = "Search &Web", Height = 56 };
            this.autoSubmitTimer = new System.Windows.Forms.Timer();
            this.startDictationTimer = new System.Windows.Forms.Timer();

            // Bottom panel to hold buttons
            var bottomPanel = new Panel() { Dock = DockStyle.Bottom, Height = 84 };
            bottomPanel.Padding = new Padding(8);
            bottomPanel.BackColor = DisplayMessage.SharedBackColor;

            // Flow layout for buttons
            var flow = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, AutoSize = false };
            flow.WrapContents = false;
            flow.Padding = new Padding(6);
            flow.Controls.Add(btnCancel);
            flow.Controls.Add(btnSendCommand);
            flow.Controls.Add(btnCopyText);
            flow.Controls.Add(btnSearchWeb);
            flow.Controls.Add(btnStart);
            // Keep explicit widths; remove Submit width since Submit is removed
            btnStart.Width = 220; btnCancel.Width = 120; btnSendCommand.Width = 140; btnCopyText.Width = 120; btnSearchWeb.Width = 140;
            bottomPanel.Controls.Add(flow);

            // Make button borders visible on all sides by using FlatStyle and a small border
            btnStart.FlatStyle = FlatStyle.Flat; btnStart.FlatAppearance.BorderSize = 1; btnStart.FlatAppearance.BorderColor = SystemColors.ControlDark; btnStart.Margin = new Padding(6);
            btnCancel.FlatStyle = FlatStyle.Flat; btnCancel.FlatAppearance.BorderSize = 1; btnCancel.FlatAppearance.BorderColor = SystemColors.ControlDark; btnCancel.Margin = new Padding(6);
            btnSendCommand.FlatStyle = FlatStyle.Flat; btnSendCommand.FlatAppearance.BorderSize = 1; btnSendCommand.FlatAppearance.BorderColor = SystemColors.ControlDark; btnSendCommand.Margin = new Padding(6);
            btnCopyText.FlatStyle = FlatStyle.Flat; btnCopyText.FlatAppearance.BorderSize = 1; btnCopyText.FlatAppearance.BorderColor = SystemColors.ControlDark; btnCopyText.Margin = new Padding(6);
            btnSearchWeb.FlatStyle = FlatStyle.Flat; btnSearchWeb.FlatAppearance.BorderSize = 1; btnSearchWeb.FlatAppearance.BorderColor = SystemColors.ControlDark; btnSearchWeb.Margin = new Padding(6);

            this.Controls.Add(txtInput);
            this.Controls.Add(bottomPanel);

            this.Text = "Voice Dictation";
            this.StartPosition = FormStartPosition.CenterScreen;
            // Increase width by 20% (900 -> 1080) to give more room for text
            this.Size = new Size(1080, 400);

            btnCancel.Click += BtnCancel_Click;
            btnStart.Click += BtnStart_Click;
            btnSendCommand.Click += BtnSendCommand_Click;
            btnCopyText.Click += BtnCopyText_Click;
            btnSearchWeb.Click += BtnSearchWeb_Click;
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

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnSendCommand_Click(object? sender, EventArgs e)
        {
            // Explicit send command - behaves like Submit but kept as a distinct action
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCopyText_Click(object? sender, EventArgs e)
        {
            try
            {
                var text = txtInput.Text ?? string.Empty;
                if (!string.IsNullOrEmpty(text)) Clipboard.SetText(text);
            }
            catch { }
        }

        private void BtnSearchWeb_Click(object? sender, EventArgs e)
        {
            try
            {
                var text = txtInput.Text ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text)) return;
                var query = Uri.EscapeDataString(text);
                var url = $"https://www.bing.com/search?q={query}";

                var psi = new ProcessStartInfo
                {
                    FileName = "msedge",
                    Arguments = url,
                    UseShellExecute = true
                };

                try
                {
                    Process.Start(psi);
                }
                catch
                {
                    // Fallback: open with default browser
                    var psi2 = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };
                    Process.Start(psi2);
                }
            }
            catch { }
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
