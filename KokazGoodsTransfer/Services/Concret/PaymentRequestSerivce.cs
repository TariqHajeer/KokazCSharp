using AutoMapper;
using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.PayemntRequestDtos;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using LinqKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class PaymentRequestSerivce : IPaymentRequestSerivce
    {
        private readonly IRepository<PaymentRequest> _repository;
        private readonly IHttpContextAccessorService _httpContextAccessorService;
        private readonly NotificationHub _notificationHub;
        private readonly IMapper _mapper;
        public PaymentRequestSerivce(IRepository<PaymentRequest> repository, IMapper mapper, IHttpContextAccessorService httpContextAccessorService, NotificationHub notificationHub)
        {
            _repository = repository;
            _httpContextAccessorService = httpContextAccessorService;
            _notificationHub = notificationHub;
            _mapper = mapper;
        }

        public async Task Accept(int id)
        {
            var entity = await _repository.GetById(id);
            entity.Accept = true;
            await _repository.Update(entity);
        }

        public async Task<bool> CanClientRequest(int clientId)
        {
            return await _repository.Any(c => c.ClientId == clientId && c.Accept == null);
        }

        public async Task<PaymentRequestDto> Create(CreatePaymentRequestDto createPaymentRequestDto)
        {
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                PaymentWayId = createPaymentRequestDto.PaymentWayId,
                Note = createPaymentRequestDto.Note,
                ClientId = _httpContextAccessorService.AuthoticateUserId(),
                CreateDate = createPaymentRequestDto.Date,
                Accept = null
            };
            await _repository.AddAsync(paymentRequest);

            var adminNotification = new AdminNotification()
            {
                NewPaymentRequetsCount = await _repository.Count(c => c.Accept == null)
            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return _mapper.Map<PaymentRequestDto>(paymentRequest);
        }

        public async Task Delete(int id)
        {
            var paymentWay = await _repository.GetById(id);
            if (paymentWay.Accept != null)
            {
                throw new ConfilectException("");
            }
            await _repository.Delete(paymentWay);
        }

        public async Task DisAccept(int id)
        {
            var entity = await _repository.GetById(id);
            entity.Accept = false;
            await _repository.Update(entity);
        }

        public async Task<PagingResualt<IEnumerable<PaymentRequestDto>>> Get(PagingDto pagingDto, PaymentFilterDto paymentFilterDto)
        {
            var predicateBuilder = PredicateBuilder.New<PaymentRequest>(true);
            if (paymentFilterDto.Id != null)
            {
                predicateBuilder = predicateBuilder.And(c => c.Id.ToString().StartsWith(paymentFilterDto.Id.ToString()));
            }
            if (paymentFilterDto.ClientId != null)
            {
                predicateBuilder = predicateBuilder.And(c => c.ClientId == paymentFilterDto.ClientId);
            }
            if (paymentFilterDto.PaymentWayId != null)
            {
                predicateBuilder = predicateBuilder.And(c => c.PaymentWayId == paymentFilterDto.PaymentWayId);
            }
            if (paymentFilterDto.Accept != null)
            {
                predicateBuilder = predicateBuilder.And(c => c.Accept == paymentFilterDto.Accept);
            }
            if (paymentFilterDto.CreateDate != null)
            {
                predicateBuilder = predicateBuilder.And(c => c.CreateDate == paymentFilterDto.CreateDate);
            }
            var pagingResult = await _repository.GetAsync(pagingDto, predicateBuilder, c => c.Client, c => c.PaymentWay);
            return new PagingResualt<IEnumerable<PaymentRequestDto>>()
            {
                Data = _mapper.Map<IEnumerable<PaymentRequestDto>>(pagingResult.Data),
                Total = pagingResult.Total,
            };

        }

        public async Task<PagingResualt<IEnumerable<PaymentRequestDto>>> GetByClient(PagingDto pagingDto, int clientId)
        {
            var pagingResult = await _repository.GetAsync(paging: pagingDto, filter: c => c.ClientId == clientId, propertySelectors: new string[] { "PaymentWay" }, orderBy: c => c.OrderByDescending(c => c.Id));
            return new PagingResualt<IEnumerable<PaymentRequestDto>>()
            {
                Data = _mapper.Map<IEnumerable<PaymentRequestDto>>(pagingResult.Data),
                Total = pagingResult.Total,
            };
        }

        public async Task<IEnumerable<PaymentRequestDto>> New()
        {
            var list = await _repository.GetAsync(c => c.Accept == null, c => c.Client, c => c.PaymentWay);
            return _mapper.Map<IEnumerable<PaymentRequestDto>>(list);
        }
    }
}
