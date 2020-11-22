using Base.Core.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Model
{
    public class User : IdentityUser<Guid>
    {
        [DefaultValue(false)]
        public bool IsBanned { get; set; }
        [DefaultValue(false)]
        public bool Delete { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}
