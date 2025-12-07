using System;
using System.Windows.Forms;
using DictationBoxMSP;

namespace ExecuteCommands.Helpers
{
    public static class VoiceDictationHelper
    {
        public static string? ShowVoiceDictation(int timeoutMs = 20000)
        {
            string? result = null;
            try
            {
                using (var frm = new VoiceDictationForm(timeoutMs, autoStartDictation: true))
                {
                    var dr = frm.ShowDialog();
                    if (dr == DialogResult.OK)
                        result = frm.ResultText;
                }
            }
            catch { }
            return result;
        }
    }
}
