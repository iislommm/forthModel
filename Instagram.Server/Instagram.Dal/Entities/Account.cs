using System.Reflection.Metadata.Ecma335;

namespace Instagram.Dal.Entities;

public class Account
{
    public long AccountId { get; set; }
    public string UserName { get; set; }
    public string Bio { get; set; }
    public List<Account> Followers { get; set; }
    public List<Account> Following { get; set; }
    public List<Post> Posts { get; set; }
    public List<Comment> Comments { get; set; }
}
