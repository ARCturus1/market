using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileStore.Domain.Entities
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "File Name")]
        public string FileName { get; set; }
    }
}
