﻿using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.Api.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class ReportController : CashFlowBaseController
    {
        
        [HttpGet("Excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel([FromServices] IGenerateExpensesReportExcelUseCase useCase,
                                                  [FromQuery] DateOnly date)
        {
            byte[] file = await useCase.Execute(date);

            if(file.Length > 0) return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

            return NoContent();
        }

        [HttpGet("PDF")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPdf([FromServices] IGenerateExpensesReportPdfUseCase useCase,
                                                [FromQuery] DateOnly date)
        {
            byte[] file = await useCase.Execute(date);

            if (file.Length > 0) return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

            return NoContent();
        }
    }
}
