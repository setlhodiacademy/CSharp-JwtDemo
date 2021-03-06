using Jwt.Demo.Common.Models;
//using Jwt.Demo.Entities;
using Jwt.Demo.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Jwt.Demo.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtDemoContext _context;

        public UserService(IJwtDemoContext context)
        {
            _context = context;
        }

        public async Task<Entities.User> Login(UserLoginRequest loginRequest)
        {
            // TODO: GOTO ACTIVE DIRECTORY
            // TODO: DO ALL OTHER CHECKS
            var user = await GetUser(loginRequest.Username);
            if (user == null)
                throw new Exception($"Invalid Credentials");

            return user;
        }

        public async Task<Entities.User> CreateUser(Entities.User user)
        {
            // var users = await GetUsers();
            // user.RoleId = users.Count() + 1;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(new CancellationToken());
            return user;
        }

        public async Task<Entities.Role> CreateRole(Entities.Role role)
        {
            // var roles = await GetRoles();
            // role.RoleId = roles.Count() + 1;
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync(new CancellationToken());
            return role;
        }

        public async Task<Entities.User> GetUser(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<Entities.User>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ThenInclude(u => u.RoleClaims)
                .ThenInclude(u => u.Claim)
                .ToListAsync();
        }
        public async Task<List<Entities.Role>> GetRoles()
        {
            return await _context.Roles
                .Include(r => r.RoleClaims)
                .ThenInclude(r => r.Claim)
                .ToListAsync();
        }

        public Claim[] GetUserClaims(Entities.User user)
        {

            var foundUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);

            if (foundUser == null)
                throw new Exception($"User: ({user.Username}) does not exist.");

            var roleClaims = GetClaims(foundUser.RoleId);

            roleClaims.Add(new Claim("UserId", foundUser.UserId.ToString()));
            roleClaims.Add(new Claim("Username", foundUser.Username?.ToLower()));
            roleClaims.Add(new Claim("FullName", $"{foundUser.FirstName} {foundUser.Surname}"));
            return roleClaims.ToArray();
        }

        private List<Claim> GetClaims(int roleId)
        {
            return _context.RoleClaims.Include(o => o.Claim).Where(x => x.RoleId == roleId)
                .Select(c => new System.Security.Claims.Claim(c.Claim.ClaimName, c.Claim.ClaimName))
                .ToList();
        }
    }
}
