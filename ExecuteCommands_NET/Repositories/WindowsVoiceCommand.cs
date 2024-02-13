using DataAccessLibrary.Models;
using ExecuteCommands.Models;
using Microsoft.EntityFrameworkCore;



namespace ExecuteCommands.Repositories
{
    public class WindowsVoiceCommand
    {
		VoiceLauncherContext Model;
        public WindowsVoiceCommand()
        {
            if (System.Environment.MachineName == "J40L4V3")
            {
                Model = new VoiceLauncherContext("Data Source=J40L4V3;Initial Catalog=VoiceLauncher;Integrated Security=True;Connect Timeout=120;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
            else
            {
                Model = new VoiceLauncherContext("Data Source=Localhost;Initial Catalog=VoiceLauncher;Integrated Security=True;Connect Timeout=120;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
        public List<WindowsSpeechVoiceCommand> GetCommands()
        {
            var result = Model?.WindowsSpeechVoiceCommands
                    .AsNoTracking()
                    .Where(v => v.ApplicationName == "Global")
                    .ToList();
            if (result == null)
            {
                throw new Exception("Failed to get commands from database");
            }
            return result;
        }
        public string GetEnterCommand(string command)
        {
            if (Model == null)
            {
                return "";
            }
            command = command.Replace("enter", "").Trim();
            var result = Model.ValuesToInserts
                    .AsNoTracking()
                    .Where(v => v.Lookup.ToLower() == command.ToLower()).FirstOrDefault();
            if (result != null)
            {
                return result.ValueToInsertValue;
            }
            return "";

        }
        public List<WindowsSpeechVoiceCommand> GetSpecialCommands(string? applicationName, string endsWith)
        {
            if (applicationName == null)
            {
                var result = Model.WindowsSpeechVoiceCommands
                        .AsNoTracking()
                        .Include(i => i.SpokenForms)
                        .Where(v => v.ApplicationName == "Global" && v.SpokenForms != null && v.SpokenForms.Any(i => i.SpokenFormText.EndsWith(endsWith)))
                        .ToList();
                return result;
            }
            else
            {
                var result = Model.WindowsSpeechVoiceCommands
                        .AsNoTracking()
                        .Include(i => i.SpokenForms)
                        .Where(v => (v.ApplicationName == "Global" || v.ApplicationName == applicationName) && v.SpokenForms != null && v.SpokenForms.Any(i => i.SpokenFormText.EndsWith(endsWith)))
                        .ToList();
                return result;
            }
        }
        public WindowsSpeechVoiceCommand? GetRandomCommand()
        {
            if (Model == null)
            {
                return null;
            }
            //Get a random voice command
            var result = Model.WindowsSpeechVoiceCommands
                    .AsNoTracking()
                    .Include(c => c.SpokenForms)
                    .Where(v => v.AutoCreated == false)
                    .OrderBy(r => Guid.NewGuid())
                    .FirstOrDefault();
            return result;
        }
        public WindowsSpeechVoiceCommand? GetCommand(string spokenCommand, string? applicationName)
        {
            if (Model == null)
            {
                return null;
            }
            if (applicationName != null && Model != null)
            {
                WindowsSpeechVoiceCommand? applicationCommand = Model.WindowsSpeechVoiceCommands
                        .AsNoTracking()
                        .Include(i => i.SpokenForms)
                        .Where(v => v.SpokenForms != null && v.SpokenForms.Any(c => c.SpokenFormText.ToLower().Trim() == spokenCommand.ToLower().Trim()) && v.ApplicationName == applicationName)
                        .FirstOrDefault();
                if (applicationCommand != null)
                {
                    return applicationCommand;
                }
            }
            WindowsSpeechVoiceCommand? command = Model?.WindowsSpeechVoiceCommands
                    .AsNoTracking()
                    .Include(i => i.SpokenForms)
                    .Where(v => v.SpokenForms != null && v.SpokenForms.Any(i => i.SpokenFormText.ToLower() == spokenCommand.ToLower()) && v.ApplicationName == "Global")
                    .FirstOrDefault();
            return command;
        }
        public WindowsSpeechVoiceCommand? GetCommandById(int id)
        {
            if (Model == null)
            {
                return null;
            }
            WindowsSpeechVoiceCommand? command = Model.WindowsSpeechVoiceCommands
                    .AsNoTracking()
                    .Include(i => i.SpokenForms)
                    .Where(v => v.Id == id)
                    .FirstOrDefault();
            return command;
        }
        public List<CustomWindowsSpeechCommand>? GetChildActions(int windowsSpeechVoiceCommandId)
        {
            var results = Model.CustomWindowsSpeechCommands
                    .AsNoTracking()
                    .Where(v => v.WindowsSpeechVoiceCommandId == windowsSpeechVoiceCommandId);
            if (results != null)
            {
                var actions = results.ToList();
                return actions;
            }
            return null;
        }
        public List<PhraseListGrammar>? GetPhraseListGrammars()
        {
            var results = Model.PhraseListGrammars.AsNoTracking();
            if (results != null)
            {
                var phraseListGrammars = results.ToList();
                return phraseListGrammars;
            }
            return null;
        }
        public List<DataAccessLibrary.Models.ApplicationDetail>? GetApplicationDetails()
        {
            var results = Model.ApplicationDetails.AsNoTracking();
            if (results != null)
            {
                var applicationDetails = results.ToList();
                return applicationDetails;
            }
            return null;
        }
        public List<Idiosyncrasy>? GetIdiosyncrasies()
        {
            var results = Model.Idiosyncrasies.AsNoTracking();
            if (results != null)
            {
                var idiosyncrasies = results.ToList();
                return idiosyncrasies;
            }
            return null;
        }
        public List<GrammarItem>? GetListItems(string grammarName)
        {
            var result = Model.GrammarNames.Where(v => v.NameOfGrammar == grammarName).FirstOrDefault();
            if (result != null)
            {
                List<GrammarItem> items = Model.GrammarItems.Where(v => v.GrammarNameId == result.Id).ToList();
                return items;
            }
            return null;
        }
        public HtmlTag? GetHtmlTag(string tag)
        {
            var result = Model.HtmlTags.Where(v => v.SpokenForm == tag).FirstOrDefault();
            return result;
        }
        public List<HtmlTag> GetHtmlTags()
        {
            var result = Model.HtmlTags.Where(v => v.SpokenForm != null).OrderBy(v => v.SpokenForm).ToList();
            return result;
        }

        public CustomIntelliSense? GetWord(string searchTerm)
        {
            var result = Model.CustomIntelliSenses.Where(i => i.LanguageId == 1 && i.CategoryId == 39 && i.DisplayValue.ToLower() == searchTerm.ToLower()).OrderBy(v => v.DisplayValue).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public List<AdditionalCommand> GetAdditionalCommands(int id)
        {
            var result = Model.AdditionalCommands.Where(v => v.CustomIntelliSenseId == id).ToList();
            return result;
        }

        public Microphone? GetMicrophoneFromTable()
        {
            var microphone = Model.Microphones.FirstOrDefault(c => c.Default);
            return microphone;
        }
        
    }
}

