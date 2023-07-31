using Memoriesx.Data;
using Memoriesx.Models;
using Memoriesx.Service.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net;


namespace Memoriesx.Service.Users
{
    public class UsersService : IUsersService
    {
        private readonly MemoriesxDbContext _memoriesxDbContext;
        public UsersService(MemoriesxDbContext memoriesxDbContext)
        {
            _memoriesxDbContext = memoriesxDbContext;
        }
        public User SignUp(User user)
        {
            _memoriesxDbContext.Users.Add(user);
            _memoriesxDbContext.SaveChanges();

            return user;
        }

        public User SignIn(String email, String password)
        {
            User user = _memoriesxDbContext.Users.FirstOrDefault(d => d.Email == email);
            return user;
        }
    }
}
