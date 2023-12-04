using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Additional.ClientMessageDtos;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models.Additional;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Quqaz.Web.Services.Interfaces.Additional;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret.Additional
{
    public class ClientMessageService : CRUDService<ClientMessage, ClientMessageDto, CreateClientMessageDto, ClientMessageDto>, IClientMessageService
    {
        private readonly IWebHostEnvironment _env;
        public ClientMessageService(IRepository<ClientMessage> repository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService, IWebHostEnvironment env) : base(repository, mapper, logging, httpContextAccessorService)
        {
            _env = env;
        }

        public override async Task<ErrorRepsonse<ClientMessageDto>> AddAsync(CreateClientMessageDto createDto)
        {
            IFormFile file = createDto.Logo;
            string filePath = null;
            if (file != null)
            {
                var fileNames = file.FileName.Split('.');
                var folderDir = Path.Combine(_env.WebRootPath, "ClientMessages");
                if (!Directory.Exists(Path.Combine(_env.WebRootPath, "ClientMessages")))
                {
                    Directory.CreateDirectory(Path.Combine(folderDir));
                }

                var fileName = Guid.NewGuid().ToString() + "." + fileNames[^1];

                filePath = Path.Combine(folderDir, fileName);
                var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                filePath = Path.Combine("ClientMessages", fileName);
            }
            var message = new ClientMessage()
            {
                Message = createDto.Message,
                Name = createDto.Name,
                Logo = filePath,
                IsPublished = false
            };
            await _repository.AddAsync(message);
            return new ErrorRepsonse<ClientMessageDto>(new ClientMessageDto()
            {
                Id = message.Id,
                Logo = message.Logo,
                Message = message.Message
            });
        }

        public async Task Publish(int id)
        {
            var message = await _repository.GetById(id);
            message.IsPublished = true;
            await _repository.Update(message);
        }

        public async Task UnPublish(int id)
        {
            var message = await _repository.GetById(id);
            message.IsPublished = false;
            await _repository.Update(message);
        }
    }
}
