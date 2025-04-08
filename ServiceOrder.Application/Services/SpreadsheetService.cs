using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using log4net;
using ServiceOrder.Domain.DTOs;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder.Services.Services
{
    public class SpreadsheetService : ISpreadsheetService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SpreadsheetService));

        public MemoryStream ExportOrdersToExcel(List<OrderDTO> orders)
        {
            var stream = new MemoryStream();

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Projetos");

                    // Cabeçalhos
                    var headers = new[]
                    {
                    "ID", "Projeto", "Valor", "Cliente", "Cliente Final", "Concessionária",
                    "Recebido",
                    "Doc Enviado", "Dias Doc", "Status Doc",
                    "Doc Recebido", "Dias Rec", "Status Rec",
                    "Cadastro", "Dias Cad", "Status Cad",
                    "Envio", "Dias Env", "Status Env",
                    "Aprovação", "Dias Apr", "Status Apr",
                    "Vistoria", "Dias Vis", "Status Vis",
                    "Finalização", "Dias Fin", "Status Fin",
                    "Pagamento", "Dias Pag", "Status Pag"
                };

                    for (int i = 0; i < headers.Length; i++)
                        worksheet.Cell(1, i + 1).Value = headers[i];

                    // Conteúdo
                    for (int i = 0; i < orders.Count; i++)
                    {
                        var o = orders[i];
                        int row = i + 2;

                        worksheet.Cell(row, 1).Value = o.Order.Id;
                        worksheet.Cell(row, 2).Value = o.Order.OrderName;
                        worksheet.Cell(row, 3).Value = o.Order.ProjectValue;
                        worksheet.Cell(row, 4).Value = o.Order.Client?.Name;
                        worksheet.Cell(row, 5).Value = o.Order.FinalClient?.Name;
                        worksheet.Cell(row, 6).Value = o.Order.ElectricCompany?.Name;

                        worksheet.Cell(row, 7).Value = o.Order.ReceivedDate?.ToString("dd/MM/yyyy");

                        worksheet.Cell(row, 8).Value = o.Order.DocumentSentDate?.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 9).Value = o?.Deadline?.DocumentSentDays;
                        worksheet.Cell(row, 10).Value = o?.DocumentSentTooltip;

                        worksheet.Cell(row, 11).Value = o.Order.DocumentReceivedDate?.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 12).Value = o?.Deadline?.DocumentReceivedDays;
                        worksheet.Cell(row, 13).Value = o?.DocumentReceivedTooltip;

                        worksheet.Cell(row, 14).Value = o.Order.ProjectRegistrationDate?.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 15).Value = o?.Deadline?.ProjectRegistrationDays;
                        worksheet.Cell(row, 16).Value = o?.ProjectRegistrationTooltip;

                        worksheet.Cell(row, 17).Value = o.Order.ProjectSubmissionDate?.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 18).Value = o?.Deadline?.ProjectSubmissionDays;
                        worksheet.Cell(row, 19).Value = o?.ProjectSubmissionTooltip;

                        worksheet.Cell(row, 20).Value = o.Order.ProjectApprovalDate?.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 21).Value = o?.Deadline?.ProjectApprovalDays;
                        worksheet.Cell(row, 22).Value = o?.ProjectApprovalTooltip;

                        worksheet.Cell(row, 23).Value = o.Order.InspectionRequestDate?.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 24).Value = o?.Deadline?.InspectionRequestDays;
                        worksheet.Cell(row, 25).Value = o?.InspectionRequestTooltip;

                        worksheet.Cell(row, 26).Value = o.Order.FinalizationDate?.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 27).Value = o?.Deadline?.FinalizationDays;
                        worksheet.Cell(row, 28).Value = o?.FinalizationTooltip;

                        worksheet.Cell(row, 29).Value = o.Order.PaymentDate?.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 30).Value = o?.Deadline?.PaymentDays;
                        worksheet.Cell(row, 31).Value = o?.PaymentTooltip;
                    }

                    workbook.SaveAs(stream);
                }

                stream.Position = 0; // voltar ao início do stream para leitura
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao exportar ordens para Excel: " + ex.Message, ex);
                throw; // você pode lançar novamente ou retornar null, conforme sua estratégia
            }

            return stream;
        }
    }
}
