using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<User> Participants { get; set; } = null!;
    }
}
