using DotNetApp1.DAO;
using DotNetApp1.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetApp1.Controllers
{
    public class FileController : Controller
    {
        [HttpPost]
        public ActionResult Upload(string siteId, string ukebaraiboId, HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return new HttpStatusCodeResult(400);

            var path = FileHelper.GetReceiptPath(siteId, ukebaraiboId);

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            file.SaveAs(path); // overwrite if exists

            UkebaraiboDao.UpdateReceiptFlag(ukebaraiboId, 1);

            return Json(true);
        }

        public ActionResult Preview(string siteId, string ukebaraiboId)
        {
            var path = FileHelper.GetReceiptPath(siteId, ukebaraiboId);

            if (!System.IO.File.Exists(path))
                return HttpNotFound();

            return File(path, "application/pdf");
        }

        [HttpPost]
        public ActionResult Delete(string siteId, string ukebaraiboId)
        {
            var path = FileHelper.GetReceiptPath(siteId, ukebaraiboId);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            UkebaraiboDao.UpdateReceiptFlag(ukebaraiboId, 0);

            return Json(true);
        }
    }

}