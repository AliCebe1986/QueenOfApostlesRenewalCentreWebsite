using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.IO.Image;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Exceptions;
using QueenOfApostlesRenewalCentre.Data;
using Microsoft.EntityFrameworkCore;
using QueenOfApostlesRenewalCentre.Models;
using System.Reflection.Metadata.Ecma335;

public class ContractController : Controller {
    private readonly ApplicationDbContext _context;

    public ContractController(ApplicationDbContext context) {
        _context = context;
    }

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(int id) {

        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);

        if ( booking == null) {
            return NotFound();
        }

        string templateUrl = "";

        var groupTypes = new HashSet<string> { "Group", "Clergy", "Oblate", "HighSchool", "DayGroup" };
        if (groupTypes.Contains(booking.ReservationType)) {
            templateUrl = booking.EndDate == DateTime.MinValue ? "~/pdf/Groups_One_Day_No_Night.pdf" : "~/pdf/Groups.pdf";
        }

        var individualTypes = new HashSet<string> { "Individual", "Student" };
        if (individualTypes.Contains(booking.ReservationType)) {
            templateUrl = booking.EndDate == DateTime.MinValue ? "~/pdf/Individuals_Just_Day.pdf" : "~/pdf/Individuals_Overnight.pdf";
        }

        var contractMap = new Dictionary<string, string>
    {
        { "EngagedEncounter", "~/pdf/Engaged_Encounter.pdf" },
        { "MarriedCouple", "~/pdf/MarriedCouples.pdf" },
        { "Sister", "~/pdf/Nuns_7_Days.pdf" }
    };

        if (contractMap.ContainsKey(booking.ReservationType)) {
            templateUrl = contractMap[booking.ReservationType];
        }


        var model = new ContractViewModel {

            booking = booking,
            ContractURL = templateUrl
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> SaveFilledPdf([FromBody] Dictionary<string, string> formData) {


        if (!formData.TryGetValue("bookingId", out string bookingIdStr) || !int.TryParse(bookingIdStr, out int bookingId)) {
            return Json(new { success = false, message = "Invalid Booking ID." });
        }

        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) {
            return Json(new { success = false, message = "Booking not found." });
        }

        string PdfTemplatePath = "";
        var groupTypes = new HashSet<string> { "Group", "Clergy", "Oblate", "HighSchool", "DayGroup" };
        if (groupTypes.Contains(booking.ReservationType)) {
            PdfTemplatePath = booking.EndDate == DateTime.MinValue ? "~/pdf/Groups_One_Day_No_Night.pdf" : "~/pdf/Groups.pdf";
        }

        var individualTypes = new HashSet<string> { "Individual", "Student" };
        if (individualTypes.Contains(booking.ReservationType)) {
            PdfTemplatePath = booking.EndDate == DateTime.MinValue ? "~/pdf/Individuals_Just_Day.pdf" : "~/pdf/Individuals_Overnight.pdf";
        }

        var contractMap = new Dictionary<string, string>
    {
        { "EngagedEncounter", "~/pdf/Engaged_Encounter.pdf" },
        { "MarriedCouple", "~/pdf/MarriedCouples.pdf" },
        { "Sister", "~/pdf/Nuns_7_Days.pdf" }
    };

        if (contractMap.ContainsKey(booking.ReservationType)) {
            PdfTemplatePath = contractMap[booking.ReservationType];
        }

        try {

            PdfTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                PdfTemplatePath.TrimStart('~', '/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            byte[] generatedPdf =  GeneratePdfBytes(formData, PdfTemplatePath);

            var contract = new Contract {

                BookingId = bookingId,
                ContractPdf = generatedPdf,
                Status = "Pending",
                ContractName = $"Contract_Booking_{booking.BookingId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf"

            };
            
        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();

            return Json(new { success = true, bookingId = booking.BookingId });
            
        } catch (Exception ex) {
            return Json(new { success = false, message = ex.Message });
        }
    }

    public byte[] GeneratePdfBytes(Dictionary<string, string> formData, string inputPdfPath) {
        using (var memoryStream = new MemoryStream())
        using (var pdfReader = new PdfReader(inputPdfPath))
        using (var pdfWriter = new PdfWriter(memoryStream))
        using (var pdfDocument = new PdfDocument(pdfReader, pdfWriter)) {
            var form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            foreach (var field in formData) {
                var pdfField = form.GetField(field.Key);
                if (pdfField != null) {
                    pdfField.SetValue(field.Value);
                }
            }

            if (formData.ContainsKey("signature") && !string.IsNullOrEmpty(formData["signature"])) {
                byte[] signatureBytes = Convert.FromBase64String(formData["signature"].Split(',')[1]);
                ImageData signatureImage = ImageDataFactory.Create(signatureBytes);
                Image img = new Image(signatureImage).ScaleAbsolute(150, 50);

                int lastPage = pdfDocument.GetNumberOfPages();
                var document = new Document(pdfDocument);
                document.ShowTextAligned(new Paragraph("Signature:"), 100, 100, lastPage,
                    iText.Layout.Properties.TextAlignment.LEFT,
                    iText.Layout.Properties.VerticalAlignment.BOTTOM, 0);

                document.Add(img.SetFixedPosition(lastPage, 170, 90));
            }

            form.FlattenFields();
            pdfDocument.Close();

            return memoryStream.ToArray();  
        }
    }



}
