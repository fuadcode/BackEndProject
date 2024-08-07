using EduHome.Data;
using EduHome.Models;
using EduHome.Services.Interfaces;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Area("AdminArea")]
public class MessageController : Controller
{
    private readonly EduCompaniesDbContext _context;
    private readonly IEmailService _emailService;

    public MessageController(EduCompaniesDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

  
    public async Task<IActionResult> Index(int stage = 1)
    {
        
        var query = _context.MessageForms
            .Select(m => new MessageForm
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Subject = m.Subject,
                Message = m.Message,
                CreatedForm = m.CreatedForm,
                Status = m.Status
            });

        var pagedData = await PaginationVM<MessageForm>.CreateVM(query, stage, 2);

        return View(pagedData);
    }


    [HttpPost]
    public async Task<IActionResult> Accept(int id)
    {
        var message = await _context.MessageForms.FindAsync(id);
        if (message == null) return NotFound();

        message.Status = "Accepted";
        _context.Update(message);
        await _context.SaveChangesAsync();

        
        _emailService.SendEmail(new List<string> { message.Email },
            "Your message has been accepted by the admin.",
            "Message Accepted",
            "Your message has been accepted.");

            return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id)
    {
        var message = await _context.MessageForms.FindAsync(id);
        if (message == null) return NotFound();

        message.Status = "Rejected";
        _context.Update(message);
        await _context.SaveChangesAsync();

        _emailService.SendEmail(new List<string> { message.Email },
            "Your message has been rejected by the admin.",
            "Message Rejected",
            "Your message has been rejected.");

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Reply(int id, string replyMessage)
    {
        var message = await _context.MessageForms.FindAsync(id);
        if (message == null) return NotFound();

        if (message.Status != "Accepted")
        {
            TempData["Message"] = "You can only reply to accepted messages.";
            TempData["MessageType"] = "error";
            return RedirectToAction("Index");
        }

        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/templates/messageTemplate/messageStatus.html");
        var template = System.IO.File.ReadAllText(templatePath);

     
        var body = template
            .Replace("{{username}}", message.Name)
            .Replace("{{status}}", "Replied")
            .Replace("{{userMessage}}", message.Message)
            .Replace("{{response}}", replyMessage);

        var emails = new List<string> { message.Email };
        var title = "Message Reply";
        var subject = "Your message has been replied to";

        _emailService.SendEmail(emails, body, title, subject);

        TempData["Message"] = "Reply sent successfully.";
        TempData["MessageType"] = "success";
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return BadRequest();
        var message = await _context.MessageForms.FirstOrDefaultAsync(s => s.Id == id);
        if (message == null) return NotFound();

        _context.MessageForms.Remove(message);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}




