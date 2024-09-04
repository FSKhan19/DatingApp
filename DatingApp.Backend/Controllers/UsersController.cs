using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DatingApp.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly DatingAppContext _context;

        public UsersController(DatingAppContext context)
        {
            _context = context;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return appUser;
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("UserName")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var appUser = await _context.Users.FindAsync(id);
        //    if (appUser == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(appUser);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UserName,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id")] AppUser appUser)
        //{
        //    if (id != appUser.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(appUser);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AppUserExists(appUser.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(appUser);
        //}

        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var appUser = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (appUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(appUser);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var appUser = await _context.Users.FindAsync(id);
        //    if (appUser != null)
        //    {
        //        _context.Users.Remove(appUser);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool AppUserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
