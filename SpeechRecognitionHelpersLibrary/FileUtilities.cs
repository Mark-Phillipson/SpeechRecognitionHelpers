using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpeechRecognitionHelpersLibrary
{
    public static class FileUtilities
    {
        public static string RemoveIllegalCharacters(string fileName)
        {
                Regex illegalInFileName = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))), RegexOptions.Compiled);
            string result = illegalInFileName.Replace(fileName, "");
            return result;
        }

    }
}
