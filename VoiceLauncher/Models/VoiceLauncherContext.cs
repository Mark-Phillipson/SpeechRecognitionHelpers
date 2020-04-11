namespace VoiceLauncher.Models
{
    using System.Data.Entity;

    public partial class VoiceLauncherContext : DbContext
    {
        public VoiceLauncherContext()
            : base("name=VoiceLauncher")
        {
        }

        public virtual DbSet<Computer> Computers { get; set; }
        public virtual DbSet<CurrentWindow> CurrentWindows { get; set; }
        public virtual DbSet<GeneralLookup> GeneralLookups { get; set; }
        public virtual DbSet<HtmlTag> HtmlTags { get; set; }
        public virtual DbSet<LauncherMultipleLauncherBridge> LauncherMultipleLauncherBridges { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<MousePosition> MousePositions { get; set; }
        public virtual DbSet<PropertyTabPosition> PropertyTabPositions { get; set; }
        public virtual DbSet<SavedMousePosition> SavedMousePositions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CustomIntelliSense> CustomIntelliSenses { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Launcher> Launchers { get; set; }
        public virtual DbSet<MultipleLauncher> MultipleLaunchers { get; set; }
        public virtual DbSet<ValuesToInsert> ValuesToInserts { get; set; }
        public virtual DbSet<CustomIntellisenseLauncherUnion> CustomIntellisenseLauncherUnions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
