using Base.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.ViewModel
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBanned { get; set; }
        public UserStatus UserStatus { get; set; }
        public bool Delete { get; set; }
    }
}
