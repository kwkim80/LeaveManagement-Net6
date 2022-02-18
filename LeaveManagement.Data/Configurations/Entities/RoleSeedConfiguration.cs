using LeaveManagement.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace LeaveManagement.Data.Configurations.Entities
{ 
    public class RoleSeedConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                 new IdentityRole
                 {
                     Id = "cac43a7e-f7bb-4458-babf-1add431ccbbf",

                     Name = Roles.Administrator,
                     NormalizedName = Roles.Administrator.ToUpper()
                 },
                 new IdentityRole
                 {
                     Id = "cac43a8e-f7cb-4148-baaf-1acb431ccbbf",
                     Name = Roles.User,
                     NormalizedName = Roles.Administrator.ToUpper()
                 }
             );
        }
    }
}