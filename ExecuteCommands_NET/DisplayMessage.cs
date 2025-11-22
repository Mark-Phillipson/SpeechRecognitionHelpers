using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictationBoxMSP
{
    public partial class DisplayMessage : Form
    {
            /// <summary>
            /// Shared UI style used by other popup forms so they can match fonts and colors.
            /// </summary>
            public static Font SharedFont { get; private set; } = SystemFonts.MessageBoxFont!;
            public static Color SharedBackColor { get; private set; } = Color.Black;
            public static Color SharedForeColor { get; private set; } = Color.White;

      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Message { get; set; } = string.Empty;
        public DisplayMessage(string message, int interval = 3000)
        {
            InitializeComponent();
            // Initialize shared style from this instance's defaults so other forms can match it.
            SharedFont = this.Font ?? SharedFont;
            SharedBackColor = Color.Black;
            SharedForeColor = Color.White;
            this.Message = message;
            this.richTextBoxMessage.Text = message;
            this.Text = "Message";
            timer1.Interval = interval;
            timer1.Enabled = true;
        }

        public DisplayMessage(string message, int interval, string title)
        {
            InitializeComponent();
            this.Message = message;
            this.richTextBoxMessage.Text = message;
            this.Text = title;
            timer1.Interval = interval;
            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DisplayMessage_Load(object sender, EventArgs e)
        {
            this.BackColor = SharedBackColor;
            this.ForeColor = SharedForeColor;
            this.Font = SharedFont;
            richTextBoxMessage.BackColor = SharedBackColor;
            richTextBoxMessage.ForeColor = SharedForeColor;
            richTextBoxMessage.Font = SharedFont;

        }
    }
}
