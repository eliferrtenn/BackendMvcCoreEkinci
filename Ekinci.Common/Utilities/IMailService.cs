using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ekinci.Common.Utilities
{
    public interface IMailService
    {
        Task<EmailResult> Send(string to, string subject, string filePath, Dictionary<string, string> parameterList = null, AttachmentCollection attachments = null);
        Task<EmailResult> Send(List<string> toList, string subject, string filePath, Dictionary<string, string> parameterList = null, AttachmentCollection attachments = null);
    }
}
