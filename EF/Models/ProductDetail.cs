using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class ProductDetail
    {
        [ForeignKey("Product")]
        public int ID { get; set; }
        public int Mass { get; set; }
        public int Power { get; set; }
        public int Size { get; set; }
        public string Producer { get; set; }
        public int MaintenanceTime { get; set; }
        public string Image { get; set; }
        public Product Product { get; set; }
    }
}