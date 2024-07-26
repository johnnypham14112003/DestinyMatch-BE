using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using FPT.DestinyMatch.Service.Interfaces;

namespace FPT.DestinyMatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IEmailService _emailService;

        public FirebaseController(IFirebaseService firebaseService, IEmailService emailService)
        {
            _firebaseService = firebaseService;
            _emailService = emailService;

        }
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification(string fcmToken, string? title, string body)
        {
            await _firebaseService.SendNotification(fcmToken, title, body);
            return Ok("Notification sent successfully: " + fcmToken);
        }
        [HttpGet]
        public async Task<IActionResult> SendMail()
        {
            await _emailService.MatchRequestEmail("HoangTest", new Repository.Models.Member
            {
                Fullname = "Pham Huy Hoang",
                Account = new Repository.Models.Account { Email = "hoangphse172789@fpt.edu.vn"}
                });
            return Ok();
        }
    }
}
