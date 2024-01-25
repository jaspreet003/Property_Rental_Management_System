using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Property_Management_System.Data;
using Property_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

[Authorize]
public class MessageController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MessageController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Message
    public async Task<IActionResult> Index()
    {
        var messages = await _context.Messages.ToListAsync();
        return View(messages);
    }

    // GET: Message/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var message = await _context.Messages.FirstOrDefaultAsync(m => m.MessageId == id);

        if (message == null)
        {
            return NotFound();
        }

        return View(message);
    }

    // GET: Message/Create
    public async Task<IActionResult> Create()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            var users = await _userManager.Users.ToListAsync();
            var userSelectList = new List<SelectListItem>();

            foreach (var user in users)
            {
                userSelectList.Add(new SelectListItem { Value = user.Id, Text = user.UserName });
            }

            var messageViewModel = new MessageViewModel
            {
                UserSelectList = userSelectList
            };

            return View(messageViewModel);
        }
        else
        {
            RedirectToPage("/Account/Login");
            return View();
        }

    }

    // POST: Message/Create
    [HttpPost]
    public async Task<IActionResult> Create(Message message)
    {
        if (ModelState.IsValid)
        {
            _context.Add(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(message);
    }

    // GET: Message/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var message = await _context.Messages.FindAsync(id);

        if (message == null)
        {
            return NotFound();
        }

        return View(message);
    }

    // POST: Message/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MessageId,Content,SenderId,ReceiverId")] Message message)
    {
        if (id != message.MessageId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(message.MessageId))
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
        return View(message);
    }

    // GET: Message/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var message = await _context.Messages.FirstOrDefaultAsync(m => m.MessageId == id);

        if (message == null)
        {
            return NotFound();
        }

        return View(message);
    }

    // POST: Message/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        _context.Remove(message);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MessageExists(int id)
    {
        return _context.Messages.Any(e => e.MessageId == id);
    }

    [HttpGet]
    public async Task<IActionResult> SentMessages()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var messages = await _context.Messages
            .Where(um => um.SenderId == userId)
            .ToListAsync();
        return View(messages);
    }

    // GET: Message/Received
    [HttpGet]
    public async Task<IActionResult> ReceivedMessages()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var messages = await _context.Messages
            .Where(um => um.ReceiverId == userId)
            .ToListAsync();
        return View(messages);
    }

    // GET: Message/SendMessage
    [HttpGet]
    public IActionResult SendMessage()
    {
        ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "UserName");
        return View();
    }

    // POST: Message/SendMessage
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(Message message)
    {
        if (ModelState.IsValid)
        {
            _context.Add(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SentMessages));
        }
        return View(message);
    }

}
