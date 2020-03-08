using System;
using System.Linq;
using VoiceLauncher.Models;
using System.Windows.Forms;

namespace VoiceLauncher
{
    public class OpenForms
    {
        private VoiceLauncherContext db;
        private int categoryId = 39;
        private int languageId = 1;
        private string searchTerm = null;
        public OpenForms()
        {
            db = new VoiceLauncherContext();
        }
        public void LoadForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string[] arguments;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() < 2)
            {
                //arguments = new string[] { args[0], "Launcher", "Unknown", "Download" };
                //strCommandLine =    strCommandLine & " "  & Chr(34)  & "/ " &  "Launcher" & CHR(34)  & " " & Chr(34) &   " / " &  ListVar1  & Chr(34) & " " & Chr(34) &   " / " &  "Unknown"  & Chr(34)
                //arguments = new string[] { args[0], "Launcher", "Access Projects", "Unknown" };
                //arguments = new string[] { args[0], "Unknown", "Unknown", "Class" };
                //arguments = new string[] { args[0], "Add New", "Some new value" };
                //arguments = new string[] { args[0], "Razor", "Snippet" };
                arguments = new string[] { args[0], "Not Applicable", "Words" };
            }
            else
            {
                arguments = Environment.GetCommandLineArgs();
            }
            //MessageBox.Show($"1:{arguments[1]} 2:{arguments[2]} 3:{arguments[3]}");
            if (arguments[1].EndsWith("Add New") && arguments[2]?.Length > 0)
            {
                CustomIntelliSenseSingleRecord customIntelliSenseSingleRecord = new CustomIntelliSenseSingleRecord();
                customIntelliSenseSingleRecord.CurrentId = (int)0;
                customIntelliSenseSingleRecord.DefaultValueToSend = arguments[2].Replace("/", "").Trim();
                var languageId = db.Languages.Where(v => v.LanguageName == "Not Applicable").FirstOrDefault()?.ID;
                customIntelliSenseSingleRecord.LanguageId = languageId;
                var categoryId = db.Categories.Where(v => v.CategoryName == "Words").FirstOrDefault()?.ID;
                customIntelliSenseSingleRecord.CategoryId = categoryId;
                Application.Run(customIntelliSenseSingleRecord);
                return;
            }
            else if (arguments[1].EndsWith("Launcher") && arguments[2].EndsWith("Unknown"))
            {
                LauncherForm launcherForm = new LauncherForm();
                launcherForm.SearchTerm = arguments[3].Replace("/", "").Trim();
                Application.Run(launcherForm);
                return;
            }
            else if (arguments[1].EndsWith("Launcher") && arguments[3].EndsWith("Unknown"))
            {
                LauncherForm launcherForm = new LauncherForm();
                launcherForm.CategoryFilter = arguments[2].Replace("/", "").Trim();
                launcherForm.Show();
                Application.Run(launcherForm);
                return;
            }

            if (arguments[1].ToLower().Contains("unknown") && arguments[2].ToLower().Contains("unknown"))
            {
                VoiceLauncher.CustomIntelliSenseForm customIntelliSense = new VoiceLauncher.CustomIntelliSenseForm();
                customIntelliSense.SearchTerm = arguments[3].Replace("/", "").Trim();
                customIntelliSense.Text = $"Custom IntelliSense Search Term: {searchTerm}";
                Application.Run(customIntelliSense);
            }
            else
            {
                VoiceLauncher.CustomIntelliSenseForm customIntelliSense = new VoiceLauncher.CustomIntelliSenseForm();
                string languageName = arguments[1].Replace("/", "").Trim();

                Language language =db.Languages.Where(v => v.LanguageName == languageName).FirstOrDefault();
                if (language == null)
                {
                    throw (new Exception($" Language not found in commandline argument {arguments[1]}"));
                }
                customIntelliSense.LanguageId = language.ID;
                string categoryName = arguments[2].Replace("/", "").Trim();
                Category category = db.Categories.Where(v => v.CategoryName == categoryName).FirstOrDefault();
                if (category == null)
                {
                    throw (new Exception($" the Category not found in commandline argument {arguments[2]}"));
                }
                customIntelliSense.CategoryId = category.ID;
                customIntelliSense.Text = $"Custom IntelliSense {language.LanguageName} {category.CategoryName}";
                Application.Run(customIntelliSense);
            }
        }
    }
}