using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
    {
        public const string currentSymbol = "$";
        public const int HEIGHT_ROW_EXPENSE_TABLE = 25;

        private readonly IExpensesReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;

            GlobalFontSettings.FontResolver = new ExpesesReportFontResolver();
        }
        public async Task<byte[]> Execute(DateOnly dateRequest)
        {
            var loggedUser = await _loggedUser.Get();
            var expenses = await _repository.FilterByMonth(dateRequest, loggedUser);
            if (expenses.Count == 0) return [];

            var document = CreateDocument(dateRequest, loggedUser.Name);
            var page = CreatePage(document);

            AddHeaderWithLogoPhotoAndName(page, loggedUser.Name);

            var totalExpenses = expenses.Sum(ex => ex.Amount);
            CreateTotalSpentSection(totalExpenses, dateRequest, page);

            foreach (var item in expenses)
            {
                var table = CreateExpenseTable(page);

                var row = table.AddRow();
                row.Height = HEIGHT_ROW_EXPENSE_TABLE;

                AddExpenseTitleInTable(row.Cells[0], item.Title);
                AddHeaderForAmountInTable(row.Cells[3]);

                row = table.AddRow();
                row.Height = HEIGHT_ROW_EXPENSE_TABLE;

                row.Cells[0].AddParagraph(item.Date.ToString("D"));
                SetStyleBaseForExpenseInformation(row.Cells[0]);
                row.Cells[0].Format.LeftIndent = 20;

                row.Cells[1].AddParagraph(item.Date.ToString("t"));
                SetStyleBaseForExpenseInformation(row.Cells[1]);

                row.Cells[2].AddParagraph(item.PaymentType.PaymentTypeToString());
                SetStyleBaseForExpenseInformation(row.Cells[2]);

                AddAmountForExpense(row.Cells[3], item.Amount);

                if (!string.IsNullOrWhiteSpace(item.Description))
                    AddDescriptionForExpense(table, item.Description, row);

                AddVisibilityBorder(table);
                AddWhiteSpace(table);
            }

            return RenderDocument(document);
        }

        private Document CreateDocument(DateOnly dateRequest, string author)
        {
            var document = new Document();
            document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {dateRequest:Y}";
            document.Info.Author = author;

            var style = document.Styles["Normal"];
            style!.Font.Name = FontHelper.DEFAULT_FONT;

            return document;
        }
        private Section CreatePage(Document document)
        {
            var section = document.AddSection();
            section.PageSetup = document.DefaultPageSetup.Clone();

            section.PageSetup.PageFormat = PageFormat.A4;

            section.PageSetup.LeftMargin = 40;
            section.PageSetup.RightMargin = 40;
            section.PageSetup.TopMargin = 80;
            section.PageSetup.BottomMargin = 80;

            return section;
        }
        private byte[] RenderDocument(Document document)
        {
            var render = new PdfDocumentRenderer
            {
                Document = document,
            };

            render.RenderDocument();

            using var file = new MemoryStream();
            render.PdfDocument.Save(file);
            return file.ToArray();
        }

        private void CreateTotalSpentSection(decimal totalExpenses, DateOnly dateRequest, Section page)
        {
            var paragraph = page.AddParagraph();
            paragraph.Format.SpaceBefore = "40";
            paragraph.Format.SpaceAfter = "40";

            var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, dateRequest.ToString("Y"));
            paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });

            paragraph.AddLineBreak();

            paragraph.AddFormattedText($"{totalExpenses:f2} {currentSymbol}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
        }
        private void AddHeaderWithLogoPhotoAndName(Section Page, string name)
        {
            var table = Page.AddTable();

            table.AddColumn();
            table.AddColumn("300");
            var row = table.AddRow();

            var assembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(assembly.Location);
            row.Cells[0].AddImage(Path.Combine(directoryName!, "Logo", "CashFlow-Color.png"));

            var greeting = string.Format(ResourceReportGenerationMessages.GREETING, $"{name}");
            row.Cells[1].AddParagraph($"{greeting}");
            row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
            row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        }
        private Table CreateExpenseTable(Section page)
        {
            var table = page.AddTable();

            table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;



            return table;
        }
        private void AddExpenseTitleInTable(Cell cell, string expenseTitle)
        {
            cell.AddParagraph(expenseTitle);
            cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.RED_LIGHT;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.MergeRight = 2;
            cell.Format.LeftIndent = 20;
        }
        private void AddHeaderForAmountInTable(Cell cell)
        {
            cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
            cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
            cell.Shading.Color = ColorsHelper.RED_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }
        private void SetStyleBaseForExpenseInformation(Cell cell)
        {
            cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.GREEN_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }
        private void AddAmountForExpense(Cell cell, decimal amount)
        {
            cell.AddParagraph($"-{amount:f2} {currentSymbol}");
            cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.WHITE;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }
        private void AddDescriptionForExpense(Table table, string descriptin, Row row)
        {
            var descriptionRow = table.AddRow();
            descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

            descriptionRow.Cells[0].AddParagraph(descriptin);
            descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
            descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
            descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            descriptionRow.Cells[0].MergeRight = 2;
            descriptionRow.Cells[0].Format.LeftIndent = 20;

            row.Cells[3].MergeDown = 1;
        }
        private void AddWhiteSpace(Table table)
        {
            var row = table.AddRow();
            row.Height = 30;
            row.Borders.Visible = false;
        }
        private void AddVisibilityBorder(Table table)
        {
            table.Borders.Width = 1;
            table.Borders.Visible = false;
            var row = table.Rows[0];
            row.Cells[0].Borders.Left.Visible = true;
            row.Cells[3].Borders.Right.Visible = true;
            row.Cells[2].Borders.Right.Visible = true;
            row.Borders.Top.Visible = true;
            row.Borders.Bottom.Visible = true;

            row = table.Rows[1];
            row.Cells[0].Borders.Left.Visible = true;
            row.Cells[3].Borders.Right.Visible = true;
            row.Cells[2].Borders.Right.Visible = true;
            row.Borders.Bottom.Visible = true;

            if (table.Rows.Count == 3)
            {
                row = table.Rows[2];
                row.Cells[0].Borders.Left.Visible = true;
                row.Cells[3].Borders.Right.Visible = true;
                row.Cells[2].Borders.Right.Visible = true;
                row.Borders.Bottom.Visible = true;
            }
        }
    }
}
