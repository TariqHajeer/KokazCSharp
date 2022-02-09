using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace KokazGoodsTransfer.Services.Concret
{
    public class ClientCashedService : CashService<Client, ClientDto, CreateClientDto, UpdateClientDto>, IClientCashedService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ClientPhone> _clientPhoneReposiotry;
        public ClientCashedService(IRepository<Client> repository, IMapper mapper, IMemoryCache cache, IRepository<Order> orderRepository, IRepository<ClientPhone> clientPhoneReposiotry) : base(repository, mapper, cache)
        {
            _orderRepository = orderRepository;
            _clientPhoneReposiotry = clientPhoneReposiotry;
        }
        public override async Task<IEnumerable<ClientDto>> GetCashed()
        {

            var name = typeof(Client).FullName;
            if (!_cache.TryGetValue(name, out IEnumerable<ClientDto> entites))
            {
                entites = await GetAsync(null, c => c.Country, c => c.ClientPhones);
                _cache.Set(name, entites);
            }
            return entites;
        }
        public override async Task<ErrorRepsonse<ClientDto>> Delete(int id)
        {
            var response = new ErrorRepsonse<ClientDto>();
            if (await _orderRepository.Any(c => c.ClientId == id))
            {
                response.Errors.Add("Cann't delete");
                return response;
            }
            return await base.Delete(id);
        }

        public async Task<ClientDto> GetById(int id)
        {
            var client = await _repository.FirstOrDefualt(c => c.Id == id, c => c.Country, c => c.ClientPhones, c => c.Orders);
            return _mapper.Map<ClientDto>(client);
        }
        public override async Task<ErrorRepsonse<ClientDto>> AddAsync(CreateClientDto createDto)
        {

            var response = new ErrorRepsonse<ClientDto>();
            if (await _repository.Any(c => c.UserName.ToLower() == createDto.UserName || c.Name.ToLower() == createDto.Name.ToLower()))
            {
                response.Errors.Add("Exisit");
                return response;
            }
            var client = _mapper.Map<Client>(createDto);
            await _repository.AddAsync(client);
            client = await _repository.GetById(client.Id);
            RemoveCash();
            return new ErrorRepsonse<ClientDto>(_mapper.Map<ClientDto>(client));
        }

        public async Task<ErrorRepsonse<PhoneDto>> AddPhone(AddPhoneDto addPhoneDto)
        {
            var client = await _repository.GetById(addPhoneDto.objectId);
            var response = new ErrorRepsonse<PhoneDto>();
            if (client == null)
            {
                response.Errors.Add("Not.Found");
                return response;
            }
            await _repository.LoadCollection(client, c => c.ClientPhones);
            if (client.ClientPhones.Any(c => c.Phone == addPhoneDto.Phone))
            {
                response.Errors.Add("Dublicate");
                return response;
            }
            var clientPhone = new ClientPhone()
            {
                ClientId = client.Id,
                Phone = addPhoneDto.Phone
            };
            client.ClientPhones.Add(clientPhone);
            await _repository.Update(client);
            response.Data = _mapper.Map<PhoneDto>(clientPhone);
            RemoveCash();
            return response;
        }

        public async Task DeletePhone(int id)
        {
            var phone = await _clientPhoneReposiotry.GetById(id);
            await _clientPhoneReposiotry.Delete(phone);
            RemoveCash();
        }
    }
}
