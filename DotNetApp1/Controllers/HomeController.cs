using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetApp1.Controllers
{
    public class HomeController : Controller
    {
        protected string CurrentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return User.Identity.Name;

#if DEBUG
                return "TEST\\developer01";
#else
            return null;
#endif
            }
        }
        // GET: Home
        public ActionResult Index()
        {
            return Content(
                $"IsAuthenticated: {User.Identity.IsAuthenticated}\n" +
                $"Name: {User.Identity.Name}"
            );
            //using (var con = new SqlConnection(Utils.DBHelper.ConnectionString))
            //{
            //    con.Open();
            //}

            //return Content("DB Connected Successfully");
        }
    }
}