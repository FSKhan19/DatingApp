using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DatingApp.Backend.Validators.Common;
using DatingApp.Backend.Models.Common;
using DatingApp.Backend.Services.Interfaces;
using DatingApp.Backend.Dtos.User;
using DatingApp.Backend.Models.User;
using DatingApp.Backend.Models;

namespace DatingApp.Backend.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<UserDto>>> Get([FromQuery] PaginationQuery query)
        {
            // Validation is handled automatically by FluentValidation
            var (users, totalCount) = await _userService.GetAsync(query.PageNumber, query.PageSize);

            var response = new PaginatedResponse<UserDto>
            {
                Items = users,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };

            return Ok(response);
        }

        // GET: Users/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> Get([FromRoute] IdRequest input)
        {
            var result = await _userService.GetAsync(input.id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        //// POST: Users/Create
        //[HttpPost]
        //public async Task<ActionResult<UserDto>> Create(CreateUserInput input)
        //{
        //    var result = await _userService.CreateAsync(input);
        //    return result.IsSuccess ? CreatedAtAction(nameof(Create), new { id = result.Value.Id }, result.Value) : BadRequest(result.Error);
        //}


        //// PUT: Users/Update
        //[HttpPut("{id:int}")]
        //public async Task<ActionResult<UserDto>> Update([FromRoute] IdRequest id, UpdateUserInput input)
        //{
        //    input.Id = id.id; // Ensure the ID matches the route parameter
        //    var result = await _userService.UpdateAsync(input);
        //    return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        //}

        // GET: Users/Delete/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] IdRequest input)
        {
            var result = await _userService.DeleteAsync(input.id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }
    }
}
