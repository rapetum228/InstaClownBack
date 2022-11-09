﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; 
        public DateTimeOffset BirthDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Post>? Posts { get; set; } 
        public virtual ICollection<User>? Subscribers { get; set; } 
        public Guid? AvatarId { get; set; }
        public virtual Avatar? Avatar { get; set; }
        public virtual ICollection<UserSession>? Sessions { get; set; }
    }
}
