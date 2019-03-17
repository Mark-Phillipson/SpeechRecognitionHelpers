using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KillApplications.Models
{
    class Application
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="Process Name")]
        public string ProcessName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }

        [Required]
        public bool Display { get; set; }

        [StringLength(4)]
        public string Kill { get; set; }
    }
}
