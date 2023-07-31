using Memoriesx.Models;
using Memoriesx.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Memoriesx.Service.Posts
{
    public interface IPostsService
    {
        List<Post> GetPosts(int page);
        int CountPosts();

        Post AddPost(Post post);
        List<Post> GetPost(int id);

        Boolean UpdatePost(Post post);
        Task<string> CheckToxic(string value);
        Boolean DelPost(int id);
    }
}
