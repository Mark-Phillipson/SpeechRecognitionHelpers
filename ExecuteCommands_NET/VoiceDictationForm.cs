using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;

namespace DictationBoxMSP
{
    public class VoiceDictationForm : Form
    {
        private TextBox txtInput = null!;
        private Button btnCancel = null!;
        // removed Restart button (use voice phrase "voice typing" to start)
        private Button btnSendCommand = null!;
        private Button btnCopyText = null!;
        private Button btnSearchWeb = null!;
        private Button btnOpenInVsc = null!;
        private Button btnToggleTransparent = null!;
        private Panel bottomPanel = null!;
        private Label lblTransient = null!;
        private List<Label> marqueeLabels = new List<Label>();
        private System.Windows.Forms.Timer marqueeTimer = null!;
        private System.Windows.Forms.Timer autoSubmitTimer = null!;
        private System.Windows.Forms.Timer startDictationTimer = null!;
        private int timeoutMs = 0;
        private bool isBackgroundTransparent = false;
        // removed unused saved colors to avoid compiler warnings
        private Color savedTxtInputBackColor;
        private Color savedBottomPanelBackColor;
        private double savedOpacity = 1.0;

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
            // Re-Start Dictation button removed (use voice phrase instead)
            this.btnCancel = new Button() { Text = "&Cancel", Height = 121 };
            // Use Alt+S for Send Command (replaces the removed Submit button)
            this.btnSendCommand = new Button() { Text = "&Send Command", Height = 121 };
            this.btnCopyText = new Button() { Text = "Copy &Text", Height = 121 };
            this.btnSearchWeb = new Button() { Text = "Search &Web", Height = 121 };
            this.btnOpenInVsc = new Button() { Text = "Open in &VS Code", Height = 121 };
            this.autoSubmitTimer = new System.Windows.Forms.Timer();
            this.startDictationTimer = new System.Windows.Forms.Timer();

            // Bottom area: use a table so marquee cannot overlap buttons.
            bottomPanel = new Panel() { Dock = DockStyle.Bottom, Height = 140 };
            bottomPanel.Padding = new Padding(8);
            bottomPanel.BackColor = DisplayMessage.SharedBackColor;

            // We'll use a 3-row TableLayout: marquee (36px), buttons (84px), transient (20px)
            var table = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 84));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));

            // Marquee row (top of bottomPanel)
            var marqueePanel = new Panel() { Dock = DockStyle.Fill, Height = 28 };
            marqueePanel.Padding = new Padding(6, 6, 6, 6);
            marqueePanel.BackColor = DisplayMessage.SharedBackColor;

            // Create multiple marquee labels so we can show more than one message at once
            int marqueeCount = 2;
            for (int i = 0; i < marqueeCount; i++)
            {
                var lbl = new Label()
                {
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = DisplayMessage.SharedForeColor,
                    BackColor = Color.Transparent,
                    Font = new Font(this.Font.FontFamily, Math.Max(this.Font.Size - 6f, 10f), FontStyle.Regular)
                };
                marqueePanel.Controls.Add(lbl);
                marqueeLabels.Add(lbl);
            }

            // Buttons row (middle)
            var buttonsContainer = new Panel() { Dock = DockStyle.Fill };
            var flow = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, AutoSize = false };
            flow.WrapContents = false;
            flow.Padding = new Padding(6);
            // shorten button heights so marquee and buttons are visible together
            var btnHeight = 64;
            this.btnCancel.Height = btnHeight;
            this.btnSendCommand.Height = btnHeight;
            this.btnCopyText.Height = btnHeight;
            this.btnSearchWeb.Height = btnHeight;
            this.btnOpenInVsc.Height = btnHeight;
            this.btnToggleTransparent = new Button() { Text = "Toggle Trans&parent", Height = btnHeight };

            flow.Controls.Add(btnCancel);
            flow.Controls.Add(btnSendCommand);
            flow.Controls.Add(btnCopyText);
            flow.Controls.Add(btnOpenInVsc);
            flow.Controls.Add(btnToggleTransparent);
            flow.Controls.Add(btnSearchWeb);

            // Keep explicit widths for remaining buttons so text remains visible
            btnCancel.Width = 140; btnSendCommand.Width = 170; btnCopyText.Width = 160; btnSearchWeb.Width = 160; btnOpenInVsc.Width = 180; btnToggleTransparent.Width = 180;

            // Make button fonts slightly larger so text is easier to read
            try
            {
                var baseSize = Math.Max(this.Font.Size * 0.85f, 10f);
                var btnFont = new Font(this.Font.FontFamily, baseSize + 1f, FontStyle.Regular);
                btnCancel.Font = btnFont;
                btnSendCommand.Font = btnFont;
                btnCopyText.Font = btnFont;
                btnSearchWeb.Font = btnFont;
                btnOpenInVsc.Font = btnFont;
                btnToggleTransparent.Font = btnFont;
            }
            catch { }

            buttonsContainer.Controls.Add(flow);

            // Transient row (bottom of bottomPanel)
            lblTransient = new Label()
            {
                Dock = DockStyle.Fill,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
                BackColor = Color.LimeGreen,
                ForeColor = Color.Black,
                Font = new Font(this.Font.FontFamily, Math.Max(this.Font.Size - 2f, 10f), FontStyle.Bold),
                Padding = new Padding(4)
            };

            table.Controls.Add(marqueePanel, 0, 0);
            table.Controls.Add(buttonsContainer, 0, 1);
            // lblTransient already created above and configured when building the table
            table.Controls.Add(lblTransient, 0, 2);

            bottomPanel.Controls.Add(table);

            // Start marquee behavior: scroll multiple labels across the panel
            try
            {
                var marqueeItems = LoadMarqueeItems();
                var rnd = new Random();
                marqueeTimer = new System.Windows.Forms.Timer();
                marqueeTimer.Interval = 28;
                marqueeTimer.Tick += (s, e) =>
                {
                    try
                    {
                        foreach (var lbl in marqueeLabels.ToList())
                        {
                            if (lbl.Left + lbl.Width <= -10)
                            {
                                var text = marqueeItems.Count > 0 ? marqueeItems[rnd.Next(marqueeItems.Count)] : "Say 'voice typing' to begin";
                                lbl.Text = text;
                                // place this label to the right of the right-most label (or panel if none)
                                int rightmost = marqueePanel.Width;
                                foreach (var other in marqueeLabels)
                                {
                                    if (other == lbl) continue;
                                    rightmost = Math.Max(rightmost, other.Left + other.Width + 40);
                                }
                                lbl.Left = rightmost;
                                lbl.Top = (marqueePanel.Height - lbl.Height) / 2;
                            }
                            else
                            {
                                lbl.Left -= 2;
                            }
                        }
                    }
                    catch { }
                };

                this.Shown += (s, e) =>
                {
                    try
                    {
                        for (int i = 0; i < marqueeLabels.Count; i++)
                        {
                            var lbl = marqueeLabels[i];
                            if (string.IsNullOrEmpty(lbl.Text))
                            {
                                lbl.Text = marqueeItems.Count > 0 ? marqueeItems[rnd.Next(marqueeItems.Count)] : "Say 'voice typing' to begin";
                            }
                            // stagger starting positions so messages are spread out
                            lbl.Left = marqueePanel.Width + i * (marqueePanel.Width / 2);
                            lbl.Top = (marqueePanel.Height - lbl.Height) / 2;
                        }
                        marqueeTimer?.Start();
                    }
                    catch { }
                };
            }
            catch { }

            // Make button borders visible on all sides by using FlatStyle and a small border
            btnCancel.FlatStyle = FlatStyle.Flat; btnCancel.FlatAppearance.BorderSize = 1; btnCancel.FlatAppearance.BorderColor = SystemColors.ControlDark; btnCancel.Margin = new Padding(6);
            btnSendCommand.FlatStyle = FlatStyle.Flat; btnSendCommand.FlatAppearance.BorderSize = 1; btnSendCommand.FlatAppearance.BorderColor = SystemColors.ControlDark; btnSendCommand.Margin = new Padding(6);
            btnCopyText.FlatStyle = FlatStyle.Flat; btnCopyText.FlatAppearance.BorderSize = 1; btnCopyText.FlatAppearance.BorderColor = SystemColors.ControlDark; btnCopyText.Margin = new Padding(6);
            btnOpenInVsc.FlatStyle = FlatStyle.Flat; btnOpenInVsc.FlatAppearance.BorderSize = 1; btnOpenInVsc.FlatAppearance.BorderColor = SystemColors.ControlDark; btnOpenInVsc.Margin = new Padding(6);
            btnSearchWeb.FlatStyle = FlatStyle.Flat; btnSearchWeb.FlatAppearance.BorderSize = 1; btnSearchWeb.FlatAppearance.BorderColor = SystemColors.ControlDark; btnSearchWeb.Margin = new Padding(6);
            btnToggleTransparent.FlatStyle = FlatStyle.Flat; btnToggleTransparent.FlatAppearance.BorderSize = 1; btnToggleTransparent.FlatAppearance.BorderColor = SystemColors.ControlDark; btnToggleTransparent.Margin = new Padding(6);

            // Provide tooltips and ensure mnemonics are enabled so shortcuts are discoverable
            try
            {
                var tt = new ToolTip();
                tt.IsBalloon = false;
                tt.ShowAlways = true;
                tt.SetToolTip(btnToggleTransparent, "Toggle Transparent (Alt+P)");
                tt.SetToolTip(btnCopyText, "Copy text to clipboard (Alt+T)");
                // Ensure mnemonics are enabled explicitly
                btnToggleTransparent.UseMnemonic = true;
                btnCopyText.UseMnemonic = true;
            }
            catch { }

            this.Controls.Add(txtInput);
            this.Controls.Add(bottomPanel);

            this.Text = "Voice Dictation";
            this.StartPosition = FormStartPosition.CenterScreen;
            // Make the form a little wider and twice as high so buttons don't get cut off
            this.Size = new Size(1200, 800);

            btnCancel.Click += BtnCancel_Click;
            // start button removed; dictation triggered via voice phrase
            btnSendCommand.Click += BtnSendCommand_Click;
            btnCopyText.Click += BtnCopyText_Click;
            btnOpenInVsc.Click += BtnOpenInVsc_Click;
            btnToggleTransparent.Click += BtnToggleTransparent_Click;
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
                var larger = new Font(baseFont!.FontFamily, Math.Max(baseFont!.Size * 1.8f, baseFont!.Size + 8f), baseFont!.Style);
                this.Font = larger;
                txtInput.Font = larger;
                txtInput.BackColor = DisplayMessage.SharedBackColor;
                txtInput.ForeColor = DisplayMessage.SharedForeColor;
                try { if (bottomPanel != null) bottomPanel.BackColor = DisplayMessage.SharedBackColor; } catch { }
            }
            catch { }
        }

        private void BtnToggleTransparent_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!isBackgroundTransparent)
                {
                    // save current opacity and colors
                    savedOpacity = this.Opacity;
                    savedTxtInputBackColor = txtInput.BackColor;
                    savedBottomPanelBackColor = bottomPanel.BackColor;

                    // set a semi-transparent window (affects controls too)
                    this.Opacity = 0.65;

                    // keep control backgrounds readable
                    txtInput.BackColor = DisplayMessage.SharedBackColor;
                    bottomPanel.BackColor = DisplayMessage.SharedBackColor;

                    isBackgroundTransparent = true;
                    // Keep the same mnemonic (Trans&parent -> Alt+P) for the disabled state
                    btnToggleTransparent.Text = "Disable Trans&parent";
                }
                else
                {
                    // restore
                    this.Opacity = savedOpacity;
                    txtInput.BackColor = savedTxtInputBackColor;
                    bottomPanel.BackColor = savedBottomPanelBackColor;
                    isBackgroundTransparent = false;
                    btnToggleTransparent.Text = "Toggle Trans&parent";
                }
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

        // BtnStart removed — StartDictation can still be invoked by voice phrase or timers

        private void StartDictation()
        {
            try
            {
                txtInput.Focus();
                txtInput.Select();
                var sim = new InputSimulator();
                // Make sure modifier keys (Alt/Ctrl/Shift) are released before simulating Win+H.
                // When the user activates the button via an access key (Alt+Key) the Alt key
                // may still be logically down which can interfere with the Win+H keystroke.
                try
                {
                    sim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.MENU);
                    sim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
                    sim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
                }
                catch { }

                // Give the input system a moment to settle after releasing modifiers
                System.Threading.Thread.Sleep(60);

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

        private async void BtnCopyText_Click(object? sender, EventArgs e)
        {
            try
            {
                var text = txtInput.Text ?? string.Empty;
                if (string.IsNullOrEmpty(text)) return;

                Clipboard.SetText(text);

                try
                {
                    // Stronger visual feedback: change text, colors and force a refresh so it's obvious
                    var originalText = btnCopyText.Text;
                    var originalBack = btnCopyText.BackColor;
                    var originalFore = btnCopyText.ForeColor;
                    var originalFont = btnCopyText.Font;

                    btnCopyText.Enabled = false;
                    btnCopyText.Text = "Copied";
                    // Ensure BackColor will be applied even when visual styles are enabled
                    var originalUseVisual = btnCopyText.UseVisualStyleBackColor;
                    try { btnCopyText.UseVisualStyleBackColor = false; } catch { }
                    btnCopyText.BackColor = Color.LimeGreen;
                    btnCopyText.ForeColor = Color.Black;
                    btnCopyText.Font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Bold);
                    btnCopyText.Refresh();

                    // Show the transient label as a very visible confirmation below the buttons
                    try
                    {
                        if (lblTransient != null)
                        {
                            lblTransient.Text = "Copied";
                            lblTransient.BackColor = Color.LimeGreen;
                            lblTransient.ForeColor = Color.Black;
                            lblTransient.Visible = true;
                            lblTransient.BringToFront();
                        }
                    }
                    catch { }

                    await Task.Delay(1250);

                    btnCopyText.Text = originalText;
                    btnCopyText.BackColor = originalBack;
                    btnCopyText.ForeColor = originalFore;
                    btnCopyText.Font = originalFont;
                    try { btnCopyText.UseVisualStyleBackColor = originalUseVisual; } catch { }
                    btnCopyText.Enabled = true;
                    btnCopyText.Refresh();

                    // Hide the transient label after restoring button state
                    try
                    {
                        if (lblTransient != null)
                        {
                            lblTransient.Visible = false;
                        }
                    }
                    catch { }
                }
                catch { }

                try { ExecuteCommands.TrayNotificationHelper.ShowNotification("Copied", "Text copied to clipboard", 1200); } catch { }
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

        private List<string> LoadMarqueeItems()
        {
            // Curated United Kingdom English voice-typing commands (UK variants only)
            return new List<string>
            {
                "Punctuation: say 'full stop' Note this will also stop Voice Typing and go back to Talon Voice",
                "New line: say 'new line'",
                "New paragraph: say 'new paragraph'",
                "Open quote / Close quote: say 'open quote' / 'close quote'",
                "Colon / Semicolon: say 'colon' / 'semicolon'",
                "Ellipsis: say 'ellipsis'",
                "Open parenthesis / Close parenthesis: say 'open parenthesis' / 'close parenthesis'",
                "Delete last spoken word/phrase: say 'delete that' to remove last phrase",
                "Select last spoken word or phrase: say 'select that'",
                "Press enter: say 'press enter'",
                "Press Backspace: say 'press backspace'",
                "Press Tab: say 'press tab'",
                "Press Space: say 'press space'",
                "Say 'voice typing' to open Windows Voice Typing and set Talon Voice to sleep"
            };
        }

        private void BtnOpenInVsc_Click(object? sender, EventArgs e)
        {
            try
            {
                var text = txtInput.Text ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text)) return;

                var tempDir = Path.GetTempPath();
                var fileName = $"dictation-{DateTime.Now:yyyyMMdd-HHmmss}.txt";
                var filePath = Path.Combine(tempDir, fileName);
                File.WriteAllText(filePath, text);

                var args = $"--new-window \"{filePath}\"";

                // Try the 'code' CLI first
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "code",
                        Arguments = args,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                    return;
                }
                catch { }

                // Try common install locations
                var possible = new[]
                {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft VS Code", "Code.exe"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft VS Code", "Code.exe")
                };

                foreach (var p in possible)
                {
                    if (File.Exists(p))
                    {
                        var psi2 = new ProcessStartInfo
                        {
                            FileName = p,
                            Arguments = args,
                            UseShellExecute = true
                        };
                        Process.Start(psi2);
                        return;
                    }
                }

                // Fallback: open with default program
                Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
            }
            catch { }
        }
    }
}
