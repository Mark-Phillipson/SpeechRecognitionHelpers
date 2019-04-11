using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictationBoxMSP
{
    public partial class DictationBoxForm : Form
    {
        public DictationBoxForm()
        {
            InitializeComponent();
        }

        private void TransferButton_Click(object sender, EventArgs e)
        {
            PerformTransfer();
        }

        private void PerformTransfer()
        {
            if (richTextBox1.SelectionLength == 0)
            {
                richTextBox1.SelectAll();
            }

            Task.Delay(50).Wait();

            richTextBox1.ForeColor = Color.Azure;

            Task.Delay(50).Wait();
            richTextBox1.Copy();
            Task.Delay(50).Wait();
            Clipboard.GetText();

            SendKeys.Send("%{Tab}");
            Task.Delay(200).Wait();
            SendKeys.Send("^v");
            Task.Delay(100).Wait();

            Application.Exit();
        }

        private void CamelButton_Click(object sender, EventArgs e)
        {
            IEnumerable<string> words = GetWords(richTextBox1.Text);
            var counter = 0;
            var value = "";
            foreach (var word in words)
            {
                counter++;
                if (counter == 1)
                {
                    value = word.ToLower();
                }
                else
                {
                    value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
                }
            }
            richTextBox1.Text = value;
            PerformTransfer();
        }

        private IEnumerable<string> GetWords(string text)
        {
            MatchCollection matches = Regex.Matches(text, @"\b[\w-]*\b");
            var words = from match in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(match.Value)
                        select match.Value;
            return words;
        }

        private void VariableButton_Click(object sender, EventArgs e)
        {
            IEnumerable<string> words = GetWords(richTextBox1.Text);
            var value = "";
            foreach (var word in words)
            {
                value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
            }
            richTextBox1.Text = value;
            PerformTransfer();
        }

        private void WindowButton_Click(object sender, EventArgs e)
        {
            if (Opacity==1)
            {
                this.Opacity = 0.5;
            }
            else
            {
                this.Opacity = 1;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.google.com/search?client=firefox-b-d&q=" + richTextBox1.Text);
        }
        public int FindMyText(string searchText, int searchStart, int searchEnd)
        {
            // Initialize the return value to false by default.
            int returnValue = -1;

            // Ensure that a search string and a valid starting point are specified.
            if (searchText.Length > 0 && searchStart >= 0)
            {
                // Ensure that a valid ending value is provided.
                if (searchEnd > searchStart || searchEnd == -1)
                {
                    // Obtain the location of the search string in richTextBox1.
                    int indexToText = richTextBox1.Find(searchText, searchStart, searchEnd, RichTextBoxFinds.MatchCase);
                    // Determine whether the text was found in richTextBox1.
                    if (indexToText >= 0)
                    {
                        // Return the index to the specified search text.
                        returnValue = indexToText;
                    }
                }
            }

            return returnValue;
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            if ( FindtextBox.Text== null  || FindtextBox.Text.Length==0)
            {
                return;
            }
            var searchFrom = 0;
            var position = 0;
            var successfulFinds = 0;
            while (position>=0)
            {

                position = FindMyText(FindtextBox.Text, searchFrom, richTextBox1.Text.Length);
                if (position>=0)
                {
                    successfulFinds++;
                    searchFrom = position + 1;
                    richTextBox1.SelectionStart = position;
                    richTextBox1.SelectionLength = FindtextBox.Text.Length;
                    richTextBox1.SelectionBackColor = Color.Red;
                }
            }
        }

        private void ReplaceButton_Click(object sender, EventArgs e)
        {
            if (FindtextBox.Text== null  || ReplaceTextBox.Text== null  || richTextBox1.Text== null  || FindtextBox.Text.Length==0 || ReplaceTextBox.Text.Length==0 || richTextBox1.Text.Length==0)
            {
                return;
            }
            var text = richTextBox1.Text;
            text=text.Replace(FindtextBox.Text, ReplaceTextBox.Text);
            richTextBox1.Text = text;
        }

        private void CopyOnlyButton_Click(object sender, EventArgs e)
        {
            string text = richTextBox1.Text;
            Clipboard.SetText(text);
            Application.Exit();
        }
    }
}
