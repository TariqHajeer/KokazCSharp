using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Dtos.ReceiptDtos;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
            var filePath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderReceipt.html";
            var readText = await File.ReadAllTextAsync(filePath);
            return readText;
        }
    }
}
