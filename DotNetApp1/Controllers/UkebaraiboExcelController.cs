using DotNetApp1.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetApp1.Controllers
{
    public class UkebaraiboExcelController : Controller
    {
        // GET: UkebaraiboExcel
        public ActionResult Export()
        {
#if DEBUG
            var userId = "E002"; // fake user for local testing
#else
        var userId = User.Identity.Name;
#endif
            var kanrishaDao = new KanrishaDao();
            var riyoshaDao = new RiyoshaDao();
            var siteDao = new SiteDao();
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
            ViewBag.SiteList = siteList;
            return View();
        }

        // Excel output
        [HttpPost]
        public ActionResult ExportExcel(
            string siteId,
            bool isAllSite,
            DateTime startDate,
            DateTime endDate)
        {
            // validation (server-side)
            if (!isAllSite && string.IsNullOrEmpty(siteId))
            {
                ModelState.AddModelError("", "対象現場を選択してください。");
            }

            if (startDate > endDate)
            {
                ModelState.AddModelError("", "申請日の範囲が正しくありません。");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.SiteList = GetSiteSelectList();
                return View("Export");
            }

            // siteId = null means ALL sites
            string targetSiteId = isAllSite ? null : siteId;

            // 🔽 call your existing Excel logic here
            return DownloadCorporateMaisu(targetSiteId, startDate, endDate);
        }

        // Dummy site list (replace with DAO)
        private List<SelectListItem> GetSiteSelectList()
        {
            return new List<SelectListItem>
            {
            new SelectListItem { Value = "S001", Text = "新宿現場" },
            new SelectListItem { Value = "S002", Text = "渋谷現場" }
            };
        }

        // Already implemented by you before
        private ActionResult DownloadCorporateMaisu(
            string siteId,
            DateTime startDate,
            DateTime endDate)
        {
            // call DAO → create Excel → return File()
            return Content("Excel output here");
        }
    }
}