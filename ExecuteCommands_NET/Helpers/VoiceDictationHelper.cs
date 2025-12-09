using System;
using System.Windows.Forms;
using DictationBoxMSP;

namespace ExecuteCommands.Helpers
{
    public static class VoiceDictationHelper
    {
        // Default timeout 0 disables auto-submit; >0 will auto-submit after that many ms
        public static string? ShowVoiceDictation(int timeoutMs = 0)
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
