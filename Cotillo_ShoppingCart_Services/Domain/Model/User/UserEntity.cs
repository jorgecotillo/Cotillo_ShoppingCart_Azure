﻿using System;
using Microsoft.AspNet.Identity;

namespace Cotillo_ShoppingCart_Services.Domain.Model.User
{
    public class UserEntity : BaseEntity, IUser<int>
    {
        public string DisplayName { get; set; }
        public string ExternalAccount { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
