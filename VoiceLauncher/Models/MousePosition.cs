namespace VoiceLauncher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MousePositions")]
    public partial class MousePosition
    {
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Command { get; set; }

        [Display(Name ="Mouse Left")]
        public int MouseLeft { get; set; }

        [Display(Name ="Mouse Top")]
        public int MouseTop { get; set; }

        [StringLength(255)]
        [Display(Name ="Tab Page Name")]
        public string TabPageName { get; set; }

        [StringLength(255)]
        [Display(Name ="Control Name")]
        public string ControlName { get; set; }

        [StringLength(255)]
        public string Application { get; set; }
    }
}
