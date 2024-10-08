﻿using Application.DTOs.Request.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public record NotificationResponse(bool Flag = false, string Message = null!, List<NotificationDTO>? Notifications = null);
}
