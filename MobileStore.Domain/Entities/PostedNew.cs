using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileStore.Domain.Entities
{
    public class PostedNew
    {
        [Key]
        public int NewId { get; set; }

        [Required(ErrorMessage = "Please enter name of new")]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(2000)]
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        public File File { get; set; }
    }
}
