namespace VoiceLauncher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PropertyTabPosition
    {
        public int ID { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name ="Object Name")]
        public string ObjectName { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name ="Property Name")]
        public string PropertyName { get; set; }

        [Display(Name ="Number of Tabs")]
        public int NumberOfTabs { get; set; }
    }
}
