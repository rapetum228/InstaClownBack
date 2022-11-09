using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public string? Location { get; set; }
        public DateTimeOffset DateTimeCreation { get; set; }
        //public Guid AuthorId { get; set; }
        public virtual User Author { get; set; } = null!;
        public virtual ICollection<PostAttach>? Attachments { get; set; } = new List<PostAttach>();
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
