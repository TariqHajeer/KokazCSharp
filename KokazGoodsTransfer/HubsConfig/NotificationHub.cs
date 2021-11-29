using KokazGoodsTransfer.Dtos.NotifcationDtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.HubsConfig
{

    [Authorize]
    public class NotificationHub : Hub
    {
        //public NotificationHub()
        //{

        //}
        public async Task AllNotification(string userId, NotficationDto[] notficationDto)
        {

            var x = JsonConvert.SerializeObject(new { notifications = notficationDto });
            if (Clients != null)
            {
                var user = Clients.User(userId);
                if (user != null)
                {
                    await Clients.User(userId).SendAsync("RM", x);
                }
            }

        }
        public async Task Test(string userId, string x)
        {
            if (Clients != null)
            {
                var user = Clients.User(userId);
                if (user != null)
                {
                    await Clients.User(userId).SendAsync("RM", x);
                }
            }

        }


        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
