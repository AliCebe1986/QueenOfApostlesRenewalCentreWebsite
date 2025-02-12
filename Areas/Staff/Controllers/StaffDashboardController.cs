using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QueenOfApostlesRenewalCentre.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff,Admin")]
    public class StaffDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Staff/StaffDashboard/Index
        public async Task<IActionResult> Index()
        {
            // Get the logged-in staff user
            var user = await _userManager.GetUserAsync(User);
            string fullName = $"{user.Name} {user.Surname}";

            // Retrieve cleaning tasks assigned to this staff (assuming CleanerName is set to full name)
            var tasks = await _context.RoomCleanings
                .Where(r => r.CleanerName == fullName)
                .ToListAsync();

            // Summary information
            ViewBag.TotalTasks = tasks.Count;
            ViewBag.CompletedTasks = tasks.Count(t => t.CleaningStatus == "Completed");
            ViewBag.PendingTasks = tasks.Count(t => t.CleaningStatus != "Completed");

            return View(tasks);
        }

        // GET: Staff/StaffDashboard/CleaningTasks
        public async Task<IActionResult> CleaningTasks()
        {
            var user = await _userManager.GetUserAsync(User);
            // Assuming tasks are filtered by the cleaner's full name; adjust as needed.
            string fullName = $"{user.Name} {user.Surname}";
            var tasks = await _context.RoomCleanings
                                      .Where(r => r.CleanerName == fullName)
                                      .ToListAsync();
            return View(tasks);
        }

        // GET: Staff/StaffDashboard/Profile
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        // GET: Staff/StaffDashboard/Reports
        public async Task<IActionResult> Reports()
        {
            var user = await _userManager.GetUserAsync(User);
            string fullName = $"{user.Name} {user.Surname}";
            var tasks = await _context.RoomCleanings
                .Where(r => r.CleanerName == fullName)
                .ToListAsync();

            ViewBag.TotalTasks = tasks.Count;
            ViewBag.CompletedTasks = tasks.Count(t => t.CleaningStatus == "Completed");
            ViewBag.PendingTasks = tasks.Count(t => t.CleaningStatus != "Completed");

            return View(tasks);
        }
    }
}
