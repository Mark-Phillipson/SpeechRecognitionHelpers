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
            // Move the hint label to the bottom so it doesn't overlap top items
            this.lblHint = new Label() { Dock = DockStyle.Bottom, Height = 22, Text = "Start typing to filter commands...", TextAlign = System.Drawing.ContentAlignment.MiddleLeft };

            // Add controls in an order that yields predictable docking without needing BringToFront
            // Add hint (bottom), then list (fill), then search (top)
            this.Controls.Add(lblHint);
            this.Controls.Add(lstResults);
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
            var sharedFont = DisplayMessage.SharedFont;
            float baseSize = SystemFonts.MessageBoxFont?.Size ?? 12f;
            FontFamily fontFamily = SystemFonts.MessageBoxFont?.FontFamily ?? FontFamily.GenericSansSerif;
            FontStyle fontStyle = SystemFonts.MessageBoxFont?.Style ?? FontStyle.Regular;
            if (sharedFont != null)
            {
                baseSize = sharedFont.Size;
                fontFamily = sharedFont.FontFamily ?? fontFamily;
                fontStyle = sharedFont.Style;
            }
            float largerSize = Math.Max(baseSize * 1.4f, baseSize + 4f);
            var largerFont = new Font(fontFamily, largerSize, fontStyle);
            this.Font = largerFont;

            if (txtSearch != null)
            {
                txtSearch.BackColor = ControlPaint.Dark(DisplayMessage.SharedBackColor);
                txtSearch.ForeColor = DisplayMessage.SharedForeColor;
                txtSearch.Font = largerFont;
            }

            if (lstResults != null)
            {
                lstResults.BackColor = DisplayMessage.SharedBackColor;
                lstResults.ForeColor = DisplayMessage.SharedForeColor;
                lstResults.Font = largerFont;
                // Avoid the ListBox clipping its first/last item by disabling IntegralHeight
                lstResults.IntegralHeight = false;
                // Use normal drawing mode to avoid owner-draw inconsistencies
                lstResults.DrawMode = DrawMode.Normal;
                // Set a generous item height based on the chosen font to avoid clipping
                try { lstResults.ItemHeight = (int)Math.Ceiling(largerFont.GetHeight()) + 8; } catch { }
                lstResults.BorderStyle = BorderStyle.FixedSingle;
            }

            if (lblHint != null)
            {
                lblHint.BackColor = DisplayMessage.SharedBackColor;
                lblHint.ForeColor = Color.LightGray;
                lblHint.Font = largerFont;
            }
        }

        private void AvailableCommandsForm_Load(object? sender, EventArgs e)
        {
            // Start with no results for performance; user begins typing to populate.
            lstResults.Items.Clear();
            // No BringToFront calls — control docking/order set in InitializeComponents
        }

        private void AvailableCommandsForm_Shown(object? sender, EventArgs e)
        {
            // Give keyboard focus to the search textbox when the form appears
            try
            {
                this.ActiveControl = txtSearch;
                txtSearch.Focus();
                txtSearch.Select();
                // Populate with a random sample of commands so the user sees examples immediately
                PopulateRandomSampleItems(20);
            }
            catch { }
        }

        private void PopulateRandomSampleItems(int sampleSize)
        {
            try
            {
                var items = new List<(string Command, string Description)>();
                try { items.AddRange(ExecuteCommands.NaturalLanguageInterpreter.AvailableCommands); } catch { }
                try { items.AddRange(ExecuteCommands.NaturalLanguageInterpreter.VisualStudioCommands); } catch { }
                try { items.AddRange(ExecuteCommands.NaturalLanguageInterpreter.VSCodeCommands); } catch { }

                // Deduplicate by Command+Description
                var distinct = items
                    .Where(i => !string.IsNullOrEmpty(i.Command))
                    .GroupBy(i => (i.Command ?? string.Empty) + "|" + (i.Description ?? string.Empty))
                    .Select(g => g.First())
                    .ToList();

                if (distinct.Count == 0)
                {
                    lstResults.Items.Clear();
                    return;
                }

                var rng = new Random();
                var sample = distinct.OrderBy(_ => rng.Next()).Take(sampleSize).Select(i =>
                {
                    try
                    {
                        var emoji = ExecuteCommands.EmojiManager.GetCommandEmoji(i.Command);
                        if (!string.IsNullOrEmpty(emoji))
                            return $"{emoji} {i.Command} — {i.Description}";
                    }
                    catch { }
                    return $"{i.Command} — {i.Description}";
                }).ToArray();

                lstResults.BeginUpdate();
                lstResults.Items.Clear();
                lstResults.Items.AddRange(sample);
                lstResults.EndUpdate();
            }
            catch { }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            var q = txtSearch.Text?.Trim() ?? string.Empty;
            // Only start searching when the user has entered 3 or more characters.
            if (q.Length < 3)
            {
                lblHint.Text = "Type 3+ characters to search...";
                // Keep the list empty until sufficient input is provided
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
            // Diagnostic logging: write combined items count and presence of 'natural dictate'
            try
            {
                var logPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log"));
                var found = items.Any(i => string.Equals(i.Command?.Trim(), "natural dictate", StringComparison.OrdinalIgnoreCase));
                System.IO.File.AppendAllText(logPath, $"[DEBUG] AvailableCommandsForm: combined items count={items.Count}, contains 'natural dictate'={found}\n");
            }
            catch { }

            // Log control bounds so we can see if the ListBox is overlapped/offset
            try
            {
                var logPath2 = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log"));
                System.IO.File.AppendAllText(logPath2, $"[DEBUG] AvailableCommandsForm.Bounds: Form.ClientSize={this.ClientSize}, txtSearch.Bounds={txtSearch.Bounds}, lblHint.Bounds={lblHint.Bounds}, lstResults.Bounds={lstResults.Bounds}\n");
            }
            catch { }

            // More flexible matching: split the query into terms and require all terms
            // to be present in either the command or the description (order-insensitive).
            // Tokenize and only keep terms of length >= 3 to avoid matching on tiny fragments
            var terms = q.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().ToLowerInvariant())
                .Where(s => s.Length >= 3)
                .ToArray();

            if (terms.Length == 0)
            {
                lblHint.Text = "Type 3+ characters to search...";
                lstResults.Items.Clear();
                return;
            }

            // Diagnostic logging: query terms
            try
            {
                var logPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log"));
                System.IO.File.AppendAllText(logPath, $"[DEBUG] AvailableCommandsForm: query='{q}', terms=[{string.Join(',', terms)}]\n");
            }
            catch { }

            var filtered = items
                .Where(i =>
                {
                    var hay = (i.Command + " " + (i.Description ?? string.Empty)).ToLowerInvariant();
                    return terms.All(t => hay.Contains(t));
                })
                .Select(i =>
                {
                    try
                    {
                        var emoji = ExecuteCommands.EmojiManager.GetCommandEmoji(i.Command);
                        if (!string.IsNullOrEmpty(emoji))
                            return $"{emoji} {i.Command} — {i.Description}";
                    }
                    catch { }
                    return $"{i.Command} — {i.Description}";
                })
                // Limit results to 4 items for quick, focused suggestions
                .Take(4)
                .ToArray();

            // Also include any configured emoji mappings that match the query (show name -> emoji)
            try
            {
                var mappings = ExecuteCommands.EmojiManager.GetAllEmojiMappings();
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

            // Diagnostic logging: filtered count
            try
            {
                var logPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log"));
                System.IO.File.AppendAllText(logPath, $"[DEBUG] AvailableCommandsForm: filtered count={filtered.Length}\n");
            }
            catch { }

            lstResults.BeginUpdate();
            lstResults.Items.Clear();
            lstResults.Items.AddRange(filtered);
            // Diagnostic: log the actual strings being added so we can confirm visibility
            try
            {
                var logPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log"));
                System.IO.File.AppendAllText(logPath, $"[DEBUG] AvailableCommandsForm: adding filtered items: {string.Join(" | ", filtered)}\n");
            }
            catch { }
            // Ensure colors are explicit so items are visible regardless of shared theme
            try { lstResults.BackColor = Color.Black; } catch { }
            try { lstResults.ForeColor = Color.White; } catch { }
            try { lstResults.Visible = true; lstResults.Refresh(); lstResults.Invalidate(); lstResults.Update(); } catch { }
            try
            {
                var logPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log"));
                System.IO.File.AppendAllText(logPath, $"[DEBUG] AvailableCommandsForm: ListBox.Items.Count={lstResults.Items.Count}\n");
            }
            catch { }
            try
            {
                if (lstResults.Items.Count > 0)
                {
                    lstResults.SelectedIndex = 0; // select first to make it visible
                    // Ensure the selected index is scrolled into view
                    lstResults.TopIndex = Math.Max(0, lstResults.SelectedIndex);
                    System.IO.File.AppendAllText(System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log")), $"[DEBUG] AvailableCommandsForm: SelectedIndex={lstResults.SelectedIndex}, TopIndex={lstResults.TopIndex}\n");
                }
            }
            catch { }
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
