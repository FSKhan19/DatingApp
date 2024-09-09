using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Data;
using DatingApp.Backend.Models.User;
using AutoMapper;
using DatingApp.Backend.Consts;

namespace DatingApp.Backend.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DatingAppContext _context;
        private readonly IMapper _mapper;

        public UsersController(DatingAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<List<GetUser>>> Get()
        {
            var appUser = await _context.Users.Where(x => !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<GetUser>>(appUser);
        }

        // GET: Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUser>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
            if (appUser == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetUser>(appUser);
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<ActionResult<GetUser>> Create(CreateUser user)
        {

            if (ModelState.IsValid)
            {
                var appUser = _mapper.Map<AppUser>(user);               
                await _context.AddAsync(appUser);
                await _context.SaveChangesAsync();
                return _mapper.Map<GetUser>(appUser);
            }
            return BadRequest();
        }


        // PUT: Users/Update
        [HttpPut]
        public async Task<ActionResult<GetUser>> Update(UpdateUser user)
        {

            if (ModelState.IsValid)
            {
                var appUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == user.Id && !m.IsDeleted);

                if (appUser == null)
                    return NotFound(Error.Record.RECORD_NOT_FOUND);

                appUser.UserName = user.UserName;
                appUser.LastModificationTime = DateTime.Now;

                var updatedUser = _context.Update(appUser);
                await _context.SaveChangesAsync();
                return _mapper.Map<GetUser>(appUser);
            }
            return BadRequest();
        }

        // GET: Users/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
            if (appUser == null)
            {
                return NotFound(Error.Record.RECORD_NOT_FOUND);
            }
            appUser.IsDeleted = true;
            _context.Users.Update(appUser);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
