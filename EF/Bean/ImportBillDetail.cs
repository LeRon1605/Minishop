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
    public class ImportBillDetail
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Product")]
        public int? ProductID { get; set; }
        [ForeignKey("ImportBill")]
        public int ImportBillID { get; set; }
        [DisplayName("Số lượng")]
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng sản phẩm không được bé hơn 0.")]
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
        public virtual ImportBill ImportBill { get; set; }
    }
}
