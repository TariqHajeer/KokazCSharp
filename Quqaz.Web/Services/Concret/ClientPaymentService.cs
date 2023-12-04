using AutoMapper;
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
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class ClientPaymentService : IClientPaymentService
    {
        private readonly IRepository<ClientPayment> _clientPaymentRepository;
        private readonly IRepository<ClientPaymentDetail> _clientPaymentDetailRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public ClientPaymentService(IRepository<ClientPayment> repository, IMapper mapper, IWebHostEnvironment environment, IRepository<ClientPaymentDetail> clientPaymentDetailRepository)
        {
            _clientPaymentRepository = repository;
            _mapper = mapper;
            _environment = environment;
            _clientPaymentDetailRepository = clientPaymentDetailRepository;
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

        public Task<PagingResualt<ReceiptDto>> GetReceipByPrintNumberId(int printNumberId, PagingDto paging)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetReceiptAsHtml(int id)
        {
            var filePath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderReceipt.html";
            var readText = await File.ReadAllTextAsync(filePath);
            return readText;
        }
    }
}
