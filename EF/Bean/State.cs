using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models.DTO
{
    public class State
    {
        public State()
        {
            StateOrder = new HashSet<StateOrder>();
        }
        [Key]
        public int ID { get; set; }
        [Display(Name ="Trạng thái")]
        public string Name { get; set; }
        [Display(Name="Mô tả")]
        public string Description { get; set; }
        public ICollection<StateOrder> StateOrder { get; set; }
    }
}