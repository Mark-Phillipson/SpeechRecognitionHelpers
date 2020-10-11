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
        public string Message { get; set; }
        public DisplayMessage(string message)
        {
            InitializeComponent();
            this.richTextBoxMessage.Text = message;
            Text = "Error Message (Will hide in three seconds)";
            timer1.Interval = 3000;
            timer1.Enabled=true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DisplayMessage_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            richTextBoxMessage.BackColor = Color.Black;
            richTextBoxMessage.ForeColor = Color.White;

        }
    }
}
