using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Models;

namespace QueenOfApostlesRenewalCentre.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffTask> StaffTasks { get; set; }
        public DbSet<WeeklySchedule> WeeklySchedules { get; set; }
        public DbSet<RoomCleaning> RoomCleanings { get; set; }
        public DbSet<News> News { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<ConferenceRoom> ConferenceRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            // Seed rooms
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, RoomNumber = "1", Name = "Room 1", Capacity = 2, Type = "Standard", WithShower = true  },
                new Room { RoomId = 2, RoomNumber = "2", Name = "Room 2", Capacity = 2, Type = "Standard", WithShower = true  },
                new Room { RoomId = 3, RoomNumber = "3", Name = "Room 3", Capacity = 2, Type = "Standard", WithShower = false  },
                new Room { RoomId = 4, RoomNumber = "4", Name = "Room 4", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 5, RoomNumber = "5", Name = "Room 5", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 6, RoomNumber = "6", Name = "Room 6", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 7, RoomNumber = "7", Name = "Room 7", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 8, RoomNumber = "8", Name = "Room 8", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 9, RoomNumber = "9", Name = "Room 9", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 10, RoomNumber = "10", Name = "Room 10", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 11, RoomNumber = "11", Name = "Room 11", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 12, RoomNumber = "12", Name = "Room 12", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 13, RoomNumber = "13", Name = "Room 13", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 14, RoomNumber = "14", Name = "Room 14", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 15, RoomNumber = "15", Name = "Room 15", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 16, RoomNumber = "16", Name = "Room 16", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 17, RoomNumber = "17", Name = "Room 17", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 18, RoomNumber = "18", Name = "Room 18", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 19, RoomNumber = "19", Name = "Room 19", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 20, RoomNumber = "20", Name = "Room 20", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 21, RoomNumber = "21", Name = "Room 21", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 22, RoomNumber = "22", Name = "Room 22", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 23, RoomNumber = "23", Name = "Room 23", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 24, RoomNumber = "24", Name = "Room 24", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 25, RoomNumber = "25", Name = "Room 25", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 26, RoomNumber = "26", Name = "Room 26", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 27, RoomNumber = "27", Name = "Room 27", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 28, RoomNumber = "28", Name = "Room 28", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 29, RoomNumber = "29", Name = "Room 29", Capacity = 2, Type = "Standard", WithShower = false },
                new Room { RoomId = 30, RoomNumber = "30", Name = "Room 30", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 31, RoomNumber = "31", Name = "Room 31", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 32, RoomNumber = "32", Name = "Room 32", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 33, RoomNumber = "33", Name = "Room 33", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 34, RoomNumber = "34", Name = "Room 34", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 35, RoomNumber = "35", Name = "Room 35", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 36, RoomNumber = "36", Name = "Room 36", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 37, RoomNumber = "37", Name = "Room 37", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 38, RoomNumber = "38", Name = "Room 38", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 39, RoomNumber = "39", Name = "Room 39", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 40, RoomNumber = "40", Name = "Room 40", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 41, RoomNumber = "41", Name = "Room 41", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 42, RoomNumber = "42", Name = "Room 42", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 43, RoomNumber = "43", Name = "Room 43", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 44, RoomNumber = "44", Name = "Room 44", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 45, RoomNumber = "45", Name = "Room 45", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 46, RoomNumber = "46", Name = "Room 46", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 47, RoomNumber = "47", Name = "Room 47", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 48, RoomNumber = "48", Name = "Room 48", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 49, RoomNumber = "49", Name = "Room 49", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 50, RoomNumber = "50", Name = "Room 50", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 51, RoomNumber = "51", Name = "Room 51", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 52, RoomNumber = "52", Name = "Room 52", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 53, RoomNumber = "53", Name = "Room 53", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 54, RoomNumber = "54", Name = "Room 54", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 55, RoomNumber = "55", Name = "Room 55", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 56, RoomNumber = "56", Name = "Room 56", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 57, RoomNumber = "57", Name = "Room 57", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 58, RoomNumber = "58", Name = "Room 58", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 59, RoomNumber = "59", Name = "Room 59", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 60, RoomNumber = "60", Name = "Room 60", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 61, RoomNumber = "61", Name = "Room 61", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 62, RoomNumber = "62", Name = "Room 62", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 63, RoomNumber = "63", Name = "Room 63", Capacity = 2, Type = "Standard", WithShower = true },
                new Room { RoomId = 64, RoomNumber = "64", Name = "Room 64", Capacity = 2, Type = "Standard", WithShower = true }
            );
        }
    }
}