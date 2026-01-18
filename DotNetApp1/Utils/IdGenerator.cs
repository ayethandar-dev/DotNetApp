using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetApp1.Utils
{
    public static class IdGenerator
    {
        public static string GenerateUkebaraiboId(string siteId, int maxSeq)
        {
            var siteNo = siteId.Substring(1); // S001 → 001
            return $"U{siteNo}{(maxSeq + 1):D2}";
        }
    }
}