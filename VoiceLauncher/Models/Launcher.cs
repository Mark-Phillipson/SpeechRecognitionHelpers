namespace VoiceLauncher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Launcher")]
    public partial class Launcher
    {
        public int ID { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(255)]
        [Display(Name="Command Line")]
        public string CommandLine { get; set; }

        public int CategoryID { get; set; }

        public int? ComputerID { get; set; }

        public virtual Computer Computer { get; set; }

        public virtual Category Category { get; set; }
    }
}
