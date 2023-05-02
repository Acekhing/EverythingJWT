using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EverythingJWT.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d28cf6e4-5ebc-4937-b593-7b5f6d57a4a8", "932a61e8-236b-4345-9a5f-bd9a445f2b7e", "User", "USER" },
                    { "f1a9b119-0f0f-4acb-b7f0-7e0d47373f78", "945430ea-be50-4c31-9931-3d44a39d2580", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "e09f995c-334f-45dc-9c54-d0249bd8ba5f", 0, "f45338f9-6593-4cba-b7fb-fb13de63baf0", "user@penpab.com", false, "System", "User", false, null, "USER@PENPAB.COM", "USER", "AQAAAAEAACcQAAAAECd7TI3xVFK0fyIhh3D0RHZ7IJ1r8aFyTCcJwvDgYLZ32AF2S3KN6Mcq+hBLOd1s3A==", null, false, "295a7a43-e8ae-4c26-bbe1-1d84344f18f4", false, "User" },
                    { "e384573d-d2f7-4547-b9df-036360273883", 0, "26a090bb-b87f-46bf-8ccc-efe595f2fcee", "admin@penpab.com", false, "System", "Admin", false, null, "ADMIN@PENPAB.COM", "ADMIN", "AQAAAAEAACcQAAAAEGzxsEVYLwL/v+qg0OQtBkwHtlx02mxMRUNGsm7gqF+WMdKXs4QGSK96i2h3G+yPIQ==", null, false, "ff7196bc-dae4-42d1-a056-fd5c78b6e3c4", false, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "d28cf6e4-5ebc-4937-b593-7b5f6d57a4a8", "e09f995c-334f-45dc-9c54-d0249bd8ba5f" },
                    { "f1a9b119-0f0f-4acb-b7f0-7e0d47373f78", "e384573d-d2f7-4547-b9df-036360273883" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d28cf6e4-5ebc-4937-b593-7b5f6d57a4a8", "e09f995c-334f-45dc-9c54-d0249bd8ba5f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f1a9b119-0f0f-4acb-b7f0-7e0d47373f78", "e384573d-d2f7-4547-b9df-036360273883" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d28cf6e4-5ebc-4937-b593-7b5f6d57a4a8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1a9b119-0f0f-4acb-b7f0-7e0d47373f78");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e09f995c-334f-45dc-9c54-d0249bd8ba5f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e384573d-d2f7-4547-b9df-036360273883");
        }
    }
}
