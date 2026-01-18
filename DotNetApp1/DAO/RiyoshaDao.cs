using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DotNetApp1.DAO
{
    public class RiyoshaDao
    {
        public List<string> GetSiteIds(string userId)
        {
            var result = new List<string>();

            var sql = @"
            SELECT SiteId
            FROM Riyosha
            WHERE UserId = @UserId
              AND DeleteFlag = 0";

            using (var conn = new SqlConnection(Utils.DBHelper.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader["SiteId"].ToString());
                    }
                }
            }

            return result;
        }
    }

}