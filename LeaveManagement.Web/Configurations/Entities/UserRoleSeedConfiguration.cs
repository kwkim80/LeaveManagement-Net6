using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Web.Configurations.Entities
{
    public class UserRoleSeedConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
               new IdentityUserRole<string>
               {
                   RoleId = "cac43a7e-f7bb-4458-babf-1add431ccbbf",
                   UserId = "ce096625-302c-4e93-ab09-9971a45301cc"
               },
               new IdentityUserRole<string>
               {
                   RoleId = "cac43a8e-f7cb-4148-baaf-1acb431ccbbf",
                   UserId = "1d48f016-fb16-4255-b83a-477f112a9d42"
               }
            );
        }

     
    }
}