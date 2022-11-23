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
        public string Text { get; set; } = null!;
        public DateTimeOffset DateTimeWriting { get; set; }
        public Comment? ResponseComment { get; set; }
        //public virtual ICollection<Comment>? Comments { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;
    }
}
