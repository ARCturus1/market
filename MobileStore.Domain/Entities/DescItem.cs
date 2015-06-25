using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MobileStore.Domain.Entities
{
    public class DescItem
    {
        [Key]
        public int Id { get; set; }
        public string Item { get; set; }
    }
}
