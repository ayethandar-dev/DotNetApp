using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace DotNetApp1.Utils
{
    public class FileHelper
    {
        public static string GetReceiptPath(string siteId, string ukebaraiboId)
        {
            return Path.Combine(
                ConfigurationManager.AppSettings["ReceiptRoot"],
                siteId,
                $"{ukebaraiboId}.pdf"
            );
        }
    }
}