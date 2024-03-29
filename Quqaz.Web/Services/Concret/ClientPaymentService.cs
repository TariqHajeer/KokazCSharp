using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Dtos.ReceiptDtos;
using Quqaz.Web.Helpers.Extensions;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class ClientPaymentService : IClientPaymentService
    {
        private readonly IRepository<ClientPayment> _clientPaymentRepository;
        private readonly IHttpContextAccessorService _httpContextAccessorService;
        private readonly IRepository<ClientPaymentDetail> _clientPaymentDetailRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public ClientPaymentService(IRepository<ClientPayment> repository, IMapper mapper, IWebHostEnvironment environment, IRepository<ClientPaymentDetail> clientPaymentDetailRepository, IHttpContextAccessorService httpContextAccessorService)
        {
            _clientPaymentRepository = repository;
            _mapper = mapper;
            _environment = environment;
            _clientPaymentDetailRepository = clientPaymentDetailRepository;
            _httpContextAccessorService = httpContextAccessorService;
        }

        public async Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetClientprints(PagingDto paging, int? number, string code)
        {

            var exprtion = PredicateBuilder.New<ClientPayment>(c => c.DestinationName == _httpContextAccessorService.AuthoticateUserName());
            if (number != null)
            {
                exprtion = exprtion.And(c => c.Id == number);
            }
            if (!string.IsNullOrEmpty(code))
            {
                exprtion = exprtion.And(c => c.ClientPaymentDetails.Any(c => c.Code.StartsWith(code)));
            }
            var paginResult = await _clientPaymentRepository.GetAsync(paging, exprtion, null, c => c.OrderByDescending(c => c.Id));
            return new PagingResualt<IEnumerable<PrintOrdersDto>>()
            {
                Total = paginResult.Total,
                Data = _mapper.Map<IEnumerable<PrintOrdersDto>>(paginResult.Data)
            };
        }

        public async Task<PrintOrdersDto> GetFirst(Expression<Func<ClientPayment, bool>> filter, params string[] includes)
        {
            var payment = await _clientPaymentRepository.FirstOrDefualt(filter, includes);
            return _mapper.Map<PrintOrdersDto>(payment);
        }

        public async Task<PagingResualt<PrintDto>> GetOrdersByPrintNumberId(int printNumberId, PagingDto paging)
        {
            var data = await _clientPaymentDetailRepository.GetAsync(paging: paging, filter: c => c.ClientPaymentId == printNumberId);
            return new PagingResualt<PrintDto>()
            {
                Total = data.Total,
                Data = _mapper.Map<PrintDto>(data.Data)
            };
        }
        public async Task<string> GetReceiptAsHtml(int id)
        {
            var path = _environment.WebRootPath + "/HtmlTemplate/DeleiverMoneyForClientReports/DeleiverMoneyForClientReport.html";
            var readTextTask = File.ReadAllTextAsync(path);

            var report = await _clientPaymentRepository.FirstOrDefualt(c => c.Id == id, c => c.ClientPaymentDetails, c => c.Discounts, c => c.Receipts);
            var readText = await readTextTask;

            readText = readText.Replace("{{printNumber}}", report.Id.ToString());
            readText = readText.Replace("{{userName}}", report.PrinterName);
            readText = readText.Replace("{{dateOfPrint}}", report.Date.ToString("yyyy-MM-dd"));
            readText = readText.Replace("{{timeOfPrint}}", report.Date.ToString("HH:mm"));
            readText = readText.Replace("{{clientName}}", report.DestinationName);
            readText = readText.Replace("{{clientPhones}}", report.DestinationPhone);
            readText = readText.Replace("{{orderplacedName}}", string.Join('-', report.GetOrdersPalced().Select(c => c.GetDescription())));
            readText = readText.Replace("{{ordersCounts}}", report.ClientPaymentDetails.Count().ToString());
            readText = readText.Replace("{{TotalCost}}", report.ClientPaymentDetails.Sum(c => c.Total).ToString());
            readText = readText.Replace("{{deliveryCostCount}}", report.ClientPaymentDetails.Sum(c => c.DeliveryCost).ToString());
            var payForClient = report.ClientPaymentDetails.Sum(c => c.Total - c.DeliveryCost);
            readText = readText.Replace("{{PayForClient}}", payForClient.ToString());

            var rows = new StringBuilder();
            var c = 1;
            foreach (var item in report.ClientPaymentDetails)
            {

                rows.Append(@"<tr style=""border: 1px black solid;padding: 5px;text-align: center;margin-bottom: 20%;overflow: auto;"">");
                rows.Append(@"<td style=""width: 3%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(c.ToString());
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 5%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Code);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 15%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Country);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 15%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Date.Value.ToString("yyyy-mm-dd"));
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 10%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Total);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.DeliveryCost);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.PayForClient);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.LastTotal?.ToString());
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.RecipientPhones);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.ClientNote?.ToString());
                rows.Append("</td>");

                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Note);
                rows.Append("</td>");
                rows.Append("</tr>");
                c++;
            }
            readText = readText.Replace("{{orders}}", rows.ToString());
            rows.Clear();
            if (report.Discounts.Any())
            {
                var discountRowTemplate = await File.ReadAllTextAsync(_environment.WebRootPath + "/HtmlTemplate/DeleiverMoneyForClientReports/DiscountRow.Html");
                foreach (var item in report.Discounts)
                {
                    var tempDiscountRow = discountRowTemplate.Replace("{{points}}", item.Points.ToString());
                    tempDiscountRow = tempDiscountRow.Replace("{{pointsMoney}}", item.Money.ToString());
                    payForClient -= item.Money;
                    tempDiscountRow = tempDiscountRow.Replace("{{pointsmoneyclientCalctotal}}", payForClient.ToString());
                    rows.AppendLine(tempDiscountRow);
                }
                readText = readText.Replace("{{discountRow}}", rows.ToString());
            }
            else
            {
                readText = readText.Replace("{{discountRow}}", string.Empty);
            }

            if (report.Receipts?.Any() != true)
            {
                readText = readText.Replace("{{ReceiptsHtml}}", "");
                return readText;
            }
            var receiptsHtml = _environment.WebRootPath + "/HtmlTemplate/DeleiverMoneyForClientReports/Receipts.html";
            rows.Clear();
            var receiptsTotal = report.Receipts.Sum(c => c.Amount);
            payForClient -= receiptsTotal;
            receiptsHtml = receiptsHtml.Replace("{{total}}", payForClient.ToString());
            receiptsHtml = receiptsHtml.Replace("{{reportstotal}}", receiptsTotal.ToString());
            c = 1;
            foreach (var item in report.Receipts)
            {
                rows.Append(@"<tr style=""border: 1px black solid;padding: 5px;text-align: center;margin-bottom: 20%;overflow: auto;"">");
                rows.Append(@"<td style=""width: 3%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(c.ToString());
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 5%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Id.ToString());
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 15%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Date.ToString("yyyy-mm-dd"));
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 15%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.IsPay ? "صرف" : "قبض");
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 10%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Amount.ToString());
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.About);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Note);
                rows.Append("</td>");
            }
            receiptsHtml = receiptsHtml.Replace("{{reports}}", rows.ToString());
            readText = readText.Replace("{{ReceiptsHtml}}", receiptsHtml);

            return readText;
        }
    }
}
