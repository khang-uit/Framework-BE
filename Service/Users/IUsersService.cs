using Memoriesx.Models;
using Microsoft.AspNetCore.Mvc;

namespace Memoriesx.Service.Users
{
    public interface IUsersService
    {
        User SignUp(User user);

        User SignIn(String email, String password);
    }
}
