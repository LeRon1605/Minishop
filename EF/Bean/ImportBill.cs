using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class ImportBill
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Tổng giá trị đơn hàng")]
        [Required(ErrorMessage = "Không được để trống giá trị đơn hàng")]
        [Range(0, int.MaxValue, ErrorMessage = "Tổng giá trị đơn hàng không được bé hơn 0.")]
        public int TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual List<ImportBillDetail> ImportBillDetails { get; set; }
    }
}
