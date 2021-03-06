using Jwt.Demo.Common.Models;
using Jwt.Demo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jwt.Demo.Services
{
    public interface IUserService
    {
        Task<Role> CreateRole(Role role);
        Task<User> CreateUser(User user);
        Task<List<Role>> GetRoles();
        Task<User> GetUser(string username);
        System.Security.Claims.Claim[] GetUserClaims(User user);
        Task<List<User>> GetUsers();
        Task<User> Login(UserLoginRequest loginRequest);
    }
}