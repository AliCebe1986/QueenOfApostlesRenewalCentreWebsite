using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;

namespace QueenOfApostlesRenewalCentre.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ConferenceRoomController : Controller {

        private readonly ApplicationDbContext _context;

        public ConferenceRoomController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var conferenceRooms = await _context.ConferenceRooms.ToListAsync();
            return View(conferenceRooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConferenceRoom conferenceRoom) {
            if (ModelState.IsValid) {

                _context.ConferenceRooms.Add(conferenceRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(conferenceRoom);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConferenceRoom conferenceRoom) {
            if (id != conferenceRoom.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(conferenceRoom);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!conferenceRoomExists(conferenceRoom.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(conferenceRoom);
        }


        private bool conferenceRoomExists(int id) {
            return _context.ConferenceRooms.Any(e => e.Id == id);
        }

    }
}
