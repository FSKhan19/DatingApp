using AutoMapper;
using DatingApp.Backend.Consts;
using DatingApp.Backend.Dtos.User;
using DatingApp.Backend.Models.User;
using DatingApp.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace DatingApp.Backend.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(IUserService userService, IMapper mapper, ITokenService tokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterUserDto>> Register(RegisterUserInput user)
        {
            var createUserInput = _mapper.Map<CreateUserInput>(user);
            var result = await _userService.CreateAsync(createUserInput);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            var userCredentials = result.Value;

            var token = _tokenService.CreateToken(userCredentials);

            return Ok(new RegisterUserDto
            {
                UserName = user.UserName,
                Token = token
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginUserDto>> Login(LoginUserInput user)
        {
            var res = await _userService.ValidateUsernameAsync(user.UserName);
            if (!res.IsSuccess)
                return Unauthorized(res.Error);

            var userCredentials = res.Value;

            using HMACSHA512 hmac = new HMACSHA512(userCredentials.PasswordSalt);
            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            if (!CryptographicOperations.FixedTimeEquals(userCredentials.PasswordHash, computedhash))
                return Unauthorized(Error.Record.INVALID_PASSWORD);

            var token = _tokenService.CreateToken(userCredentials);

            return Ok(new LoginUserDto
            {
                UserName = userCredentials.UserName,
                Token = token
            });
        }
    }
}
