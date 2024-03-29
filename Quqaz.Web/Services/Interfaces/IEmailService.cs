using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsHtml(string to, string from, string password, string subject, string message);
        Task SendEmailAsHtml(string to, string from, string password, string subject, string message, IFormFile attachment);
    }
}
