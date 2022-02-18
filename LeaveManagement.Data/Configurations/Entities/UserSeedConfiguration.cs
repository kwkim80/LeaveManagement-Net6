
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Data.Configurations.Entities
{
    public class UserSeedConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            var hasher = new PasswordHasher<Employee>();
            builder.HasData(
                 new Employee
                 {
                     Id = "ce096625-302c-4e93-ab09-9971a45301cc",
                     Email = "admin@gmail.com",
                     NormalizedEmail = "ADMIN@GMAIL.COM",
                     NormalizedUserName = "ADMIN@GMAIL.COM",
                     UserName = "admin@gmail.com",
                     Firstname = "System",
                     Lastname = "Admin",
                     PasswordHash = hasher.HashPassword(null, "Q1w2e3r4!@"),
                     EmailConfirmed = true
                 },
                 new Employee
                 {
                     Id = "1d48f016-fb16-4255-b83a-477f112a9d42",
                     Email = "user@gmail.com",
                     NormalizedEmail = "USER@GMAIL.COM",
                     NormalizedUserName = "USER@GMAIL.COM",
                     UserName = "user@gmail.com",
                     Firstname = "System",
                     Lastname = "User",
                     PasswordHash = hasher.HashPassword(null, "Q1w2e3r4!@"),
                     EmailConfirmed = true
                 }
            );
        }
    }
}