using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public class PostRepo : IPostRepo
    {
        private MegyekDbContext dbContext;
        private IMeProvider meProvider;
        public PostRepo(MegyekDbContext dbContext, IMeProvider meProvider)
        {
            this.dbContext = dbContext;
            this.meProvider = meProvider;
        }

        public Team GetManagedTeam(int? teamId)
        {
            return meProvider.GetTeam(teamId);
        }

        public async Task<Post> GetPostAsync(int? postId)
        {
            return await dbContext.Post.FindAsync(postId);
        }

        public IList<Post> GetPostList(int? teamId, int count)
        {
            return meProvider.GetTeam(teamId)?.Post.TakeLast(count).OrderByDescending(x => x.Id).ToList();
        }

        public async Task AddPostAsync(Post post)
        {
            if (await meProvider.IsMemberOfTeamAsync(post?.TeamId))
            {
                dbContext.Post.Add(post);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task RemovePostAsync(Post post)
        {
            if (await meProvider.IsMemberOfTeamAsync(post?.TeamId))
            {
                dbContext.Post.Remove(post);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
