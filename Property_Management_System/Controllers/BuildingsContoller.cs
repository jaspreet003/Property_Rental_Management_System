using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Property_Management_System.Data;
using Property_Management_System.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Property_Management_System.Controllers
{
    [Authorize(Roles = "Owner, Manager")]
    public class BuildingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BuildingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Buildings
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            return View(await _context.Buildings.ToListAsync());
        }

        // GET: Buildings/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Get all users.
            var users = await _userManager.Users.ToListAsync();

            // Filter the users who are managers.
            var managers = users.Where(u => _userManager.IsInRoleAsync(u, "Manager").Result).ToList();

            // Filter the users who are owners.
            var owners = users.Where(u => _userManager.IsInRoleAsync(u, "Owner").Result).ToList();

            // Pass the managers and owners to the view.
            ViewBag.Managers = new SelectList(managers, "Id", "UserName");
            ViewBag.Owners = new SelectList(owners, "Id", "UserName");

            return View();
        }

        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuildingId,NbFloor,NbApartment,Address,OwnerId,ManagerId")] Building building)
        {
                // If the user is an Owner, use the user's ID as the OwnerId.
                var ownerId = _userManager.GetUserId(User);
                var owner = await _userManager.FindByIdAsync(ownerId);
                building.OwnerId = ownerId;
                building.OwnerName = owner.UserName;

            // Set ManagerName.
            var manager = await _userManager.FindByIdAsync(building.ManagerId);
            building.ManagerName = manager.UserName;

            _context.Add(building);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Buildings/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            var users = await _userManager.Users.ToListAsync();
            // Filter the users who are managers.
            var managers = users.Where(u => _userManager.IsInRoleAsync(u, "Manager").Result).ToList();
            // Filter the users who are owners.
            var owners = users.Where(u => _userManager.IsInRoleAsync(u, "Owner").Result).ToList();
            // Query the users who are managers.
            ViewBag.Managers = new SelectList(managers, "Id", "UserName");
            // Query the users who are owners.
            ViewBag.Owners = new SelectList(owners, "Id", "UserName");

            return View(building);
        }

        // POST: Buildings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuildingId,NbFloor,NbApartment,Address,OwnerId,ManagerId")] Building building)
        {
            if (id != building.BuildingId)
            {
                return NotFound();
            }

            try
            {
                // Set OwnerName and ManagerName.
                var owner = await _userManager.FindByIdAsync(building.OwnerId);
                building.OwnerName = owner.UserName;
                var manager = await _userManager.FindByIdAsync(building.ManagerId);
                building.ManagerName = manager.UserName;

                _context.Update(building);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(building.BuildingId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.BuildingId == id);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var building = await _context.Buildings
                .FirstOrDefaultAsync(m => m.BuildingId == id);
            if (building == null)
            {
                return View("NotFound");
            }

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}