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
        // GET: Home
        public ActionResult Index()
        {
            using (var con = new SqlConnection(Utils.DBHelper.ConnectionString))
            {
                con.Open();
            }

            return Content("DB Connected Successfully");
        }
    }
}