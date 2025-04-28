using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Controllers
{
    public class MailController : Controller
    {
        private readonly MailService _mailService;

        public MailController(MailService mailService)
        {
            _mailService = mailService;
        }
       
        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SendEmail(string recipient, string subject, string body)
        {
            if (string.IsNullOrEmpty(recipient) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
            {
                return Json(new { success = false, message = "All fields are required." });
            }

            bool isSent = await _mailService.SendEmailAsync(recipient, subject, body);
            if (isSent)
            {
                return Json(new { success = true, message = "Email sent successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Email sending failed." });
            }
        }
        
    }
}
