using Azure.Core;
using Azure;
using Memoriesx.Data;
using Memoriesx.Models;
using Memoriesx.Models.Dto;
using Memoriesx.Service.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Http;
using Memoriesx.Service.EmailSender;
using Memoriesx.Service;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memoriesx.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
//    [Authorize]

    public class PostsController : ControllerBase
    {
        private readonly MemoriesxDbContext _memoriesxDbContext;
        private readonly IEmailSender _emailSender;
        private readonly IPostsService _postsService;
        public PostsController(IPostsService postsService, MemoriesxDbContext memoriesxDbContext, IEmailSender emailSender)
        {
            _postsService = postsService;
            _memoriesxDbContext = memoriesxDbContext;
            _emailSender =emailSender;

        }

        // GET: api/<PostsController>
        [HttpGet]
        public IActionResult Get(string page)
        {
            int numberPage = Int32.Parse(page); 
            List<Post> posts = _postsService.GetPosts(numberPage);
            int total = _postsService.CountPosts();
            return Ok(new {data = posts, currentPgae = numberPage, numberOfPages = (int)Math.Ceiling((double)total / 8)});
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Post post = _postsService.GetPost(id)[0];
            return Ok(new { post = post });
        }

        // POST api/<PostsController>
        [HttpPost, Authorize]
        public IActionResult Add([FromBody] Post post)
        {

            var id = User.FindFirstValue(ClaimTypes.Sid);
            post.CreatorId = Int32.Parse(id);
            Post newPost = _postsService.AddPost(post);

            return Ok(new { data = newPost});
        }

        // PATCH api/<PostsController>/5/likePost
        [HttpPatch("{id}/likePost")]
        public IActionResult Like(int id)
        {

            var userId = User.FindFirstValue(ClaimTypes.Sid);
            int uId = Int32.Parse(userId);
            IQueryable<Like> liked = _memoriesxDbContext.Likes.Where(x => x.PostId == id).Where(x => x.UserId == uId);
            if(liked.Count() <= 0)
            {
                Like like = new Like();
                like.UserId = uId;
                like.PostId = id;
                _memoriesxDbContext.Likes.Add(like);
                _memoriesxDbContext.SaveChanges();
            }
            else
            {
                _memoriesxDbContext.Likes.RemoveRange(liked);
                _memoriesxDbContext.SaveChanges();
            }

            Post getPost = _postsService.GetPost(id)[0];


            return Ok(new { post = getPost });
        }

        [HttpPost("{id}/commentPost")]
        public async Task<IActionResult> Comment([FromBody] CommentDto commentDto, [FromRoute]int id)
        {

            var userId = User.FindFirstValue(ClaimTypes.Sid);
            int uId = Int32.Parse(userId);

            string result = await _postsService.CheckToxic(commentDto.Value);
            ToxicDto ToxicPredict = JsonConvert.DeserializeObject<ToxicDto>(result);
            string predictResponse = " ";
            int count = 0;
            if(ToxicPredict.toxic > 0.8)
            {
                count++;
                predictResponse += "toxic, ";
            }
            if (ToxicPredict.severe_toxic > 0.8)
            {
                count++;
                predictResponse += "severe_toxic, ";
            }
            if (ToxicPredict.threat > 0.8)
            {
                count++;
                predictResponse += "threat, ";
            }
            if (ToxicPredict.obscene > 0.8)
            {
                count++;
                predictResponse += "obscene, ";
            }
            if (ToxicPredict.insult > 0.8)
            {
                count++;
                predictResponse += "insult, ";
            }
            if (ToxicPredict.identity_hate > 0.8)
            {
                count++;
                predictResponse += "identity_hate, ";
            }

            if(count > 0)
            {
                return BadRequest(new { msg = "Your comment has" + predictResponse, error = true});
            }
            else
            {
                Comment comment = new Comment();
                comment.UserId = uId;
                comment.PostId = id;
                comment.Content = commentDto.Value;
                _memoriesxDbContext.Comments.Add(comment);
                _memoriesxDbContext.SaveChanges();

                Post getPost = _postsService.GetPost(id)[0];

                return Ok(new { post = getPost, error = false});
            }           
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // POST api/<PostsController>/5/report
        [HttpPost("{id}/report")]
        public void Report(int id, [FromBody] ReportDto reportDto)
        {
            Post getPost = _postsService.GetPost(id)[0];
            User user = _memoriesxDbContext.Users.FirstOrDefault(d => d.Id == getPost.CreatorId);


            Report report = new Report();
            report.Message = reportDto.Msg;


            var userId = User.FindFirstValue(ClaimTypes.Sid);
            int uId = Int32.Parse(userId);

            report.CreatorId = uId;
            report.ReportedId = user.Id;
            report.PostId = id;

            _memoriesxDbContext.Reports.Add(report);
            _memoriesxDbContext.SaveChanges();

            var message = new MessageDto(new string[] { user.Email }, "Bạn bị báo cáo bài viết từ Memoriesx", "<a href='http://localhost:3000/posts/" + id + "'>Bấm vào để xem bài viết</a></hr>" + "Nội dung báo cáo:" + report.Message);
            _emailSender.SendEmail(message);
        }

        // GET: api/<PostsController>
        [HttpGet("reports")]
        public IActionResult GetReports()
        {
            List<Report> reports = _memoriesxDbContext.Reports
                .Include(x => x.Creator)
                .Select(x => new Report()
                {
                    Id = x.Id,
                    Message = x.Message,
                    Creator = x.Creator,
                    Post = x.Post,
                    Reported = x.Reported
                })
                .ToList();
            return Ok(new { data = reports });
        }

        [HttpGet("views")]
        public IActionResult GetViews()
        {
            List<View> views = _memoriesxDbContext.Views.ToList();
            return Ok(new { views });
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_postsService.DelPost(id));
        }
    }
}
