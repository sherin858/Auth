using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebAPI.Authentication.Data;

var builder = WebApplication.CreateBuilder(args);

#region Default

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region Database

var connectionString = builder.Configuration.GetConnectionString("Company");
builder.Services.AddDbContext<CompanyContext>(options =>
    options.UseSqlServer(connectionString));

#endregion

//Must be before Authentication part
#region Identity Manager

builder.Services.AddIdentity<Employee, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;

    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<CompanyContext>();

#endregion

#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cool";
    options.DefaultChallengeScheme = "Cool";
})
    .AddJwtBearer("Cool", options =>
    {
        string keyString = builder.Configuration.GetValue<string>("SecretKey") ?? string.Empty;
        var keyInBytes = Encoding.ASCII.GetBytes(keyString);
        var key = new SymmetricSecurityKey(keyInBytes);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

#endregion

#region Authorization

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Managers", policy => policy
        .RequireClaim(ClaimTypes.Role, "Manager", "CEO")
        .RequireClaim(ClaimTypes.NameIdentifier));
    
    options.AddPolicy("Employees", policy => policy
        .RequireClaim(ClaimTypes.Role, "Employee", "Manager", "CEO")
        .RequireClaim("Nationality", "Egyptian")
        .RequireClaim(ClaimTypes.NameIdentifier));
});

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();