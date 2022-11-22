﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

    }
}
