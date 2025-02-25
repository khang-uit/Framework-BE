﻿using MimeKit;

namespace Memoriesx.Models.Dto
{
    public class MessageDto
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public MessageDto(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("Memoriesx", x)));
            Subject = subject;
            Content = content;
        }
    }
}
