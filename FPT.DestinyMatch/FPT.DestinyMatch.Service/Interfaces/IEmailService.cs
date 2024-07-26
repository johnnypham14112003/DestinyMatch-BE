using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IEmailService
    {
        Task MatchRequestEmail(string SenderName, Member receiverMember);
    }
}
