using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Property_Management_System.Data;
using Property_Management_System.Models;

[Authorize(Roles = "Owner")]
public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    private int GetCurrentOwnerBuildingId()
    {
        var ownerId = _userManager.GetUserId(User);
        var property = _context.Buildings.FirstOrDefault(p => p.OwnerId == ownerId);
        return property?.BuildingId ?? 0;
    }
    // GET: Users
    public async Task<IActionResult> Index()
    {
        List<ApplicationUser> users;
        users = await _userManager.Users.ToListAsync();

        return View(users);
    }


    // GET: Users/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ApplicationUser user, string password, string role)
    {
        if (ModelState.IsValid)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, token);

                await _userManager.AddToRoleAsync(user, role);
                await _userManager.UpdateSecurityStampAsync(user); // Force user to change password at next login
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(user);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ApplicationUser user, string role)
    {
        if (id != user.Id)
        {
            return View("NotFound");
        }

        var existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return View("NotFound");
        }

        // Update the properties of the existing user
        existingUser.UserName = user.UserName;
        existingUser.Email = user.Email;
        // Add any other properties that you want to update

        var result = await _userManager.UpdateAsync(existingUser);
        if (result.Succeeded)
        {
            var currentRoles = await _userManager.GetRolesAsync(existingUser);
            await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
            await _userManager.AddToRoleAsync(existingUser, role);
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(user);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _context.Users.FindAsync(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    private bool UserExists(string id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
