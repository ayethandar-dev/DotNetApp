using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetApp1.Viewmodels
{
    public class HomeIndexViewModel
    {
        public string SelectedSiteId { get; set; }

        public List<SelectListItem> SiteList { get; set; }

        public bool IsKanrisha { get; set; }
        public bool IsRiyosha { get; set; }

        public string Message { get; set; }
    }
}