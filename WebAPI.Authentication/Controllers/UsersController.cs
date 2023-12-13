using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Authentication.Data;
using WebAPI.Authentication.Dtos;

namespace WebAPI.Authentication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<Employee> _userManager;

    public UsersController(IConfiguration configuration,
        UserManager<Employee> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    #region Static Login

    [HttpPost]
    [Route("Static-Login")]
    public ActionResult<TokenDto> StaticLogin(LoginDto credentials)
    {
        if (credentials.UserName != "admin" || credentials.Password != "password")
        {
            return BadRequest();
        }

        var claimsList = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1565478"),
            new Claim(ClaimTypes.Name, credentials.UserName),
            new Claim(ClaimTypes.Role, "Employeee"),
            new Claim("Nationality", "Egyptian"),
        };

        string keyString = _configuration.GetValue<string>("SecretKey") ?? string.Empty;
        var keyInBytes = Encoding.ASCII.GetBytes(keyString);
        var key = new SymmetricSecurityKey(keyInBytes);

        //Combination of secret Key and HashingAlgorithm
        var signingCredentials = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha256Signature);

        //Putting All together
        var expiry = DateTime.Now.AddMinutes(15);

        var jwt = new JwtSecurityToken(
                expires: expiry,
                claims: claimsList,
                signingCredentials: signingCredentials);

        //Getting Token String
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(jwt);

        return new TokenDto { Token = tokenString };
    }

    #endregion

    #region Login

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto credentials)
    {
        Employee? user = await _userManager.FindByNameAsync(credentials.UserName);
        if (user == null)
        {
            // you can send a message
            return BadRequest();
        }

        bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, credentials.Password);
        if (!isPasswordCorrect)
        {
            // you can send a message
            return BadRequest();
        }

        var claimsList = await _userManager.GetClaimsAsync(user);

        return GenerateToken(claimsList);
    }

    #endregion

    #region Register

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var newEmployee = new Employee
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Department = registerDto.Department,
        };

        var creationResult = await _userManager.CreateAsync(newEmployee, 
            registerDto.Password);
        if (!creationResult.Succeeded)
        {
            return BadRequest(creationResult.Errors);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, newEmployee.Id),
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("Nationality", "Egyptian")
        };

        await _userManager.AddClaimsAsync(newEmployee, claims);

        return NoContent();
    }

    #endregion

    #region Helpers

    private TokenDto GenerateToken(IList<Claim> claimsList)
    {
        string keyString = _configuration.GetValue<string>("SecretKey") ?? string.Empty;
        var keyInBytes = Encoding.ASCII.GetBytes(keyString);
        var key = new SymmetricSecurityKey(keyInBytes);

        //Combination of secret Key and HashingAlgorithm
        var signingCredentials = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha256Signature);

        //Putting All together
        var expiry = DateTime.Now.AddMinutes(15);

        var jwt = new JwtSecurityToken(
                expires: expiry,
                claims: claimsList,
                signingCredentials: signingCredentials);

        //Getting Token String
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(jwt);

        return new TokenDto
        {
            Token = tokenString,
            Expiry = expiry
        };
    }

    #endregion
}
