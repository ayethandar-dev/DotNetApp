using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetApp1.DTO
{
    public class UkebaraiboExcelDto
    {
        public string UkebaraiboId { get; set; }
        public DateTime ShinseiDate { get; set; }
        public string ShoninStatus { get; set; }
        public string CorporateCompanyName { get; set; }
        public int Maisuu { get; set; }
        public int ReceiptFlag { get; set; }
        public string JimusekininshaName { get; set; }
        public string Kubun { get; set; }
    }
}