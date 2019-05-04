namespace VoiceLauncher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LauncherMultipleLauncherBridge")]
    public partial class LauncherMultipleLauncherBridge
    {
        public int ID { get; set; }

        public int LauncherID { get; set; }

        public int MultipleLauncherID { get; set; }
    }
}
