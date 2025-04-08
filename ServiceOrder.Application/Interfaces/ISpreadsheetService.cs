using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ServiceOrder.Domain.DTOs;

namespace ServiceOrder.Services.Interfaces
{
    public interface ISpreadsheetService
    {
        public MemoryStream ExportOrdersToExcel(List<OrderDTO> orders);
    }
}
