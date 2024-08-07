
using EduHome.Data;
using EduHome.Models;
using EduHome.Services.Interfaces;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
[Area("AdminArea")]
public class SellingController : Controller
{

    private readonly EduCompaniesDbContext _dbContext;
    private readonly IEmailService _emailService;

    public SellingController(EduCompaniesDbContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index(int stage = 1)
    {
        var query = _dbContext.BuyingRequests
            .Include(br => br.Course)
            .Include(br => br.User)
            .Select(br => new BuyingRequest
            {
                Id = br.Id,
                CourseName = br.Course.Name,
                UserName = br.User.UserName,
                Desc = br.Course.Desc,
                RequestDate = br.RequestDate,
                Status = br.Status,
                Email = br.User.Email
            });

        return View(await PaginationVM<BuyingRequest>.CreateVM(query, stage, 2));
    }

    [HttpPost]
    public async Task<IActionResult> Accept(int id)
    {
        var request = await _dbContext.BuyingRequests
            .Include(br => br.User) 
            .FirstOrDefaultAsync(br => br.Id == id);

        if (request == null) return NotFound();

        request.Status = "Accepted";
        _dbContext.Update(request);
        await _dbContext.SaveChangesAsync();

        _emailService.SendEmail(new List<string> { request.User.Email },
            "Your request has been accepted by the admin.",
            "Request Accepted",
            "Your request has been accepted.");

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id)
    {
        var request = await _dbContext.BuyingRequests
            .Include(br => br.User) 
            .FirstOrDefaultAsync(br => br.Id == id);

        if (request == null) return NotFound();

        request.Status = "Rejected";
        _dbContext.Update(request);
        await _dbContext.SaveChangesAsync();

        _emailService.SendEmail(new List<string> { request.User.Email },
            "Your request has been rejected by the admin.",
            "Request Rejected",
            "Your request has been rejected.");

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Reply(int id, string replyMessage)
    {

        var request = await _dbContext.BuyingRequests
            .Include(br => br.User) 
            .FirstOrDefaultAsync(br => br.Id == id);

        if (request == null) return NotFound();

        if (request.Status != "Accepted")
        {
            TempData["Message"] = "You can only reply to accepted requests.";
            TempData["MessageType"] = "error";
            return RedirectToAction("Index");
        }

        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/templates/requestTemplate/requestStatus.html");
        var template = System.IO.File.ReadAllText(templatePath);

        var body = template
           .Replace("{{username}}", request.User.UserName)
           .Replace("{{status}}", "Replied")
           .Replace("{{response}}", replyMessage);

        var emails = new List<string> { request.User.Email }; 
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
        var request = await _dbContext.BuyingRequests.FirstOrDefaultAsync(s => s.Id == id);
        if (request == null) return NotFound();

        _dbContext.BuyingRequests.Remove(request);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}

