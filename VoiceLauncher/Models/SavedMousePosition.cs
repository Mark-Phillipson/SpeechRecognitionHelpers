namespace VoiceLauncher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SavedMousePosition")]
    public partial class SavedMousePosition
    {
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name ="Named Location")]
        public string NamedLocation { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public DateTime? Created { get; set; }
    }
}
