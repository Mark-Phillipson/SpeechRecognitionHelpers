using SpeechRecognitionHelpersLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            if (Opacity == 1)
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
            string searchString;
            if (richTextBox1.SelectedText != null && richTextBox1.SelectedText.Length > 0)
            {
                searchString = richTextBox1.SelectedText;
            }
            else
            {
                searchString = richTextBox1.Text;
            }
            searchString = searchString.Replace("C#", "CSharp");
            searchString = searchString.Replace("c#", "CSharp");

            Process.Start("https://www.google.com/search?client=firefox-b-d&q=" + searchString);
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
            SearchAndIdentifyText();
        }

        private void SearchAndIdentifyText()
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.Black;
            if (FindtextBox.Text == null || FindtextBox.Text.Length == 0)
            {
                return;
            }
            var searchFrom = 0;
            var position = 0;
            var successfulFinds = 0;
            while (position >= 0)
            {
                position = FindMyText(FindtextBox.Text, searchFrom, richTextBox1.Text.Length);
                if (position >= 0)
                {
                    successfulFinds++;
                    searchFrom = position + 1;
                    richTextBox1.SelectionStart = position;
                    richTextBox1.SelectionLength = FindtextBox.Text.Length;
                    richTextBox1.SelectionBackColor = Color.Red;
                }
            }
            richTextBox1.DeselectAll();
        }

        private void ReplaceButton_Click(object sender, EventArgs e)
        {
            if (FindtextBox.Text == null || ReplaceTextBox.Text == null || richTextBox1.Text == null || FindtextBox.Text.Length == 0 || ReplaceTextBox.Text.Length == 0 || richTextBox1.Text.Length == 0)
            {
                return;
            }
            var text = richTextBox1.Text;
            text = text.Replace(FindtextBox.Text, ReplaceTextBox.Text);
            richTextBox1.Text = text;
        }

        private void CopyOnlyButton_Click(object sender, EventArgs e)
        {
            if (richTextBox1?.Text != null && richTextBox1.Text.Length > 0)
            {
                string text = richTextBox1.Text;
                Clipboard.SetText(text);
            }
            Application.Exit();
        }

        private void ButtonFrontSize_Click(object sender, EventArgs e)
        {
            float currentSize = richTextBox1.Font.Size;
            if (currentSize == 21.75)
            {
                currentSize -= 10.0f;
            }
            else
            {
                currentSize += 10.0f;
            }
            richTextBox1.Font = new Font(richTextBox1.Font.Name, currentSize, richTextBox1.Font.Style);
        }

        private void DictationBoxForm_Load(object sender, EventArgs e)
        {
            GetClipboardIntoTextbox();
            richTextBox1.SelectionBackColor = Color.Black;
        }

        private void GetClipboardIntoTextbox()
        {
            if (Clipboard.ContainsText())
            {
                this.richTextBox1.Text = Clipboard.GetText();
                this.richTextBox1.SelectionStart = 0;
                this.richTextBox1.SelectionLength = this.richTextBox1.TextLength;
            }
        }

        private void buttonPasteText_Click(object sender, EventArgs e)
        {
            GetClipboardIntoTextbox();
        }

        private void FindtextBox_TextChanged(object sender, EventArgs e)
        {
            SearchAndIdentifyText();
        }

        private void buttonSaveToFile_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != null && richTextBox1.Text.Length > 0)
            {
                int characters = 30;
                if (richTextBox1.Text.Length < 30)
                {
                    characters = richTextBox1.Text.Length;
                }
                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).FullName;
                var filename = FileUtilities.RemoveIllegalCharacters(richTextBox1.Text.Trim().Substring(0, characters));
                filename = $@"{path}\Documents\{filename}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
                File.WriteAllText(filename, richTextBox1.Text);
                this.Text = $"Dictation Box Saved to: {filename}";
                Process.Start(filename);
            }
        }

        private void buttonScreenCapture_Click(object sender, EventArgs e)
        {
            this.Opacity = 0;
            string picturesFolder = CaptureImages();
            this.Text = $"Dictation Box - Screen saved to {picturesFolder}";
            this.Opacity = 1;
        }

        private static string CaptureImages()
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height,
                                           PixelFormat.Format32bppRgb
                                           );

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            var picturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var filename = $@"{picturesFolder}\ScreenshotPrimary.jpg";
            bmpScreenshot.Save(filename, ImageFormat.Jpeg);
            Clipboard.SetText(filename);
            //Create a new bitmap.
            bmpScreenshot = new Bitmap(Screen.AllScreens[1].Bounds.Width,
                                           Screen.AllScreens[1].Bounds.Height,
                                           PixelFormat.Format32bppRgb
                                           );

            // Create a graphics object from the bitmap.
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.AllScreens[1].Bounds.X,
                                        Screen.AllScreens[1].Bounds.Y,
                                        0,
                                        0,
                                        Screen.AllScreens[1].Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            filename = $@"{picturesFolder}\ScreenshotSecondary.jpg";
            bmpScreenshot.Save(filename, ImageFormat.Jpeg);
            return picturesFolder;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            string webAddress;
            if (richTextBox1.SelectedText != null && richTextBox1.SelectedText.Length > 0)
            {
                webAddress = richTextBox1.SelectedText;
            }
            else
            {
                webAddress = richTextBox1.Text;
            }

            if (!string.IsNullOrEmpty(webAddress))
            {
                try
                {
                    Process.Start(webAddress);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Filename or Web Address Required!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
    }
}
