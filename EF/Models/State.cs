using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class State
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Trạng thái")]
        public string Name { get; set; }
        [Display(Name="Mô tả")]
        public string Description { get; set; }
        public List<StateOrder> StateOrder { get; set; }
    }
}