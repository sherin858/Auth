using Microsoft.AspNetCore.Identity;

namespace WebAPI.Authentication.Data;

public class Employee : IdentityUser
{
    public string Department { get; set; } = string.Empty;
}
