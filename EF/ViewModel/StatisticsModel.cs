using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.Models
{
    public class StatisticsModel
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public int OrderCancelCount { get; set; }
        public int OrderInProcess { get; set; }
        public int Revenue { get; set; }
        public int Benifit { get; set; }
        public string Format { get; set; }
        public int NewUser { get; set; }
    }
}
