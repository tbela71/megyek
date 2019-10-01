using System.Collections.Generic;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public interface IPostRepo
    {
        Team GetManagedTeam(int? teamId);
        IList<Post> GetPostList(int? id, int v);
        Task AddPostAsync(Post post);
        Task<Post> GetPostAsync(int? postId);
        Task RemovePostAsync(Post post);
    }
}