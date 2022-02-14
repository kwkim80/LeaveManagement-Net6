using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagement.Web.Data.Migrations
{
    public partial class AddedDefaultUserAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cac43a7e-f7bb-4458-babf-1add431ccbbf", "45e66075-bbca-4675-87cc-f27e91c8d192", "Administrator", "ADMINISTRATOR" },
                    { "cac43a8e-f7cb-4148-baaf-1acb431ccbbf", "c39bc23e-b4de-4469-be21-2c87e946f84b", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateJoined", "DateOfBirth", "Email", "EmailConfirmed", "Firstname", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TaxId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1d48f016-fb16-4255-b83a-477f112a9d42", 0, "98fb5ebc-0b41-4147-b58d-ab9d9e75a1c0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@gmail.com", true, "System", "User", false, null, "USER@GMAIL.COM", "USER@GMAIL.COM", "AQAAAAEAACcQAAAAEKZe7VFXy0S9lmR+DWbANE+Ke59iS2+po4IXNvKelNiUI9B1wnQY0YUM2Rci1iAq5Q==", null, false, "d63df94a-d235-4e26-aa93-631daaccb691", null, false, "user@gmail.com" },
                    { "ce096625-302c-4e93-ab09-9971a45301cc", 0, "d2624d0d-c68f-43a9-88fa-890901578ba9", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "System", "Admin", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEOH+uoZCY6x376H31ubEXbxqG7pexyrsT+wsWEXPCQObjaZX3rVG6CLDvg+KnpkGRw==", null, false, "e7a72db4-c606-45da-b7cd-c9095332bc39", null, false, "admin@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "cac43a8e-f7cb-4148-baaf-1acb431ccbbf", "1d48f016-fb16-4255-b83a-477f112a9d42" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "cac43a7e-f7bb-4458-babf-1add431ccbbf", "ce096625-302c-4e93-ab09-9971a45301cc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cac43a8e-f7cb-4148-baaf-1acb431ccbbf", "1d48f016-fb16-4255-b83a-477f112a9d42" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cac43a7e-f7bb-4458-babf-1add431ccbbf", "ce096625-302c-4e93-ab09-9971a45301cc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cac43a7e-f7bb-4458-babf-1add431ccbbf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cac43a8e-f7cb-4148-baaf-1acb431ccbbf");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1d48f016-fb16-4255-b83a-477f112a9d42");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ce096625-302c-4e93-ab09-9971a45301cc");
        }
    }
}
