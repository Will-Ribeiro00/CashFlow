using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
    public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private const string currentSymbol = "$";
        private readonly IExpensesReadOnlyRepository _repository;
        public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository) => _repository = repository;
        public async Task<byte[]> Execute(DateOnly dateRequest)
        {
            var expenses = await _repository.FilterByMonth(dateRequest);
            if (expenses.Count == 0) return [];

            using var workbook = GeneralWorkbookConfiguration();

            var worksheet = workbook.Worksheets.Add($"{ResourceReportGenerationMessages.FILE_NAME} - {dateRequest:Y}");

            InsertHeader(worksheet);

            var row = 2;
            foreach (var item in expenses)
            {
                worksheet.Cell($"A{row}").Value = item.Title;
                worksheet.Cell($"B{row}").Value = item.Date;
                worksheet.Cell($"C{row}").Value = item.PaymentType.PaymentTypeToString();
                worksheet.Cell($"D{row}").Value = item.Amount;
                worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"-{currentSymbol} #,##0.00";
                worksheet.Cell($"E{row}").Value = item.Description;

                row++;
            }
            GeneralWorksheetConfiguration(worksheet);

            var file = new MemoryStream();
            workbook.SaveAs(file);
            file.Position = 0;

            workbook.Dispose();
            return file.ToArray();
        }

        private void InsertHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
            worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
            worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
            worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
            worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;
            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#85c1e9");

            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }
        private XLWorkbook GeneralWorkbookConfiguration()
        {
            var workbook = new XLWorkbook();
            workbook.Author = "CashFlow API";
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            return workbook;
        }
        private void GeneralWorksheetConfiguration(IXLWorksheet worksheet)
        {
            worksheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.CellsUsed().Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            worksheet.Columns().AdjustToContents();
        }
    }
}
