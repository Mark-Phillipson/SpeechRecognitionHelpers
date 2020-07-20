using System.Drawing;
using System.Windows.Forms;

namespace VoiceLauncher
{
    public static class CustomTheme
    {
        public static void SetDataGridViewTheme(DataGridView dataGridView, string fontName = "Cascadia Code", float fontSize = 11)
        {
            FontFamily fontFamily = new FontFamily(fontName);
            Font font = new Font(fontFamily, fontSize, FontStyle.Bold, GraphicsUnit.Point);
            var style = new DataGridViewCellStyle
            { BackColor = Color.FromArgb(38, 38, 38), ForeColor = Color.White, Font = font };
            dataGridView.DefaultCellStyle = style;
            dataGridView.ColumnHeadersDefaultCellStyle = style;
            dataGridView.RowHeadersDefaultCellStyle = style;
            dataGridView.RowsDefaultCellStyle = style;
            dataGridView.EnableHeadersVisualStyles = false;
        }

    }

}
