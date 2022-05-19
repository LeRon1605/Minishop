using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.ViewModel
{
    class OrderViewModel
    {
        public int ID { get; set; }
        public string ReceiverAddress { get; set; }
        public int Total { get; set; }
        public int UserID { get; set; }
        public bool isCancel { get; set; }
        public bool isReceived { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual User User { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<State> States { get; set; }
    }
}
