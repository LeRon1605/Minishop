using EF.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3.Areas.Admin.Models
{
    public class ProductModel
    {
        public Product Product { get; set; }
        public ProductDetail Detail { get; set; }
    }
}