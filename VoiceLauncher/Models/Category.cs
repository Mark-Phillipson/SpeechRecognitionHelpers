namespace VoiceLauncher.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category
    {
        private readonly ObservableListSource<CustomIntelliSense> _customIntellisenses = new ObservableListSource<CustomIntelliSense>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            //CustomIntelliSenses = new HashSet<CustomIntelliSense>();
            Launchers = new ObservableListSource<Launcher>();
        }

        [Key]
        public int ID { get; set; }

        [StringLength(30)]
        [Column("Category")]
        public string CategoryName { get; set; }

        [StringLength(255)]
        [Column("Category_Type")]
        public string CategoryType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CustomIntelliSense> CustomIntelliSenses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableListSource<Launcher> Launchers { get; set; }
        public virtual ObservableListSource<CustomIntelliSense> CustomIntelliSenses { get { return _customIntellisenses; } }
    }
}
