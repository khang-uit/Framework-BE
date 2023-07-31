using Memoriesx.Data;
using Memoriesx.Models;
using Memoriesx.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Memoriesx.Service.Posts
{
    public class PostsService : IPostsService
    {
        private readonly MemoriesxDbContext _memoriesxDbContext;
        public PostsService(MemoriesxDbContext memoriesxDbContext)
        {
            _memoriesxDbContext = memoriesxDbContext;
        }
        public Post AddPost(Post post)
        {
            _memoriesxDbContext.Posts.Add(post);
            _memoriesxDbContext.SaveChanges();

            return post;
        }

        public bool DelPost(int id)
        {
            Post post = _memoriesxDbContext.Posts.Find(id);
            _memoriesxDbContext.Posts.Remove(post);
            _memoriesxDbContext.SaveChanges();
            return true;
        }

        public int CountPosts()
        {
            return _memoriesxDbContext.Posts.Count();
        }

        public List<Post> GetPost(int id)
        {

            return _memoriesxDbContext.Posts
                .Include(x => x.Likes)
                .Where(x => x.Id == id)
                .Select(x => new Post()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    Message = x.Message,
                    SelectedFile = x.SelectedFile,
                    CreatorId = x.CreatorId,
                    CreatedAt = x.CreatedAt,
                    Likes = x.Likes
                        .Where(y => y.PostId == x.Id)
                        .ToArray(),
                    Comments = x.Comments
                        .Where(y => y.PostId == x.Id)
                        .ToArray()
                })
                .ToList();

        }

        public List<Post> GetPosts(int page)
        {
            View viewed = _memoriesxDbContext.Views.Where(d => d.Date == System.DateTime.Now.Date).FirstOrDefault();
            if (viewed == null)
            {
                View newView = new View();
                newView.Count = 1;
                newView.Date = System.DateTime.Now;
                _memoriesxDbContext.Views.Add(newView);
            }
            else
            {
                viewed.Count += 1;
            }

            _memoriesxDbContext.SaveChanges();

            return _memoriesxDbContext.Posts
                .Include(x => x.Likes)
                .Skip(8 * page - 8)
                .Take(8)
                .Select(x => new Post()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    Message = x.Message,
                    SelectedFile = x.SelectedFile,
                    CreatorId = x.CreatorId,
                    CreatedAt = x.CreatedAt,
                    Likes = x.Likes
                        .Where(y => y.PostId == x.Id)
                        .ToArray(),
                    Comments = x.Comments
                        .Where(y => y.PostId == x.Id)
                        .ToArray()
                })
                .ToList();
        }

        public async Task<string> CheckToxic(string value)
        {
            using (var client = new HttpClient())
            {
                string comment_text = value;
                var myContent = JsonConvert.SerializeObject(new { comment_text = comment_text });

                var result = await client.PostAsync("http://127.0.0.1:5000/predict", new StringContent(myContent, Encoding.UTF8, "application/json"));

                if (result.Content != null)
                {
                    // Error Here
                    string responseContent = await result.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
            string error = "nothing";
            return error;
        }


        public bool UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
