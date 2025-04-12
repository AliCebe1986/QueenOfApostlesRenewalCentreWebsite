using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using QueenOfApostlesRenewalCentre.Data;
using QueenOfApostlesRenewalCentre.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Forms;


namespace QueenOfApostlesRenewalCentre.Services {
    public class PdfService {

        private readonly ApplicationDbContext _context;
        private readonly IConverter _pdfConverter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PdfService(ApplicationDbContext context, IConverter pdfConverter, IWebHostEnvironment webHostEnvironment) {

            _context = context;
            _pdfConverter = pdfConverter;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<byte[]> GenerateInvoicePdf(int invoiceId) {
            var invoice = await _context.Invoices
                                .Where(i => i.InvoiceId == invoiceId)
                                .FirstOrDefaultAsync();

            if (invoice == null) {
                throw new Exception("Invoice not found.");
            }

            var booking = await _context.Bookings.FindAsync(invoice.BookingId);

            if (booking == null) {
                throw new Exception("Booking not found for this invoice.");
            }

            var rooms = await _context.Rooms
                                        .Where(r => booking.RoomIds.Contains(r.RoomId))
                                        .ToListAsync();

            string invoiceHtml = $@"
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; }}
                .invoice-box {{ width: 100%; padding: 20px; border: 1px solid #eee; }}
                h2 {{ text-align: center; }}
                table {{ width: 100%; border-collapse: collapse; }}
                table, th, td {{ border: 1px solid black; padding: 8px; text-align: left; }}
            </style>
        </head>
        <body>
            <div class='invoice-box'>
                <h2>Invoice #{invoice.InvoiceId}</h2>
                <p><strong>Guest Name:</strong> {booking.GuestName}</p>
                <p><strong>Email:</strong> {booking.Email}</p>
                <p><strong>Phone:</strong> {booking.PhoneNumber}</p>
                <p><strong>Reservation Type:</strong> {booking.ReservationType}</p>
                <p><strong>Arrival:</strong> {booking.StartDate:yyyy-MM-dd}</p>
                <p><strong>Departure:</strong> {booking.EndDate:yyyy-MM-dd}</p>
                
                <h3>Rooms Booked</h3>
                <ul>
                    {string.Join("", rooms.Select(r => $"<li>{r.Name} (Room #{r.RoomNumber})</li>"))}
                </ul>

                <h3>Price Breakdown</h3>
                <table>
                    <tr><th>Item</th><th>Amount</th></tr>
                    <tr><td>Room Charges</td><td>{invoice.RoomCost:C}</td></tr>
                    <tr><td>Breakfast</td><td>{invoice.BreakfastCost:C}</td></tr>
                    <tr><td>Lunch</td><td>{invoice.LunchCost:C}</td></tr>
                    <tr><td>Dinner</td><td>{invoice.DinnerCost:C}</td></tr>
                    <tr><td>Premises Use</td><td>{invoice.PremisesUseCost:C}</td></tr>
                    <tr><td>Discount</td><td>{invoice.DirectorsDiscount:C}</td></tr>
                    <tr><td><strong>Total</strong></td><td><strong>{invoice.TotalAmount:C}</strong></td></tr>
                </table>

                <p><strong>Status:</strong> {invoice.Status}</p>
                <p><strong>Issued Date:</strong> {invoice.IssuedDate:yyyy-MM-dd}</p>
            </div>
        </body>
        </html>";

            var pdfDocument = new HtmlToPdfDocument() {
                GlobalSettings = new GlobalSettings {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { new ObjectSettings { HtmlContent = invoiceHtml } }

            };
            return _pdfConverter.Convert(pdfDocument);
        }



        public Dictionary<string, string> ExtractFormFields(string pdfPath) {


            var formFields = new Dictionary<string, string>();

            using (var pdfReader = new PdfReader(pdfPath))
            using (var pdfDocument = new PdfDocument(pdfReader)) {

                var form = PdfAcroForm.GetAcroForm(pdfDocument, false);
                if (form != null) {
                    foreach (var field in form.GetAllFormFields()) {
                        formFields.Add(field.Key, field.Value.GetValueAsString());
                    }
                }


            }
            return formFields;

        }



    }
}

