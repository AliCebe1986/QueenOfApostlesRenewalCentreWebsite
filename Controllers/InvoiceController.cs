using Microsoft.AspNetCore.Mvc;
using QueenOfApostlesRenewalCentre.Services;

namespace QueenOfApostlesRenewalCentre.Controllers {

    [Route("Invoice")]
    public class InvoiceController : Controller {

        private readonly PdfService _pdfService;


     
        public InvoiceController(PdfService pdfService) {
            _pdfService = pdfService;
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> Download(int id) {
            try {
                byte[] pdfBytes = await _pdfService.GenerateInvoicePdf(id);
                return File(pdfBytes, "application/pdf", $"Invoice_{id}.pdf");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
