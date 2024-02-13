using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Configuration;

namespace ExecuteCommands.Models
{
	public partial class VoiceLauncherContext : DbContext
	{
		readonly string _connectionString;
		private Configuration _configuration;
		public VoiceLauncherContext(string connectionString)
		{
			_connectionString = connectionString;
		}
		 public virtual DbSet<WindowsSpeechVoiceCommand> WindowsSpeechVoiceCommands { get; set; } 
		 public  virtual DbSet<SpokenForm> SpokenForms { get; set; }
		public virtual DbSet<CustomWindowsSpeechCommand> CustomWindowsSpeechCommands { get; set; }
		public virtual DbSet<PhraseListGrammar> PhraseListGrammars { get; set; }
		public virtual DbSet<GrammarItem> GrammarItems { get; set; }
		public virtual DbSet<GrammarName> GrammarNames { get; set; }
		public virtual DbSet<Idiosyncrasy> Idiosyncrasies { get; set; }
		public virtual DbSet<ApplicationDetail> ApplicationDetails { get; set; }
		public virtual DbSet<Microphone> Microphones { get; set; }
		public virtual DbSet<Prompt> Prompts { get; set; }
		public virtual DbSet<TalonAlphabet> TalonAlphabets { get; set; }
		public virtual DbSet<CssProperty> CssProperties { get; set; }

		public virtual DbSet<AdditionalCommand> AdditionalCommands { get; set; }
		public virtual DbSet<Computer> Computers { get; set; }
		public virtual DbSet<CurrentWindow> CurrentWindows { get; set; }
		public virtual DbSet<GeneralLookup> GeneralLookups { get; set; }
		public virtual DbSet<HtmlTag> HtmlTags { get; set; }
		public virtual DbSet<LauncherMultipleLauncherBridge> LauncherMultipleLauncherBridges { get; set; }
		public virtual DbSet<Logins> Logins { get; set; }
		public virtual DbSet<MousePositions> MousePositions { get; set; }
		public virtual DbSet<PropertyTabPositions> PropertyTabPositions { get; set; }
		public virtual DbSet<SavedMousePosition> SavedMousePositions { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<CustomIntelliSense> CustomIntelliSenses { get; set; }
		public virtual DbSet<Language> Languages { get; set; }
		public virtual DbSet<Launcher> Launchers { get; set; }
		public virtual DbSet<MultipleLauncher> MultipleLaunchers { get; set; }
		public virtual DbSet<ValuesToInsert> ValuesToInserts { get; set; }
		public virtual DbSet<CustomIntelliSenseViewModel> CustomIntelliSenseViewModels { get; set; }
		public virtual DbSet<Todo> Todos { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				if (_configuration != null)
				{
					optionsBuilder.UseSqlServer(_connectionString);
				}
				else
				{
					optionsBuilder.UseSqlServer("Data Source=Localhost;Initial Catalog=VoiceLauncher;Integrated Security=True;Connect Timeout=120;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
				}
#if DEBUG
				optionsBuilder.LogTo(message => Console.WriteLine(message));
				optionsBuilder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
#endif
			}
			base.OnConfiguring(optionsBuilder);
		}
	}
}
