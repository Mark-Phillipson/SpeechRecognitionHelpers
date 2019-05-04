namespace VoiceLauncher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ValuesToInsert")]
    public partial class ValuesToInsert
    {
        public int ID { get; set; }

        [Column("ValueToInsert")]
        [Required]
        [StringLength(255)]
        public string ValueToInsert { get; set; }

        [Required]
        [StringLength(255)]
        public string Lookup { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}
