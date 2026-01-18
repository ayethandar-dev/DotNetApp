using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using DotNetApp1.DTO;

namespace DotNetApp1.Services
{

    public class UkebaraiboExcelService
    {
        public XLWorkbook CreateUkebaraiboWorkbook(List<UkebaraiboExcelDto> data)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("受払簿");

            ws.Cell(1, 1).Value = "申請日";
            ws.Cell(1, 2).Value = "承認状態";
            ws.Cell(1, 3).Value = "会社名";
            ws.Cell(1, 4).Value = "枚数";
            ws.Cell(1, 5).Value = "領収書";
            ws.Cell(1, 6).Value = "事務責任者";

            int row = 2;
            foreach (var d in data)
            {
                ws.Cell(row, 1).Value = d.ShinseiDate;
                ws.Cell(row, 2).Value = d.ShoninStatus;
                ws.Cell(row, 3).Value = d.CorporateCompanyName;
                ws.Cell(row, 4).Value = d.Maisuu;
                ws.Cell(row, 5).Value = d.ReceiptFlag == 1 ? "有" : "無";
                ws.Cell(row, 6).Value = d.JimusekininshaName;
                row++;
            }

            ws.Columns().AdjustToContents();
            return wb;
        }
    }

}