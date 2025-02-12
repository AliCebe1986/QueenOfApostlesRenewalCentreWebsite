using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QueenOfApostlesRenewalCentre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            // Retrieve the latest 6 news items ordered by SortOrder (or PublishedDate)
            var newsItems = await _context.News
                .OrderByDescending(n => n.SortOrder) // or use .OrderByDescending(n => n.PublishedDate)
                .Take(6)
                .ToListAsync();

            // Pass the news items to the view (the view should be strongly typed to IEnumerable<News>)
            return View(newsItems);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
