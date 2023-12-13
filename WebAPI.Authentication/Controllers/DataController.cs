using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Authentication.Data;

namespace WebAPI.Authentication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly UserManager<Employee> _userManager;

    public DataController(UserManager<Employee> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetSecuredData()
    {
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //var user = await _userManager.FindByIdAsync(userId);

        var user = await _userManager.GetUserAsync(User);

        return Ok(new string[] { "Ahmed",
            "Muhammed",
            "Mona",
            user!.Department,
            user!.Email!
        });
    }

    [HttpGet]
    [Authorize(Policy = "Managers")]
    [Route("ForManagers")]
    public ActionResult GetSecuredDataForManagers()
    {
        return Ok(new string[] {
            "Ahmed",
            "Muhammed",
            "Mona",
            "This Data From Managers Only"
        });
    }

    [HttpGet]
    [Authorize(Policy = "Employees")]
    [Route("ForEmployees")]
    public ActionResult GetSecuredDataForEmployees()
    {
        return Ok(new string[] {
            "Ahmed",
            "Muhammed",
            "Mona",
            "This Data From Employees"
        });
    }
}
