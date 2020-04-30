using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpeechRecognitionHelpersLibrary
{
    public class MyRenderer : ToolStripProfessionalRenderer
    {
        public MyRenderer() : base(new MyColors()) { }
    }

    public class MyColors : ProfessionalColorTable
    {
        public override Color MenuStripGradientEnd
        {
            get { return Color.FromArgb(38, 38, 38); }
        }
        public override Color MenuStripGradientBegin
        {
            get { return Color.FromArgb(38, 38, 38); }
        }
        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(100, 100, 100); }
        }
        public override Color ImageMarginGradientEnd
        {
            get { return Color.FromArgb(38, 38, 38); }
        }
        public override Color ImageMarginGradientBegin
        {
            get { return Color.FromArgb(38, 38, 38); }
        }
        public override Color ImageMarginGradientMiddle
        {
            get { return Color.FromArgb(38, 38, 38); }
        }
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(100, 100, 100); }
        }
        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(38, 38, 38); }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(100, 100, 100); }
        }
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(100, 100, 100); }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(100, 100, 100); }
        }
        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.FromArgb(100, 100, 100); }
        }
    }
}
