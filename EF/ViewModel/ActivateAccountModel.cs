using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBL3.Models
{
    public class ActivateAccountModel
    {
        public int userID { get; set; }
        public string userEmail { get; set; }
        public string Key { get; set; }
        public DateTime Date { get; set; }
    }
}