using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace Instagram.Dal.Entities;

public class Comment
{
    public long CommentId { get; set; }
    public string Body { get; set; }
    public DateTime CreatedTime { get; set; }
    public long AccountId { get; set; }
    public Account Account { get; set; }
    public long PostId { get; set; }
    public Post Post { get; set; }
    public Comment ParentComment { get; set; }
    public List<Comment> Replies { get; set; }

}
