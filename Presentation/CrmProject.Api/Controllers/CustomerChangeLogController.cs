using ClosedXML.Excel;
using CrmProject.Application.DTOs.CustomerChangeLogDtos;
using CrmProject.Application.Services.CustomerChangeLogServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CrmProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerChangeLogController : ControllerBase
    {
        private readonly ICustomerChangeLogService _changeLogService;

        public CustomerChangeLogController(ICustomerChangeLogService changeLogService)
        {
            _changeLogService = changeLogService;
        }

        // JSON ile tüm loglar
        [HttpGet("all")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _changeLogService.GetAllLogsAsync();
            return Ok(logs);
        }

        // Belirli müşteri logları
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetLogsByCustomer(int customerId)
        {
            var logs = await _changeLogService.GetLogsByCustomerIdAsync(customerId);
            return Ok(logs);
        }

        // Excel olarak indir (tüm loglar)
        [HttpGet("excel")]
        public async Task<IActionResult> ExportAllLogsToExcel()
        {
            var logs = await _changeLogService.GetAllLogsAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("CustomerChangeLogs");

            // Başlıklar
            worksheet.Cell(1, 1).Value = "Firma Adı";
            worksheet.Cell(1, 2).Value = "Şube Adı";
            worksheet.Cell(1, 3).Value = "Sahip Adı";
            worksheet.Cell(1, 4).Value = "Değişen Alan";
            worksheet.Cell(1, 5).Value = "Eski Değer";
            worksheet.Cell(1, 6).Value = "Yeni Değer";
            worksheet.Cell(1, 7).Value = "Değiştiren Kullanıcı";
            worksheet.Cell(1, 8).Value = "Değişim Tarihi";

            worksheet.Range(1, 1, 1, 8).Style.Font.Bold = true; // Başlık kalın

            int row = 2;
            foreach (var log in logs)
            {
                worksheet.Cell(row, 1).Value = log.CompanyName;
                worksheet.Cell(row, 2).Value = log.BranchName;
                worksheet.Cell(row, 3).Value = log.OwnerName;
                worksheet.Cell(row, 4).Value = log.FieldName;

                worksheet.Cell(row, 5).Value = log.OldValue;
                worksheet.Cell(row, 5).Style.Fill.BackgroundColor = XLColor.LightPink; // Eski değer rengi

                worksheet.Cell(row, 6).Value = log.NewValue;
                worksheet.Cell(row, 6).Style.Fill.BackgroundColor = XLColor.LightGreen; // Yeni değer rengi

                worksheet.Cell(row, 7).Value = log.ChangedBy;
                worksheet.Cell(row, 8).Value = log.ChangedAt.ToString("yyyy-MM-dd HH:mm:ss");

                // Satır yüksekliği
                worksheet.Row(row).Height = 20;

                // Hücre kenarlıkları
                worksheet.Range(row, 1, row, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(row, 1, row, 8).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                row++;
            }

            worksheet.Columns().AdjustToContents(); // Kolon genişlikleri otomatik

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "CustomerChangeLogs.xlsx");
        }
    }
    }
