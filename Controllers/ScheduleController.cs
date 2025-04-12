using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;

namespace QueenOfApostlesRenewalCentre.Controllers {




    [Route("Schedule")]
    public class ScheduleController : Controller {

        private readonly ApplicationDbContext _context;


        public ScheduleController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Index() {
            return View();
        }


        [HttpGet("GetRoomSchedule")]
        public async Task<IActionResult> GetRoomSchedule(int roomId, DateTime weekStartDate) {
            var schedule = new List<object>();
            DateTime currentDay = weekStartDate.Date;

            for (int i = 0; i < 7; i++) {
                bool isBooked = await _context.Bookings.AnyAsync(b =>
                    b.RoomIds.Contains(roomId) &&
                    b.Status != "Cancelled" &&
                    b.StartDate.Date <= currentDay &&
                    b.EndDate.Date >= currentDay);

                schedule.Add(new {
                    Date = currentDay.ToString("yyyy-MM-dd"),
                    IsBooked = isBooked
                });

                currentDay = currentDay.AddDays(1);
            }

            return Ok(schedule);
        }




    }
}
