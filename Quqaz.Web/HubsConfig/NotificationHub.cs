﻿using Quqaz.Web.Dtos.NotifcationDtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.HubsConfig
{

    [Authorize]
    public class NotificationHub : Hub
    {

        public async Task AllNotification(string userId, NotificationDto[] notficationDto)
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
        public async Task AdminNotifcation(AdminNotification adminNotification)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("AdminNotification", adminNotification);
            }
        }
    }
}
