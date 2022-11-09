using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public User User { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTimeOffset DateTimeWriting { get; set; }
        public virtual Post Post { get; set; } = null!;
    }
}
