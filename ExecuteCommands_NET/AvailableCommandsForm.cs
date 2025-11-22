using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DictationBoxMSP
{
    public class AvailableCommandsForm : Form
    {
        private TextBox txtSearch = null!;
        private ListBox lstResults = null!;
        private Label lblHint = null!;

        public AvailableCommandsForm()
        {
            InitializeComponents();
            ApplySharedStyles();
            Load += AvailableCommandsForm_Load;
            Shown += AvailableCommandsForm_Shown;
        }

        private void InitializeComponents()
        {
            this.txtSearch = new TextBox() { Dock = DockStyle.Top, Margin = new Padding(8), Height = (int)(28 * 1.2) };
            this.lstResults = new ListBox() { Dock = DockStyle.Fill }; 
            this.lblHint = new Label() { Dock = DockStyle.Top, Height = 20, Text = "Start typing to filter commands...", TextAlign = System.Drawing.ContentAlignment.MiddleLeft };

            this.Controls.Add(lstResults);
            this.Controls.Add(lblHint);
            this.Controls.Add(txtSearch);

            this.Text = "Available Commands";
            this.StartPosition = FormStartPosition.CenterScreen;
            // Make the form bigger for easier reading
            this.Size = new Size(900, 700);

            txtSearch.TextChanged += TxtSearch_TextChanged;
            lstResults.DoubleClick += LstResults_DoubleClick;
            // Ensure the textbox can receive focus/tab stop
            txtSearch.TabIndex = 0;
            txtSearch.TabStop = true;
            txtSearch.ReadOnly = false;
            txtSearch.Enabled = true;
        }

        private void ApplySharedStyles()
        {
            this.BackColor = DisplayMessage.SharedBackColor;
            this.ForeColor = DisplayMessage.SharedForeColor;

            // Increase font size for accessibility (40% larger than shared font)
            float baseSize = DisplayMessage.SharedFont?.Size ?? SystemFonts.MessageBoxFont.Size;
            float largerSize = Math.Max(baseSize * 1.4f, baseSize + 4f);
            var largerFont = new Font(DisplayMessage.SharedFont?.FontFamily ?? SystemFonts.MessageBoxFont.FontFamily, largerSize, DisplayMessage.SharedFont?.Style ?? SystemFonts.MessageBoxFont.Style);
            this.Font = largerFont;

            txtSearch.BackColor = ControlPaint.Dark(DisplayMessage.SharedBackColor);
            txtSearch.ForeColor = DisplayMessage.SharedForeColor;
            txtSearch.Font = largerFont;

            lstResults.BackColor = DisplayMessage.SharedBackColor;
            lstResults.ForeColor = DisplayMessage.SharedForeColor;
            lstResults.Font = largerFont;

            lblHint.BackColor = DisplayMessage.SharedBackColor;
            lblHint.ForeColor = Color.LightGray;
            lblHint.Font = largerFont;
        }

        private void AvailableCommandsForm_Load(object? sender, EventArgs e)
        {
            // Start with no results for performance; user begins typing to populate.
            lstResults.Items.Clear();
            // Ensure layout order: bring search to front
            txtSearch.BringToFront();
            lblHint.BringToFront();
        }

        private void AvailableCommandsForm_Shown(object? sender, EventArgs e)
        {
            // Give keyboard focus to the search textbox when the form appears
            try
            {
                this.ActiveControl = txtSearch;
                txtSearch.Focus();
                txtSearch.Select();
            }
            catch { }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            var q = txtSearch.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(q))
            {
                lblHint.Text = "Start typing to filter commands...";
                lstResults.Items.Clear();
                return;
            }

            lblHint.Text = string.Empty;

            // Aggregate available commands from interpreter's public lists
            var items = new List<(string Command, string Description)>();
            try
            {
                items.AddRange(ExecuteCommands.NaturalLanguageInterpreter.AvailableCommands);
            }
            catch { }
            try
            {
                items.AddRange(ExecuteCommands.NaturalLanguageInterpreter.VisualStudioCommands);
            }
            catch { }
            try
            {
                items.AddRange(ExecuteCommands.NaturalLanguageInterpreter.VSCodeCommands);
            }
            catch { }

            var filtered = items
                .Where(i => i.Command.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 || (i.Description ?? string.Empty).IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(i =>
                {
                    try
                    {
                        var emoji = ExecuteCommands.NaturalLanguageInterpreter.GetCommandEmoji(i.Command);
                        if (!string.IsNullOrEmpty(emoji))
                            return $"{emoji} {i.Command} — {i.Description}";
                    }
                    catch { }
                    return $"{i.Command} — {i.Description}";
                })
                .Take(200)
                .ToArray();

            // Also include any configured emoji mappings that match the query (show name -> emoji)
            try
            {
                var mappings = ExecuteCommands.NaturalLanguageInterpreter.GetAllEmojiMappings();
                var mappingMatches = mappings
                    .Where(m => (m.Name ?? string.Empty).IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 || (m.Emoji ?? string.Empty).IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(m => $"{m.Emoji} {m.Name} — Emoji mapping")
                    .ToArray();
                // Prepend mapping results so they are visible first
                if (mappingMatches.Length > 0)
                {
                    var combined = new string[mappingMatches.Length + filtered.Length];
                    mappingMatches.CopyTo(combined, 0);
                    filtered.CopyTo(combined, mappingMatches.Length);
                    filtered = combined;
                }
            }
            catch { }

            lstResults.BeginUpdate();
            lstResults.Items.Clear();
            lstResults.Items.AddRange(filtered);
            lstResults.EndUpdate();
        }

        private void LstResults_DoubleClick(object? sender, EventArgs e)
        {
            if (lstResults.SelectedItem == null) return;
            // For now, copy the selected command text to clipboard to let the user paste or use it.
            Clipboard.SetText(lstResults.SelectedItem.ToString() ?? string.Empty);
            MessageBox.Show("Command copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
