namespace QueenOfApostlesRenewalCentre.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; } = Guid.NewGuid().ToString();

        public string Message { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
