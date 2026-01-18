using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetApp1.Viewmodels
{
    public class UkebaraiboIndexViewModel
    {
        public string SelectedSiteId { get; set; }
        public List<SelectListItem> SiteList { get; set; }
        public List<UkebaraiboRowViewModel> UkebaraiboList { get; set; }
    }
}