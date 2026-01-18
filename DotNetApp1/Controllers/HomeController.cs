using DotNetApp1.DAO;
using DotNetApp1.Viewmodels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DotNetApp1.Controllers
{
    public class HomeController : Controller
    {
        protected string currentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return User.Identity.Name;

#if DEBUG
                return "E001";
#else
            return null;
#endif
            }
        }
        // GET: Home
        public ActionResult Index()
        {
            //return Content(
            //    $"IsAuthenticated: {User.Identity.IsAuthenticated}\n" +
            //    $"Name: {User.Identity.Name}"
            //);
            //using (var con = new SqlConnection(Utils.DBHelper.ConnectionString))
            //{
            //    con.Open();
            //}

            //return Content(currentUser);
            //return Content("DB Connected Successfully");

            var vm = new HomeIndexViewModel
            {
                SiteList = new List<SelectListItem>()
            };

#if DEBUG
            var userId = "E002"; // fake user for local testing
#else
        var userId = User.Identity.Name;
#endif
            var kanrishaDao = new KanrishaDao();
            var riyoshaDao = new RiyoshaDao();
            var siteDao = new SiteDao();

            if (kanrishaDao.Exists(userId))
            {
                vm.IsKanrisha = true;
                vm.SiteList = siteDao.GetAllSites();
            }
            else
            {
                var siteIds = riyoshaDao.GetSiteIds(userId);

                if (siteIds.Any())
                {
                    vm.IsRiyosha = true;
                    vm.SiteList = siteDao.GetSitesByIds(siteIds);
                }
                else
                {
                    vm.Message = "make application to use the system";
                }
            }
            if (vm.SiteList.Any())
            {
                vm.SiteList[0].Selected = true;
                vm.SelectedSiteId = vm.SiteList[0].Value;
            }

            return View(vm);
        }
    }
}