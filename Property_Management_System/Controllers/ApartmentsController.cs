using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Property_Management_System.Data;
using Property_Management_System.Models;
using System.Threading.Tasks;

[Authorize]
public class ApartmentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApartmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Index, Details, Create, Edit actions go here
    [Authorize(Roles = "Owner,Manager")]

    // GET: Apartment/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var Apartment = await _context.Apartments
            .FirstOrDefaultAsync(m => m.ApartmentId == id);
        if (Apartment == null)
        {
            return NotFound();
        }

        if (User.IsInRole("Manager"))
        {
            return Unauthorized();
        }

        return View(Apartment);
    }
    [Authorize(Roles = "Owner,Manager")]

    // POST: Apartment/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (User.IsInRole("Manager"))
        {
            return Unauthorized();
        }

        var Apartment = await _context.Apartments.FindAsync(id);
        _context.Apartments.Remove(Apartment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Apartment
    public async Task<IActionResult> Index()
    {
        // Get the buildings and convert to a dictionary for quick lookup.
        var buildingDictionary = _context.Buildings.ToDictionary(b => b.BuildingId, b => b.Address);

        // Get the apartments and include the Building addresses.
        var apartmentsWithBuildingInfo = await _context.Apartments
            .Select(apartment => new
            {
                Apartment = apartment,
                BuildingAddress = buildingDictionary.ContainsKey(apartment.BuildingId) ? buildingDictionary[apartment.BuildingId] : "Not found"
            })
            .ToListAsync();

        // Create a ViewModel if one doesn't exist and pass the data to the view.
        // Assuming you have a ViewModel that can hold both Apartment and BuildingAddress, 
        // otherwise, you need to create one.

        var viewModelList = apartmentsWithBuildingInfo.Select(a => new ApartmentViewModel
        {
            // ... Map other properties as needed
            Floor = a.Apartment.Floor,
            ApartmentNumber = a.Apartment.ApartmentNumber.ToString(),
            RentPerMonth = a.Apartment.RentPerMonth,
            NumberOfRooms = a.Apartment.NumberOfRooms,
            NumberOfBathrooms = a.Apartment.NumberOfBathrooms,
            Status = a.Apartment.status,
            BuildingId = a.Apartment.BuildingId,
            BuildingAddress = a.BuildingAddress // Add this property to your ViewModel
        }).ToList();

        // Pass the buildings to the view in ViewBag.
        ViewBag.Buildings = new SelectList(buildingDictionary, "Key", "Value");

        return View(viewModelList); // Pass the ViewModel list to the view
    }


    // GET: Apartment/Create
    [Authorize(Roles = "Owner,Manager")]

    public IActionResult Create()
    {
        // Get the current user's ID.
        var currentUserId = _userManager.GetUserId(User);

        // Filter buildings based on current user's ownership or management.
        var buildings = _context.Buildings
                                .Where(b => b.OwnerId == currentUserId || b.ManagerId == currentUserId)
                                .ToList();

        // Pass the buildings to the view.
        ViewBag.Buildings = new SelectList(buildings, "BuildingId","Address");

        return View();
    }

    [Authorize(Roles = "Owner,Manager")]

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ApartmentId,ApartmentNumber,Floor,RentPerMonth,NumberOfRooms,NumberOfBathrooms,BuildingId, status")] Apartment apartment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(apartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(apartment);
    }
    [Authorize(Roles = "Owner,Manager")]

    // GET: Apartment/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var Apartment = await _context.Apartments.FindAsync(id);
        if (Apartment == null)
        {
            return NotFound();
        }
        return View(Apartment);
    }
    [Authorize(Roles = "Owner,Manager")]

    // POST: Apartment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ApartmentId,ApartmentNumber,Floor,RentPerMonth,NumberOfRooms,NumberOfBathrooms,BuildingId, ")] Apartment Apartment)
    {
        if (id != Apartment.ApartmentId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(Apartment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentExists(Apartment.ApartmentId))
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
        return View(Apartment);
    }

    private bool ApartmentExists(int id)
    {
        return _context.Apartments.Any(e => e.ApartmentId == id);
    }
}