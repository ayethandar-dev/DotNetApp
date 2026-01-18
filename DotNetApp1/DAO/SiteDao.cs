using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetApp1.DAO
{
    public class SiteDao
    {
        public List<SelectListItem> GetAllSites()
        {
            var list = new List<SelectListItem>();

            var sql = "SELECT SiteId, SiteName FROM Site";

            using (var conn = new SqlConnection(Utils.DBHelper.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SelectListItem
                        {
                            Value = reader["SiteId"].ToString(),
                            Text = reader["SiteName"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public List<SelectListItem> GetSitesByIds(List<string> siteIds)
        {
            if (!siteIds.Any())
                return new List<SelectListItem>();

            var sql = $@"
            SELECT SiteId, SiteName
            FROM Site
            WHERE SiteId IN ({string.Join(",", siteIds.Select((x, i) => "@p" + i))})";

            using (var conn = new SqlConnection(Utils.DBHelper.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                for (int i = 0; i < siteIds.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@p" + i, siteIds[i]);
                }

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var list = new List<SelectListItem>();
                    while (reader.Read())
                    {
                        list.Add(new SelectListItem
                        {
                            Value = reader["SiteId"].ToString(),
                            Text = reader["SiteName"].ToString()
                        });
                    }
                    return list;
                }
            }
        }
    }

}