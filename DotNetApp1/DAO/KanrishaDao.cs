using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DotNetApp1.DAO
{
    public class KanrishaDao
    {
        public bool Exists(string userId)
        {
            var sql = @"
            SELECT 1
            FROM Kanrisha
            WHERE UserId = @UserId
              AND DeleteFlag = 0";

            using (var conn = new SqlConnection(Utils.DBHelper.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();
                return cmd.ExecuteScalar() != null;
            }
        }
    }
}