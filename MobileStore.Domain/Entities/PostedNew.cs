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

        [MaxLength(500)]
        public string ShortDesk { get; set; }

        //[MaxLength(4000)]
        //[Required(ErrorMessage = "Please enter a description")]
        public ICollection<DescItem> Description { get; set; }

        public DateTime? Date { get; set; }

        public File File { get; set; }
    }
}
