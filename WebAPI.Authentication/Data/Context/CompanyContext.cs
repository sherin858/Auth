using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Authentication.Data;

public class CompanyContext : IdentityDbContext<Employee>
{
    public CompanyContext(DbContextOptions<CompanyContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Employee>().ToTable("Employees");
        builder.Entity<IdentityUserClaim<string>>().ToTable("EmployesClaims");
    }
}
