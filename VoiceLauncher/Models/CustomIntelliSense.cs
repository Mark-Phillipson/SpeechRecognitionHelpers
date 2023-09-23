namespace VoiceLauncher.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CustomIntelliSense")]
    public partial class CustomIntelliSense
    {
        public int ID { get; set; }

        public int LanguageID { get; set; }

        [StringLength(255)]
        [Required]
        [Display(Name = "Display Value")]
        public string Display_Value { get; set; }

        [Display(Name = "SendKeys Value")]
        public string SendKeys_Value { get; set; }

        [StringLength(255)]
        [Display(Name = "Command Type")]
        public string Command_Type { get; set; }

        public int CategoryID { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Search { get; set; }

        public int? ComputerID { get; set; }

        [Required]
        [StringLength(30)]
        public string DeliveryType { get; set; }
		[StringLength(60)]
		public string Variable1 { get; set; }
		[StringLength(60)]
		public string Variable2 { get; set; }
		[StringLength(60)]
		public string Variable3 { get; set; }


		public virtual Computer Computer { get; set; }

        public virtual Category Category { get; set; }

        public virtual Language Language { get; set; }
    }
}
