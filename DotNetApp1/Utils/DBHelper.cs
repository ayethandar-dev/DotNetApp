using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DotNetApp1.Utils
{
    public static class DBHelper
    {
        public static string ConnectionString
            => ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}