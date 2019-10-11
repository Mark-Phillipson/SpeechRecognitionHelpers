using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExecuteCommands
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] arguments = null;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() < 2)
            {
                arguments = new string[] { args[0], "ToggleMicrophone" };
                //arguments=  new string[] { args[0], "/upper-left" };
            }
            else
            {
                arguments = Environment.GetCommandLineArgs();
            }
            this.Text = arguments[1];
        }
    }
}
