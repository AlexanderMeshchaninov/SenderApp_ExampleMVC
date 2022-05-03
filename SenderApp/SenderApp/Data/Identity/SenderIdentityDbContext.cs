using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SenderApp.Data.Identity
{
    public sealed class SenderIdentityDbContext : IdentityDbContext<CustomIdentityUser>
    {
        public SenderIdentityDbContext(DbContextOptions<SenderIdentityDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}