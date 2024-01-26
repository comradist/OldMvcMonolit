using Northwind.Mvc.Models;

namespace Northwind.Mvc.Service;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest); 
}