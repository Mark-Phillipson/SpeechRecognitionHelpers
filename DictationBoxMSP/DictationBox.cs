using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    }
}
