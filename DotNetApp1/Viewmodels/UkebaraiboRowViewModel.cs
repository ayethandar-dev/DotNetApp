using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetApp1.Viewmodels
{
    public class UkebaraiboRowViewModel
    {
        public string UkebaraiboId { get; set; }
        public DateTime ShinseiDate { get; set; }
        public string ShoninStatus { get; set; }
        public string CorporateCompanyId { get; set; }
        public string CorporateCompanyName { get; set; }
        public int IchinichiKen { get; set; }
        public int TokaKen { get; set; }
        public int Maisuu => IchinichiKen + (TokaKen * 10);
        public int ReceiptFlag { get; set; }
        public string JimusekininshaName { get; set; }
    }
}