using AutoMapper;
using DatingApp.Backend.Consts;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Data;
using DatingApp.Backend.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace DatingApp.Backend.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DatingAppContext _context;
        private readonly IMapper _mapper;
        public AccountController(DatingAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<GetUser>> Register(RegisterUser user)
        {
            if (await IsUserExists(user.UserName))
                return BadRequest(Error.Record.USERNAME_ALREADY_TAKEN);

            using HMACSHA512 hmac = new HMACSHA512();
            var appUser = new AppUser()
            {
                UserName = user.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)),
                PasswordSalt = hmac.Key
            };
            await _context.Users.AddAsync(appUser);
            await _context.SaveChangesAsync();
            return _mapper.Map<GetUser>(appUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<GetUser>> Login(LoginUser user)
        {
            var appUser = await _context.Users.SingleOrDefaultAsync(x=>x.UserName == user.UserName);

            if (appUser == null)
                return Unauthorized(Error.Record.INVALID_USERNAME);

            using HMACSHA512 hmac = new HMACSHA512(appUser.PasswordSalt);
            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            if (appUser.PasswordHash == computedhash)
            {
                for (int i = 0; i < computedhash.Length; i++)
                {
                    if (appUser.PasswordHash[i] != computedhash[i])
                        return Unauthorized(Error.Record.INVALID_PASSWORD);
                }
            }

            return _mapper.Map<GetUser>(appUser);
        }

        private async Task<bool> IsUserExists(string userName)
        {
            return await _context.Users.AnyAsync(x=>x.UserName.ToLower() == userName.ToLower());
        }
    }
}
