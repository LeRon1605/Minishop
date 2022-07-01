using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models.DTO
{
    [Table("OrderHistoric")]
    public class StateOrder
    {
        [Key]
        public int ID { get; set; }
        public int StateID { get; set; }
        public int OrderID { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("StateID")]
        public State State { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
    }
}