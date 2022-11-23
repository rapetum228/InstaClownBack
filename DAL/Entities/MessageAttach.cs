using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class MessageAttach: Attach
    {
        public Guid MessageId { get; set; }
        public Message Message { get; set; } = null!;
    }
}
