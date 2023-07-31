using Memoriesx.Models.Dto;

namespace Memoriesx.Service.EmailSender
{
    public interface IEmailSender
    {
        void SendEmail(MessageDto message);
    }
}
