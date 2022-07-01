using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models.DTO
{
    public class Cart
    {
        public Cart()
        {
            CartProduct = new HashSet<CartProduct>();
        }
        [ForeignKey("User")]
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public User User { get; set; }
        public virtual ICollection<CartProduct> CartProduct { get; set; }
    }
}