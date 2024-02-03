using AutoMapper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Quqaz.Web.Helpers;
using Quqaz.Web.HubsConfig;
using Quqaz.Web.Dtos.NotifcationDtos;
using Quqaz.Web.CustomException;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Quqaz.Web.Services.Concret
{
    public class ClientCashedService : CashService<Client, ClientDto, CreateClientDto, UpdateClientDto>, IClientCashedService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ClientPhone> _clientPhoneReposiotry;
        private readonly IUintOfWork _uintOfWork;
        private readonly NotificationHub _notificationHub;
        private readonly new IClientRepository _repository;
        private readonly IWebHostEnvironment _environment;
        public ClientCashedService(IClientRepository repository, IMapper mapper, IMemoryCache cache, IRepository<Order> orderRepository, IRepository<ClientPhone> clientPhoneReposiotry, IUintOfWork uintOfWork, Logging logging, IHttpContextAccessorService httpContextAccessorService, NotificationHub notificationHub, IWebHostEnvironment environment)
            : base(repository, mapper, cache, logging, httpContextAccessorService)
        {
            _orderRepository = orderRepository;
            _clientPhoneReposiotry = clientPhoneReposiotry;
            _uintOfWork = uintOfWork;
            _notificationHub = notificationHub;
            this._repository = repository;
            _environment = environment;
        }
        public override async Task<IEnumerable<ClientDto>> GetCashed()
        {
            if (!_cache.TryGetValue(cashName, out IEnumerable<ClientDto> entites))
            {
                entites = await GetAsync(null, c => c.Country, c => c.ClientPhones);
                _cache.Set(cashName, entites);
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

        public override async Task<ClientDto> GetById(int id)
        {
            var client = await _repository.FirstOrDefualt(c => c.Id == id, c => c.Country, c => c.ClientPhones);
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
        public override async Task<ErrorRepsonse<ClientDto>> Update(UpdateClientDto updateDto)
        {
            var client = await _repository.GetById(updateDto.Id);
            _mapper.Map(updateDto, client);
            await _repository.Update(client);
            RemoveCash();
            return new ErrorRepsonse<ClientDto>(_mapper.Map<ClientDto>(client));
        }

        public async Task<ErrorRepsonse<ClientDto>> GivePoints(GiveOrDiscountPointsDto giveOrDiscountPointsDto)
        {
            var client = await _uintOfWork.Repository<Client>().GetById(giveOrDiscountPointsDto.ClientId);

            await _uintOfWork.BegeinTransaction();
            try
            {
                string sen = "";
                if (giveOrDiscountPointsDto.IsGive)
                {
                    client.Points += giveOrDiscountPointsDto.Points;
                    sen += $"تم إهدائك {giveOrDiscountPointsDto.Points} نقاط";
                }
                else
                {
                    client.Points -= giveOrDiscountPointsDto.Points;
                    sen += $"تم خصم {giveOrDiscountPointsDto.Points} نقاط منك";
                }
                Notfication notfication = new Notfication()
                {
                    ClientId = client.Id,
                    Note = sen,
                };
                await _uintOfWork.Repository<Client>().Update(client);
                await _uintOfWork.Repository<Notfication>().AddAsync(notfication);
                await _uintOfWork.Commit();
                RemoveCash();
                return new ErrorRepsonse<ClientDto>() { Data = _mapper.Map<ClientDto>(client) };
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
                throw ex;
            }

        }

        public async Task<int> Account(AccountDto accountDto)
        {
            var userId = _httpContextAccessorService.AuthoticateUserId();

            await _uintOfWork.BegeinTransaction();
            Receipt receipt = new Receipt()
            {
                IsPay = accountDto.IsPay,
                About = accountDto.About,
                CreatedBy = _httpContextAccessorService.AuthoticateUserName(),
                ClientId = accountDto.ClinetId,
                Date = DateTime.Now,
                Amount = accountDto.Amount,
                Manager = accountDto.Manager,
                Note = accountDto.Note,
            };

            await _uintOfWork.Add(receipt);
            var treasuer = await _uintOfWork.Repository<Treasury>().FirstOrDefualt(c => c.Id == userId);
            treasuer.Total += accountDto.Amount;
            await _uintOfWork.Update(treasuer);
            var history = new TreasuryHistory()
            {
                Amount = accountDto.Amount,
                ReceiptId = receipt.Id,
                TreasuryId = treasuer.Id,
                CreatedOnUtc = DateTime.Now,
            };
            await _uintOfWork.Add(history);
            await _uintOfWork.Commit();
            return receipt.Id;

        }

        public async Task<AuthClient> GetAuthClient()
        {
            var client = await _repository.FirstOrDefualt(c => c.Id == _httpContextAccessorService.AuthoticateUserId(), c => c.ClientPhones, c => c.Country);
            return _mapper.Map<AuthClient>(client);
        }

        public async Task Update(CUpdateClientDto updateClientDto)
        {

            await _uintOfWork.BegeinTransaction();
            var client = await _uintOfWork.Repository<Client>().FirstOrDefualt(c => c.Id == _httpContextAccessorService.AuthoticateUserId());
            var clientName = client.Name;
            var clientUserName = client.UserName;
            var oldPassword = client.Password;
            client = _mapper.Map(updateClientDto, client);
            client.Name = clientName;
            client.UserName = clientUserName;
            client.Password = oldPassword;
            await _uintOfWork.Repository<Client>().LoadCollection(client, c => c.ClientPhones);

            if (updateClientDto.Phones?.Any() == true)
            {
                client.ClientPhones.Clear();
                foreach (var item in updateClientDto.Phones)
                {
                    client.ClientPhones.Add(new ClientPhone()
                    {
                        Phone = item,
                    });
                }
            }
            if (updateClientDto.Photo != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "clientphoto");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create a unique filename to avoid overwriting existing files
                var file = updateClientDto.Photo;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                client.Photo = filePath;
            }

            await _uintOfWork.Update(client);
            bool isEditRequest = clientName != updateClientDto.Name || clientUserName != updateClientDto.UserName;
            if (isEditRequest)
            {
                EditRequest editRequest = new EditRequest();
                if (clientName != updateClientDto.Name)
                {
                    editRequest.OldName = clientName;
                    editRequest.NewName = updateClientDto.Name;
                }
                if (clientUserName != updateClientDto.UserName)
                {
                    editRequest.OldUserName = clientUserName;
                    editRequest.NewUserName = updateClientDto.UserName;
                }
                editRequest.Accept = null;
                editRequest.ClientId = _httpContextAccessorService.AuthoticateUserId();
                editRequest.UserId = null;
                await _uintOfWork.Add(editRequest);

            }
            await _uintOfWork.Commit();
            if (isEditRequest)
            {
                var newEditRquests = await _uintOfWork.Repository<EditRequest>().Count(c => c.Accept == null);

                var adminNotification = new AdminNotification()
                {
                    NewEditRquests = newEditRquests,
                };
                await _notificationHub.AdminNotifcation(adminNotification);
            }
        }

        public async Task<List<ClientDto>> GetClientsByBranchId(int branchId)
        {
            var clients = await _repository.GetClientsByBranchId(branchId);
            return _mapper.Map<List<ClientDto>>(clients);
        }

        public async Task UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var client = await _repository.GetById(_httpContextAccessorService.AuthoticateUserId());
            if (!MD5Hash.VerifyMd5Hash(updatePasswordDto.OldPassword, client.Password))
            {
                throw new ConflictException("كلمة السر خطأ");
            }
            client.Password = MD5Hash.GetMd5Hash(updatePasswordDto.NewPassowrd);
            await _repository.Update(client);
        }

        public async Task SetToken(string token)
        {
            var client = await _repository.GetById(_httpContextAccessorService.AuthoticateUserId());
            client.FirebaseToken = token;
            await _repository.Update(client);            
        }
    }
}
