using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
    public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private const string CURRENT_SYMBOL = "$";

        private readonly IExpensesReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;
        }
        public async Task<byte[]> Execute(DateOnly dateRequest)
        {
            var loggedUser = await _loggedUser.Get();
            var expenses = await _repository.FilterByMonth(dateRequest, loggedUser);
            if (expenses.Count == 0) return [];

            using var workbook = GeneralWorkbookConfiguration(loggedUser.Name);

            var worksheet = workbook.Worksheets.Add($"{ResourceReportGenerationMessages.FILE_NAME} - {dateRequest:Y}");

            InsertHeader(worksheet);

            var row = 2;
            foreach (var item in expenses)
            {
                worksheet.Cell($"A{row}").Value = item.Title;
                worksheet.Cell($"B{row}").Value = item.Date;
                worksheet.Cell($"C{row}").Value = item.PaymentType.PaymentTypeToString();
                worksheet.Cell($"D{row}").Value = item.Amount;
                worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"-{CURRENT_SYMBOL} #,##0.00";
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
        private XLWorkbook GeneralWorkbookConfiguration(string author)
        {
            var workbook = new XLWorkbook();
            workbook.Author = author;
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            return workbook;
        }
        private void GeneralWorksheetConfiguration(IXLWorksheet worksheet)
        {
            worksheet.Cells().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cells().Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            worksheet.Columns().AdjustToContents();
        }
    }
}
