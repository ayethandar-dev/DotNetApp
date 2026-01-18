using ClosedXML.Excel;
using DotNetApp1.DAO;
using DotNetApp1.Services;
using DotNetApp1.Viewmodels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetApp1.Controllers
{
    public class UkebaraiboController : Controller
    {
        private readonly UkebaraiboDao _dao = new UkebaraiboDao();
        private readonly UkebaraiboExcelService _excelService = new UkebaraiboExcelService();
        // GET: Ukebaraibo
        public ActionResult Index(string siteId)
        {
#if DEBUG
            var userId = "E002"; // fake user for local testing
#else
        var userId = User.Identity.Name;
#endif

            // ① get sites user can access
            var kanrishaDao = new KanrishaDao();
            var siteDao = new SiteDao();
            var riyoshaDao = new RiyoshaDao();
            var siteList = new List<SelectListItem>();

            if (kanrishaDao.Exists(userId))
            {
                siteList = siteDao.GetAllSites();
            }
            else
            {
                var siteIds = riyoshaDao.GetSiteIds(userId);

                if (siteIds.Any())
                {
                    siteList = siteDao.GetSitesByIds(siteIds);
                }
            }
            // ② auto-select site
            string selectedSiteId = siteId;

            if (string.IsNullOrEmpty(selectedSiteId) && siteList.Any())
            {
                selectedSiteId = siteList.First().Value;
            }

            // ③ load ukebaraibo
            var ukebaraiboList = string.IsNullOrEmpty(selectedSiteId)
                ? new List<UkebaraiboRowViewModel>()
                : UkebaraiboDao.GetBySite(userId, selectedSiteId);

            var model = new UkebaraiboIndexViewModel
            {
                SelectedSiteId = selectedSiteId,
                SiteList = siteList,
                UkebaraiboList = ukebaraiboList
            };

            return View(model);
        }

        public ActionResult Download()
        {
            return View();
        }

        public ActionResult DownloadUkebaraiboExcel(string siteId)
        {

            var data = _dao.GetUkebaraiboForExcel(siteId);
            var wb = _excelService.CreateUkebaraiboWorkbook(data);

            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Ukebaraibo_{siteId}.xlsx"
                );
            }
        }

        public ActionResult DownloadCorporateMaisu(string siteId)
        {
            var data = _dao.GetCorporateMaisuForExcel(siteId);

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("会社別枚数");

            ws.Cell(1, 1).Value = "協力会社";
            ws.Cell(1, 2).Value = "区分01枚数";
            ws.Cell(1, 3).Value = "区分02枚数";
            ws.Cell(1, 4).Value = "合計枚数";

            int row = 2;
            foreach (var d in data)
            {
                ws.Cell(row, 1).Value = d.CorporateCompanyName;
                ws.Cell(row, 2).Value = d.Kubun01Maisu;
                ws.Cell(row, 3).Value = d.Kubun02Maisu;
                ws.Cell(row, 4).Value = d.TotalMaisu;
                row++;
            }

            ws.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"CorporateMaisu_{siteId}.xlsx"
                );
            }
        }
    }
}