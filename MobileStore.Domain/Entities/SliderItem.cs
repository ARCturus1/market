using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileStore.Domain.Entities
{
    public class SliderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2000)]
        public string FilePath { get; set; }
    }
}
