using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Property_Management_System.Models;
using Property_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

public class VisitsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public VisitsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var role = await _userManager.GetRolesAsync(user);

        IQueryable<Visit> visits;

        if (role.Contains("Tenant"))
        {
            visits = _context.Visits.Where(v => v.TenantId == user.Id);
        }
        else if (role.Contains("Manager") )
        {
            visits = _context.Visits.Where(v => v.Building.ManagerId == user.Id);
        }
        else if (role.Contains("Owner"))
        {
            visits = _context.Visits;
        }
        else
        {
            return Unauthorized();
        }

        return View(await visits.ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);
        var role = await _userManager.GetRolesAsync(user);

        IQueryable<Building> buildings;
        List<ApplicationUser> tenants;

        if (role.Contains("Owner"))
        {
            buildings = from b in _context.Buildings
                        join a in _context.Apartments on b.BuildingId equals a.BuildingId
                        select b;
            tenants = (await _userManager.GetUsersInRoleAsync("Tenant")).ToList();
        }
        
        else if (role.Contains("Manager"))
        {
            buildings = from b in _context.Buildings
                        join a in _context.Apartments on b.BuildingId equals a.BuildingId
                        where b.ManagerId == user.Id
                        select b;
            tenants = (await _userManager.GetUsersInRoleAsync("Tenant")).ToList();
        }
        else if (role.Contains("Tenant"))
        {
            buildings = from b in _context.Buildings
                        join a in _context.Apartments on b.BuildingId equals a.BuildingId
                        select b;
            tenants = new List<ApplicationUser> { user };
        }
        else
        {
            return Unauthorized();
        }

        ViewData["BuildingId"] = new SelectList(await buildings.Distinct().ToListAsync(), "BuildingId", "Address");
        ViewData["TenantId"] = new SelectList(tenants, "Id", "UserName");
        if (buildings.Any())
        {
            var apartments = _context.Apartments.Where(a => a.BuildingId == buildings.First().BuildingId);
            ViewData["ApartmentId"] = new SelectList(await apartments.ToListAsync(), "ApartmentId", "ApartmentNumber");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("VisitId,VisitDate,Slot,TenantId,BuildingId,ApartmentId,OwnerId")] Visit visit)
    {
        var user = await _userManager.GetUserAsync(User);
        var role = await _userManager.GetRolesAsync(user);

        var building = await _context.Buildings.FindAsync(visit.BuildingId);
        visit.OwnerId = building.OwnerId;
        visit.ManagerId = building.ManagerId;

        _context.Add(visit);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var visit = await _context.Visits.FindAsync(id);
        if (visit == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        var role = await _userManager.GetRolesAsync(user);

        IQueryable<Building> buildings;
        List<ApplicationUser> tenants;

        if (role.Contains("Owner"))
        {
            buildings = from b in _context.Buildings
                        join a in _context.Apartments on b.BuildingId equals a.BuildingId
                        select b;
            tenants = (await _userManager.GetUsersInRoleAsync("Tenant")).ToList();
        }
       
        else if (role.Contains("Manager"))
        {
            buildings = from b in _context.Buildings
                        join a in _context.Apartments on b.BuildingId equals a.BuildingId
                        where b.ManagerId == user.Id
                        select b;
            tenants = (await _userManager.GetUsersInRoleAsync("Tenant")).ToList();
        }
        else if (role.Contains("Tenant"))
        {
            buildings = from b in _context.Buildings
                        join a in _context.Apartments on b.BuildingId equals a.BuildingId
                        select b;
            tenants = new List<ApplicationUser> { user };
        }
        else
        {
            return Unauthorized();
        }

        ViewData["BuildingId"] = new SelectList(await buildings.Distinct().ToListAsync(), "BuildingId", "Address");
        ViewData["TenantId"] = new SelectList(tenants, "Id", "UserName");
        if (buildings.Any())
        {
            var apartments = _context.Apartments.Where(a => a.BuildingId == buildings.First().BuildingId);
            ViewData["ApartmentId"] = new SelectList(await apartments.ToListAsync(), "ApartmentId", "ApartmentNumber");
        }
        return View(visit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("VisitId,VisitDate,Slot,TenantId,BuildingId,ApartmentId")] Visit visit)
    {
        if (id != visit.VisitId)
        {
            return NotFound();
        }
        var existingVisit = await _context.Visits.FindAsync(id);
        if (existingVisit == null)
        {
            return NotFound();
        }
        existingVisit.VisitDate = visit.VisitDate;
        existingVisit.Slot = visit.Slot;
        existingVisit.TenantId = visit.TenantId;
        existingVisit.BuildingId = visit.BuildingId;
        existingVisit.ApartmentId = visit.ApartmentId;
        try
        {
            _context.Update(existingVisit);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VisitExists(visit.VisitId))
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
    public JsonResult GetApartments(int buildingId)
    {
        var apartments = from a in _context.Apartments
                         join b in _context.Buildings on a.BuildingId equals b.BuildingId
                         where b.BuildingId == buildingId
                         select new { value = a.ApartmentId, text = a.ApartmentNumber };

        return Json(apartments);
    }
    private bool VisitExists(int id)
    {
        return _context.Visits.Any(e => e.VisitId == id);
    }



}