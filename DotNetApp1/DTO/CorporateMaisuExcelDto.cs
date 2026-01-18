using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetApp1.DTO
{
    public class CorporateMaisuExcelDto
    {
        public string CorporateCompanyName { get; set; }
        public int Kubun01Maisu { get; set; }
        public int Kubun02Maisu { get; set; }
        public int TotalMaisu { get; set; }
    }
}