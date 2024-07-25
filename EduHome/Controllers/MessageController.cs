using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using EduHome.Models;
using Microsoft.Data.SqlClient;

public class ContactController : Controller
{
    private readonly string _connectionString;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IConfiguration configuration, ILogger<ContactController> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Submit(MessageUsers model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO ContactMessages (Name, Email, Subject, Message) VALUES (@Name, @Email, @Subject, @Message)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Subject", model.Subject);
                    cmd.Parameters.AddWithValue("@Message", model.Message);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return RedirectToAction("ThankYou");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while submitting the contact form.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult ThankYou()
    {
        return View();
    }
}
