using DotNetApp1.DTO;
using DotNetApp1.Viewmodels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace DotNetApp1.DAO
{
    public class UkebaraiboDao
    {
        private static string ConnStr =>
            System.Configuration.ConfigurationManager
            .ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Get Ukebaraibo list by site (role check should be done before calling)
        /// </summary>
        public static List<UkebaraiboRowViewModel> GetBySite(string userId, string siteId)
        {
            var result = new List<UkebaraiboRowViewModel>();

            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("  u.UkebaraiboId,");
            sql.AppendLine("  u.ShinseiDate,");
            sql.AppendLine("  u.ShoninStatus,");
            sql.AppendLine("  c.CompanyName AS CorporateCompanyName,");
            sql.AppendLine("  u.Ichinichiken,");
            sql.AppendLine("  u.Tokaken,");
            sql.AppendLine("  u.ReceiptFlag,");
            sql.AppendLine("  e.EmployeeName AS JimusekininshaName");
            sql.AppendLine("FROM Ukebaraibo u");
            sql.AppendLine("LEFT JOIN CorporateCompany c ON u.CorporateCompanyId = c.CompanyId");
            sql.AppendLine("LEFT JOIN Employee e ON u.JimusekininshaId = e.EmployeeId");
            sql.AppendLine("WHERE u.DeleteFlag = 0");
            sql.AppendLine("  AND u.SiteId = @SiteId");
            sql.AppendLine("ORDER BY u.ShinseiDate DESC");

            using (var con = new SqlConnection(Utils.DBHelper.ConnectionString))
            using (var cmd = new SqlCommand(sql.ToString(), con))
            {
                cmd.Parameters.AddWithValue("@SiteId", siteId);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new UkebaraiboRowViewModel
                        {
                            UkebaraiboId = reader["UkebaraiboId"].ToString(),
                            ShinseiDate = Convert.ToDateTime(reader["ShinseiDate"]),
                            ShoninStatus = reader["ShoninStatus"].ToString(),
                            CorporateCompanyName = reader["CorporateCompanyName"].ToString(),
                            IchinichiKen = Convert.ToInt32(reader["Ichinichiken"]),
                            TokaKen = Convert.ToInt32(reader["Tokaken"]),
                            ReceiptFlag = Convert.ToInt32(reader["ReceiptFlag"]),
                            JimusekininshaName = reader["JimusekininshaName"].ToString()
                        });
                    }
                }
            }

            return result;
        }

        public static void UpdateReceiptFlag(string ukebaraiboId, int flag)
        {
            const string sql = @"
        UPDATE Ukebaraibo
        SET ReceiptFlag = @Flag
        WHERE UkebaraiboId = @Id
    ";

            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Flag", flag);
                cmd.Parameters.AddWithValue("@Id", ukebaraiboId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateCompany(string ukebaraiboId, string companyId)
        {
            const string sql = @"
        UPDATE Ukebaraibo
        SET CooperateCompanyId = @CompanyId
        WHERE UkebaraiboId = @UkebaraiboId
    ";

            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@CompanyId", companyId);
                cmd.Parameters.AddWithValue("@UkebaraiboId", ukebaraiboId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<UkebaraiboExcelDto> GetUkebaraiboForExcel(string siteId)
        {
            var list = new List<UkebaraiboExcelDto>();

            string sql = @"
            SELECT
                u.UkebaraiboId,
                u.ShinseiDate,
                u.ShoninStatus,
                c.CompanyName,
                (u.IchinichiKen + u.TokaKen * 10) AS Maisuu,
                u.ReceiptFlag,
                u.Kubun,
                p.EmployeeName AS JimusekininshaName
            FROM Ukebaraibo u
            LEFT JOIN CorporateCompany c
                ON u.CorporateCompanyId = c.CompanyId
            LEFT JOIN Employee p
                ON u.JimusekininshaId = p.EmployeeId
            WHERE u.SiteId = @SiteId
              AND u.DeleteFlag = 0
            ORDER BY u.ShinseiDate";

            using (SqlConnection conn =
                   new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@SiteId", siteId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new UkebaraiboExcelDto
                        {
                            UkebaraiboId = reader["UkebaraiboId"].ToString(),
                            ShinseiDate = Convert.ToDateTime(reader["ShinseiDate"]),
                            ShoninStatus = reader["ShoninStatus"].ToString(),
                            CorporateCompanyName = reader["CompanyName"].ToString(),
                            Maisuu = Convert.ToInt32(reader["Maisuu"]),
                            ReceiptFlag = Convert.ToInt32(reader["ReceiptFlag"]),
                            Kubun = reader["Kubun"].ToString(),
                            JimusekininshaName = reader["JimusekininshaName"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public List<CorporateMaisuExcelDto> GetCorporateMaisuForExcel(string siteId)
        {
            var list = new List<CorporateMaisuExcelDto>();

            string sql = @"
            SELECT
                c.CompanyName,

                SUM(CASE WHEN u.Kubun = '01'
                    THEN (u.IchinichiKen + u.TokaKen * 10)
                    ELSE 0 END) AS Kubun01Maisu,

                SUM(CASE WHEN u.Kubun = '02'
                    THEN (u.IchinichiKen + u.TokaKen * 10)
                    ELSE 0 END) AS Kubun02Maisu,

                SUM(u.IchinichiKen + u.TokaKen * 10) AS TotalMaisu
            FROM Ukebaraibo u
            INNER JOIN CorporateCompany c
                ON u.CorporateCompanyId = c.CompanyId
            WHERE u.DeleteFlag = 0
              AND (@SiteId IS NULL OR u.SiteId = @SiteId)
            GROUP BY c.CompanyName
            ORDER BY c.CompanyName";

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (string.IsNullOrEmpty(siteId))
                    cmd.Parameters.AddWithValue("@SiteId", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@SiteId", siteId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CorporateMaisuExcelDto
                        {
                            CorporateCompanyName = reader["CompanyName"].ToString(),
                            Kubun01Maisu = Convert.ToInt32(reader["Kubun01Maisu"]),
                            Kubun02Maisu = Convert.ToInt32(reader["Kubun02Maisu"]),
                            TotalMaisu = Convert.ToInt32(reader["TotalMaisu"])
                        });
                    }
                }
            }

            return list;
        }
    }
}