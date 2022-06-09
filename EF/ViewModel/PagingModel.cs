using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3.Models
{
    public class PagingModel
    {
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }
        public Func<int, string> GenerateURL { get; set; }
    }
}