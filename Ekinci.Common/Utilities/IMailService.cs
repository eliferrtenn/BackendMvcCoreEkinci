using System.Net.Mail;

namespace Ekinci.Common.Utilities
{
    public interface IMailService
    {
        Task<EmailResult> Send(string to, string subject, string filePath, Dictionary<string, string> parameterList = null, AttachmentCollection attachments = null);
        Task<EmailResult> Send(List<string> toList, string subject, string filePath, Dictionary<string, string> parameterList = null, AttachmentCollection attachments = null);
    }
}