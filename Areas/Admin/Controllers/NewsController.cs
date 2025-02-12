using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QueenOfApostlesRenewalCentre.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/News
        public async Task<IActionResult> Index()
        {
            // Order news by SortOrder (ascending). You can also order by PublishedDate if desired.
            var newsList = await _context.News.OrderBy(n => n.SortOrder).ToListAsync();
            return View(newsList);
        }

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news)
        {
            if (ModelState.IsValid)
            {
                news.PublishedDate = DateTime.Now;
                // If no SortOrder is provided, assign a default value (max existing order + 1)
                if (news.SortOrder == 0)
                {
                    int maxOrder = _context.News.Any() ? _context.News.Max(n => n.SortOrder) : 0;
                    news.SortOrder = maxOrder + 1;
                }
                _context.News.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var news = await _context.News.FindAsync(id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        // POST: Admin/News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News news)
        {
            if (id != news.NewsId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.News.Any(n => n.NewsId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: Admin/News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var news = await _context.News.FirstOrDefaultAsync(n => n.NewsId == id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/News/Reorder
        public async Task<IActionResult> Reorder()
        {
            var newsList = await _context.News.OrderBy(n => n.SortOrder).ToListAsync();
            return View(newsList);
        }

        // POST: Admin/News/Reorder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reorder(int[] sortedIds)
        {
            // sortedIds is an array of news IDs in the new order.
            if (sortedIds != null)
            {
                for (int i = 0; i < sortedIds.Length; i++)
                {
                    var newsItem = await _context.News.FindAsync(sortedIds[i]);
                    if (newsItem != null)
                    {
                        newsItem.SortOrder = i + 1;
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
