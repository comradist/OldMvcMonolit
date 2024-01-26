using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using Northwind.Mvc.Service;

namespace Northwind.Mvc.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MailController : Controller
{
    private readonly IMailService mailService;
    public MailController(IMailService mailService)
    {
        this.mailService = mailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMail([FromForm]MailRequest request)
    {
        try
        {
            await mailService.SendEmailAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            throw;
        }
    }
}