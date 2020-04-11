using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoiceLauncher.Models
{
    [Table("View_IntelliSense_Launcher_Union")]
    public class CustomIntellisenseLauncherUnion
    {
        public int Id { get; set; }
        [Column("Display_Value")]
        [Display(Name = "Display Value")]
        public string DisplayValue { get; set; }
        [Column("SendKeys_Value")]
        [Display(Name = "SendKeys Value")]
        public string SendkeysValue { get; set; }
        public string Category { get; set; }
        public string Source { get; set; }
        public string Language { get; set; }
    }
}
