namespace QueenOfApostlesRenewalCentre.Models
{
    public class UserDashboardViewModel
    {
        public List<Booking> UpcomingReservations { get; set; }
        public List<Booking> PastReservations { get; set; }

        public List<Booking> CurrentReservations { get; set; }
    }
}
