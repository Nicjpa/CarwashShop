using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWashShopAPI.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarWashsShops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdvertisingDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountOfWashingUnits = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningTime = table.Column<int>(type: "int", nullable: false),
                    ClosingTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarWashsShops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Income",
                columns: table => new
                {
                    CarWashShopId = table.Column<int>(type: "int", nullable: false),
                    CarWashShopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calendar = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Income = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarWashShopsOwners",
                columns: table => new
                {
                    CarWashShopId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarWashShopsOwners", x => new { x.OwnerId, x.CarWashShopId });
                    table.ForeignKey(
                        name: "FK_CarWashShopsOwners_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarWashShopsOwners_CarWashsShops_CarWashShopId",
                        column: x => x.CarWashShopId,
                        principalTable: "CarWashsShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnerRemovalRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestStatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OwnerToBeRemovedId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CarWashShopId = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerRemovalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnerRemovalRequests_AspNetUsers_OwnerToBeRemovedId",
                        column: x => x.OwnerToBeRemovedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OwnerRemovalRequests_AspNetUsers_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OwnerRemovalRequests_CarWashsShops_CarWashShopId",
                        column: x => x.CarWashShopId,
                        principalTable: "CarWashsShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopRemovalRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarWashShopId = table.Column<int>(type: "int", nullable: false),
                    RequestStatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopRemovalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopRemovalRequests_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopRemovalRequests_CarWashsShops_CarWashShopId",
                        column: x => x.CarWashShopId,
                        principalTable: "CarWashsShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsumerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarWashShopId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ScheduledDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_CarWashsShops_CarWashShopId",
                        column: x => x.CarWashShopId,
                        principalTable: "CarWashsShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarWashShopsServices",
                columns: table => new
                {
                    CarWashShopId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarWashShopsServices", x => new { x.ServiceId, x.CarWashShopId });
                    table.ForeignKey(
                        name: "FK_CarWashShopsServices_CarWashsShops_CarWashShopId",
                        column: x => x.CarWashShopId,
                        principalTable: "CarWashsShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarWashShopsServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "06638581-8f0c-4119-a637-e4f3b5bbe858", 0, "Redcliff View 78", "293a64f7-25e3-4776-bdac-4afd3cd93efa", "CustomUser", "wadzanai.bote@gmail.com", false, "Wadzanai", "Bote", false, null, "WADZANAI.BOTE@GMAIL.COM", "WADZA", "AQAAAAEAACcQAAAAENPEAVtZ8E5JWdDqUnslhHksnUuQ5k3KoNy7dKEqD3dbQ5n4T1vbotML39D3v9OJkQ==", "+1 582-444-7776", false, "Consumer", "7dd3509a-92f2-4dda-8cfd-210031f8d729", false, "wadza" },
                    { "1741abab-ff61-4598-a947-9c798c4ff866", 0, "Pinewood Heights 70", "e2c3f6fd-a327-4077-ae57-972b00e17945", "CustomUser", "vese.calin@gmail.com", false, "Vese", "Calin", false, null, "VESE.CALIN@GMAIL.COM", "VESEC", "AQAAAAEAACcQAAAAEOsRHVwdk0IkO+HHjwi0a/sG2blaVfHkYraMkyyDG36pxjelgN3Mvx4Yghtu1T4qqg==", "+1 312-251-6119", false, "Owner", "51b9d769-4f19-4cd4-820e-ffee6f43550d", false, "vesec" },
                    { "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", 0, "Whiteroof Valley", "34b2aabd-dd46-4f46-b2ec-cd94163656e7", "CustomUser", "robert.bradford@gmail.com", false, "Robert", "Bradford", false, null, "ROBERT.BRADFORD@GMAIL.COM", "ROBBER", "AQAAAAEAACcQAAAAEKahnP2UCEn+wVqHxcOZL0uQyUaX7hAgXalde8/9OMTBBADLSAv05zq7n97ngTHuYg==", "+1 304-894-4852", false, "Consumer", "6db82fc9-d7a1-497a-96de-c84e02a593b4", false, "robber" },
                    { "24ab6a6c-14f1-4b49-8964-ecfcbce372a3", 0, "9th Street", "afec30e4-ddea-4ad9-895a-646cc4ff1b57", "CustomUser", "john.smith@gmail.com", false, "John", "Smith", false, null, "JOHN.SMITH@GMAIL.COM", "JSMITH", "AQAAAAEAACcQAAAAEEGjsjbxW2+JKUXu6cTO/ERkFIDK8AP/Z5chD7IcPhUJOtupGa2qVQ9DaEt00bkgFQ==", "+1 582-282-2749", false, "Admin", "79c7c07a-d832-41e7-8bfc-b2e865bee7f4", false, "jsmith" },
                    { "2e5be4cb-41c8-4265-8959-e6558a272b62", 0, "66th Street", "808c90b9-8a9b-4aab-83cb-c95bd7efa927", "CustomUser", "aurimas.trunchinskas@gmail.com", false, "Aurimas", "Trunchinskas", false, null, "AURIMAS.TRUNCHINSKAS@GMAIL.COM", "AURIT", "AQAAAAEAACcQAAAAEJ4CKiL5jtWv+18NxCU2oOW7ti/VVc63vgzW/8a6JD9mhHcBEpOzRopoUjM32Y0v2Q==", "+1 304-894-4852", false, "Consumer", "48690506-dabc-4635-9cbe-d1a40e75bf2e", false, "aurit" },
                    { "36838a09-6809-4423-964e-154dea2e45c0", 0, "Absolute Street 129", "37462927-00e0-4b11-8f5c-ba9ddf1a8e2e", "CustomUser", "viktor.popov@gmail.com", false, "Viktor", "Popov", false, null, "VIKTOR.POPOV@GMAIL.COM", "VIKTOR", "AQAAAAEAACcQAAAAELwPg41dtrHRY0RgJ1A/de7FkpZAzWGrPbw4NzIbMRtx0uTGxu1TpA29ZgxAt4L4Hg==", "+1 218-955-6366", false, "Consumer", "4963f408-553d-437d-a3a6-4a010de2d23c", false, "viktor" },
                    { "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", 0, "Los Olivos 112", "eb31ef58-c3ba-439d-8998-5be18e30ae18", "CustomUser", "johnatan.garcia@gmail.com", false, "Johnatan", "Garcia", false, null, "JOHNATAN.GARCIA@GMAIL.COM", "JOHNATAN", "AQAAAAEAACcQAAAAEO1QK6mcvRHqaSbx+KNqD3fUarYZ7u1Iu+OIvXZ3jFy+LM1nMWhbQsKL4aQy+pajjA==", "+1 218-955-6366", false, "Consumer", "8dea1fa1-d892-4a6a-98e5-458e6f8a25f8", false, "johnatan" },
                    { "6f57119a-6b89-43ed-8df4-4b70d5259548", 0, "66th Street", "3be876ca-5aba-4c00-b877-1bbcb88815f0", "CustomUser", "mark.tanarte@gmail.com", false, "Mark", "Tanarte", false, null, "MARK.TANARTE@GMAIL.COM", "MARKTAN", "AQAAAAEAACcQAAAAEBhKyu1nf2YNOFwcZmaTrJkKHYZa17Q9zECfjJzU/k2z2dOHvy8oUUMBK2Uk/H/IlQ==", "+1 409-861-1005", false, "Owner", "8d4ea5b6-05dc-4538-8abd-5200d8161a2e", false, "marktan" },
                    { "71a07f92-c8b6-47a8-8f1f-0eb340062e57", 0, "Summerville Lane 19", "fce5fe07-cb04-45b1-8d3f-2935b7743705", "CustomUser", "linda.jones@gmail.com", false, "Linda", "Jones", false, null, "LINDA.JONES@GMAIL.COM", "LINDA", "AQAAAAEAACcQAAAAED7pZKkRG3i3fIklTOfo3cS0z0AnUxOWkHD3VntxAlRz+m3MoEjeuy80BtNNPJ3DeA==", "+1 223-814-3940", false, "Owner", "2081dcba-bfb0-4f7a-a76b-f0f4637b88a7", false, "linda" },
                    { "74ea7ef1-0444-447a-9780-0b3a0126a20b", 0, "Sintagma Square 106", "8a155324-8995-4740-8b90-b0264d6a7f28", "CustomUser", "ramon.altamiranda@gmail.com", false, "Ramon", "Altamiranda", false, null, "RAMON.ALTAMIRANDA@GMAIL.COM", "ALTARAMON", "AQAAAAEAACcQAAAAEMd2Alwq8CGvo+USX4SahpwulqVDi4V3xhRumSTZG5c7EiGN7UPokgijTDhj2lGRDQ==", "+1 505-753-6592", false, "Owner", "ffc863a6-b7b8-4605-8225-3f33025aeddf", false, "altaramon" },
                    { "94084a54-4f4e-4e86-805c-0ba0abdb1ec6", 0, "Green Street 99", "d60a1674-2101-42dd-bcc0-8dffa3ed7602", "CustomUser", "ehab.eshaak@gmail.com", false, "Ehab", "Eshaak", false, null, "EHAB.ESHAAK@GMAIL.COM", "EHABES", "AQAAAAEAACcQAAAAEC/445Uv5gqUFWxIBIvp2YIKVJrehWflzQ6bg3lmPynt6k71aSQ0g1hYaq+hdfWQqg==", "+1 315-919-1406", false, "Owner", "c0f657e8-5fd8-4aaa-8d91-6582c0beaad4", false, "ehabes" },
                    { "989b1e73-da14-4218-ac8c-d60aaf816520", 0, "Palmville Heights 24", "07e7aaac-0f64-48ee-84ee-63ad486ea387", "CustomUser", "alister.fernandez@gmail.com", false, "Alister", "Fernandez", false, null, "ALISTER.FERNANDEZ@GMAIL.COM", "ALISTER", "AQAAAAEAACcQAAAAEOXc+EiNdu+7BUTOgw/hOrZPrcxMHophevGDZ3eBRFtXVFENoyYzcAvPnQnWUOYeqw==", "+1 508-796-7186", false, "Consumer", "ab8f35ec-7d29-40b7-805c-5686273e6792", false, "alister" },
                    { "a73fc0f6-3559-4848-9224-099903fcdca2", 0, "Rapsberry Grow 154", "0038d985-3cdb-4f32-b00c-3bc9231a6d4d", "CustomUser", "cindy.shaw@gmail.com", false, "Cindy", "Shaw", false, null, "CINDY.SHAW@GMAIL.COM", "CINDY", "AQAAAAEAACcQAAAAEJfRxKZztEPwm6olG30bnd3WEUaEKJEVcxCpFJDH0BsuOdsvpAK90AAJBdinT28qMw==", "+1 582-333-4023", false, "Consumer", "a64a72fe-990f-474d-aed3-485b1d47588c", false, "cindy" },
                    { "b48c3cdd-dc9a-4d9d-af2c-420a68556126", 0, "Blackpot Square 12", "dcbd83aa-34f3-432f-990f-9e884bedc47a", "CustomUser", "carlos.benavidez@gmail.com", false, "Carlos", "Benavidez", false, null, "CARLOS.BENAVIDEZ@GMAIL.COM", "CARLOS", "AQAAAAEAACcQAAAAEM0joBAQ+tQfalENy4dbUP1qGNP5hdSCJUVT/LyYV03w6+AwRj5udlspzV+JIu4rjg==", "+1 215-293-3691", false, "Consumer", "e1bd2ec3-e419-410f-91a7-163db176382b", false, "carlos" },
                    { "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", 0, "Hell's Kitchen 6A", "bde62843-b43d-42c1-a5ed-923c2b9f134f", "CustomUser", "martina.salerno@gmail.com", false, "Martina", "Salerno", false, null, "MARTINA.SALERNO@GMAIL.COM", "MARTINA", "AQAAAAEAACcQAAAAEMaSDyZHVezgck34uLHa3+w0isT7C315HK8MSgTgWTtvePDsQD9SUnprjJ0YCaoElw==", "+1 505-644-9019", false, "Consumer", "96fb5552-7d90-47d7-beb8-f2cf92a53cef", false, "martina" },
                    { "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f", 0, "Silverlake Lane 96", "39d641bf-334c-4109-9736-eb098081815f", "CustomUser", "monica.bordei@gmail.com", false, "Monica", "Bordei", false, null, "MONICA.BORDEI@GMAIL.COM", "MONICA", "AQAAAAEAACcQAAAAEOLqMoEadYvuaYnwaylSMPxOQAoCxjSSPAKF0WH79cMo8NPb5YCSXtCnoCdz641+4A==", "+1 582-322-0444", false, "Owner", "8887da5b-81eb-4b7e-b037-59d7a0a6c1fa", false, "monica" },
                    { "dbf1bf5c-8485-4ebb-9d83-3806149d8048", 0, "22nd Jump Street", "32818d30-9730-4b41-9f8b-fed77170c8ed", "CustomUser", "vishnu.gawas@gmail.com", false, "Vishnu", "Gawas", false, null, "VISHNU.GAWAS@GMAIL.COM", "VISHNU", "AQAAAAEAACcQAAAAEEQqV8ib3VXaFui8PbzBEOct1pNFPKfh2EofgD2BPecZngo4NgKOIljLpsYBw9HjwQ==", "+1 213-354-2486", false, "Consumer", "6b7a6f8a-fe01-44c8-8bc1-1bca3b83c45b", false, "vishnu" },
                    { "e8952694-1ca9-44b1-a8fa-73988bb4eee5", 0, "Barksdale Boulevard 506", "75e66499-faca-466b-8c99-d34f3f08bab0", "CustomUser", "mohinder.pathania@gmail.com", false, "Mohinder", "Pathania", false, null, "MOHINDER.PATHANIA@GMAIL.COM", "MOHINDER", "AQAAAAEAACcQAAAAEBoRh3Vo/dj3A3Xvhxu09EH/EWIyE2CTh4FdENnx4LBTlbn6a0DJ8QllNZBjPKxzzw==", "+1 509-243-9105", false, "Owner", "3b3b21a7-ef76-4c1f-8c72-fb0140cdc609", false, "mohinder" },
                    { "edebb245-2066-4126-b9e4-dc020ffdafe7", 0, "Yellow Roof Street 66", "ff2cc63f-9cbc-4fb1-ab4f-75297b72b6d2", "CustomUser", "andry.goncharenko@gmail.com", false, "Andry", "Goncharenko", false, null, "ANDRY.GONCHARENKO@GMAIL.COM", "ANDRY", "AQAAAAEAACcQAAAAEP3g7dUGIr3y030jkCa0FoBDELyDeFJfpIEK/8/AaDPGItsNh+HKf+vij2CGJ/AQ9Q==", "+1 410-470-4327", false, "Owner", "5e8d4c4c-7d46-47b2-927c-b8ea7b643c87", false, "andry" },
                    { "f02b000c-622d-4c3f-b215-7e08cea2469c", 0, "Timberwood Fall 64", "c9f62b72-ebb2-49a4-bd6d-71c8a9d98b46", "CustomUser", "alex.petcu@gmail.com", false, "Alex", "Petcu", false, null, "ALEX.PETCU@GMAIL.COM", "ALEXP", "AQAAAAEAACcQAAAAEEbEk7f7ta78haru/GMLJY9PhL8QBfa3dlAGr2a/BRA45ucs1yxAJMGsgTRVZIvM6g==", "+1 423-923-5656", false, "Owner", "2da6bd5d-5a5e-485a-b504-91b34066b583", false, "alexp" },
                    { "f4352621-5ced-4afa-854f-49a10819d206", 0, "Iron Boulevard 45", "d179bbce-a5fc-4f73-b241-1e70e79158e6", "CustomUser", "michael.santos@gmail.com", false, "Michael", "Santos", false, null, "MICHAEL.SANTOS@GMAIL.COM", "MSANTOS", "AQAAAAEAACcQAAAAELuls3X+IJZzxOIXvclhfC1f+aVWKlHiBct9SzfvFsuCjuYZtbKgmXqp5q96qy9r5w==", "+1 262-589-1904", false, "Owner", "cdcc921c-c4dc-4875-9f0f-54ad493f3311", false, "msantos" }
                });

            migrationBuilder.InsertData(
                table: "CarWashsShops",
                columns: new[] { "Id", "Address", "AdvertisingDescription", "AmountOfWashingUnits", "ClosingTime", "Name", "OpeningTime" },
                values: new object[,]
                {
                    { 1, "Sunshine road 99", "Fast, Clean and Waterloo", 10, 23, "Waterloo", 8 },
                    { 2, "Black desert street 75", "Thorough and professional cleaning", 8, 23, "Geyser Blaze", 8 },
                    { 3, "Mellwood Pine 44", "Biggest in the city", 25, 23, "Vehicle Washing Center", 8 },
                    { 4, "Rocky Mountain 56", "Pure and clean", 6, 23, "Real Wash", 8 },
                    { 5, "Main Square 96", "Super fast and furious", 12, 23, "Phantom Wash", 8 },
                    { 6, "Pinapple Block 82", "Refresh your vehicle", 5, 23, "BubbleTime", 8 },
                    { 7, "Melon Valley 27", "Best you've seen so far", 6, 23, "Purifying Station", 8 },
                    { 8, "Dusty Road 33", "Get you done fast and smooth", 8, 23, "EazyPizzy", 8 },
                    { 9, "Sunrise Hill 206", "Your car's appearance matters", 10, 23, "Emerald Wash", 8 },
                    { 10, "Riverside Downstreet 66", "Can't ignore the quality", 25, 23, "Tsunami Wash", 8 }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Basic outside cleaning", "Tsunami STANDARD", 10.75m },
                    { 2, "STANDARD + extra polishing", "Tsunami EXTENDED", 14.99m },
                    { 3, "EXTENDED + inside deep cleaning", "Tsunami GRAND", 18.25m },
                    { 4, "GRAND + free beer and wifi while you wait", "Tsunami PREMIUM", 23.50m },
                    { 5, "desc", "Emerald BRIGHT", 12.50m },
                    { 6, "desc", "Emerald SHINE", 16.75m },
                    { 7, "desc", "Emerald DIVINE", 24.99m },
                    { 8, "desc", "EazyPizzy FASTACTION", 10m },
                    { 9, "desc", "EazyPizzy STANDARD", 14.20m },
                    { 10, "desc", "EazyPizzy SPECIALACTION", 19.25m },
                    { 11, "desc", "Purifying BASIC", 12m }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 12, "desc", "Purifying STANDARD", 15.75m },
                    { 13, "desc", "Purifying FULL", 20m },
                    { 14, "desc", "Bubble CASUAL", 7.99m },
                    { 15, "desc", "Bubble EXTRA", 12m },
                    { 16, "desc", "Bubble FLOOD", 16.25m },
                    { 17, "desc", "Phantom CLASS", 12.75m },
                    { 18, "desc", "Phantom OUTSTANDING", 16.50m },
                    { 19, "desc", "Phantom LEGENDARY", 22.90m },
                    { 20, "desc", "RealWash CLEAN PACK", 12m },
                    { 21, "desc", "RealWash BRIGHT PACK", 14.75m },
                    { 22, "desc", "RealWash GODLIKE SHINE PACK", 18.60m },
                    { 23, "desc", "Vehicle LIGHT", 11.50m },
                    { 24, "desc", "Vehicle STANDARD", 15.50m },
                    { 25, "desc", "Vehicle MARVELOUS", 19.99m },
                    { 26, "desc", "Geyser LIGHT", 10.70m },
                    { 27, "desc", "Geyser MEDIUM", 15.25m },
                    { 28, "desc", "Geyser BLAZE", 18.75m },
                    { 29, "desc", "Waterloo STANDARD", 12.20m },
                    { 30, "desc", "Waterloo PREMIUM", 16.99m }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "24ab6a6c-14f1-4b49-8964-ecfcbce372a3" },
                    { 2, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "6f57119a-6b89-43ed-8df4-4b70d5259548" },
                    { 3, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "1741abab-ff61-4598-a947-9c798c4ff866" },
                    { 4, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "f4352621-5ced-4afa-854f-49a10819d206" },
                    { 5, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "74ea7ef1-0444-447a-9780-0b3a0126a20b" },
                    { 6, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "94084a54-4f4e-4e86-805c-0ba0abdb1ec6" },
                    { 7, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "edebb245-2066-4126-b9e4-dc020ffdafe7" },
                    { 8, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "e8952694-1ca9-44b1-a8fa-73988bb4eee5" },
                    { 9, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f" },
                    { 10, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "f02b000c-622d-4c3f-b215-7e08cea2469c" },
                    { 11, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "71a07f92-c8b6-47a8-8f1f-0eb340062e57" },
                    { 12, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "b48c3cdd-dc9a-4d9d-af2c-420a68556126" },
                    { 13, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "06638581-8f0c-4119-a637-e4f3b5bbe858" },
                    { 14, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "a73fc0f6-3559-4848-9224-099903fcdca2" },
                    { 15, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "dbf1bf5c-8485-4ebb-9d83-3806149d8048" },
                    { 16, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "989b1e73-da14-4218-ac8c-d60aaf816520" },
                    { 17, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "c4b2e35a-d562-483a-9c89-f4a3d3d59e77" },
                    { 18, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "56c4a3a6-cc46-4c6d-85cd-2d19a25835df" },
                    { 19, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "36838a09-6809-4423-964e-154dea2e45c0" },
                    { 20, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "2e5be4cb-41c8-4265-8959-e6558a272b62" },
                    { 21, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Consumer", "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingStatus", "CarWashShopId", "ConsumerId", "DateCreated", "IsPaid", "ScheduledDateTime", "ServiceId" },
                values: new object[,]
                {
                    { 1, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 30 },
                    { 2, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 29 },
                    { 3, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 30 },
                    { 4, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 30 },
                    { 5, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), 29 },
                    { 6, 2, 2, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 7, 2, 2, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), 26 },
                    { 8, 2, 2, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 27 },
                    { 9, 2, 3, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 10, 2, 3, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 11, 2, 2, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 12, 2, 2, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 26 },
                    { 13, 2, 2, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 14, 2, 2, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 27 },
                    { 15, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 16, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 17, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 18, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 19, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 20, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 21, 2, 3, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 25 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingStatus", "CarWashShopId", "ConsumerId", "DateCreated", "IsPaid", "ScheduledDateTime", "ServiceId" },
                values: new object[,]
                {
                    { 22, 2, 3, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 23, 2, 3, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 2, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 24, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 25, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 26, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 20 },
                    { 27, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 28, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 29, 2, 5, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 30, 2, 5, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 31, 2, 4, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 32, 2, 4, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 19, 10, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 33, 2, 4, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 34, 2, 4, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 20 },
                    { 35, 2, 5, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 36, 2, 5, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 37, 2, 5, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 17 },
                    { 38, 2, 5, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 39, 2, 6, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 40, 2, 6, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 41, 2, 5, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 3, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 42, 2, 5, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 43, 2, 5, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 17 },
                    { 44, 2, 5, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 17 },
                    { 45, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 46, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 4, 29, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 47, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 5, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 48, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 5, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 49, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 5, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 50, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 5, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 51, 2, 5, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 5, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 52, 2, 5, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 5, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 53, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 54, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 55, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 56, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 57, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 58, 2, 7, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 59, 2, 7, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 60, 2, 7, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 61, 2, 6, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 62, 2, 6, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 63, 2, 6, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingStatus", "CarWashShopId", "ConsumerId", "DateCreated", "IsPaid", "ScheduledDateTime", "ServiceId" },
                values: new object[,]
                {
                    { 64, 2, 6, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 65, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 66, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 67, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 68, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 69, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 70, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 71, 2, 7, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 72, 2, 7, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 73, 2, 7, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 74, 2, 7, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 75, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 76, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 77, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 78, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 79, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 80, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 81, 2, 8, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 82, 2, 8, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 6, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 83, 2, 8, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 84, 2, 8, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 85, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 86, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 87, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 88, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 89, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 90, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 91, 2, 9, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 92, 2, 9, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 93, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 94, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 95, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 96, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 97, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 98, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 21, 10, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 99, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 100, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 8, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "CarWashShopsOwners",
                columns: new[] { "CarWashShopId", "OwnerId" },
                values: new object[,]
                {
                    { 2, "1741abab-ff61-4598-a947-9c798c4ff866" },
                    { 1, "6f57119a-6b89-43ed-8df4-4b70d5259548" },
                    { 2, "6f57119a-6b89-43ed-8df4-4b70d5259548" },
                    { 10, "6f57119a-6b89-43ed-8df4-4b70d5259548" },
                    { 10, "71a07f92-c8b6-47a8-8f1f-0eb340062e57" }
                });

            migrationBuilder.InsertData(
                table: "CarWashShopsOwners",
                columns: new[] { "CarWashShopId", "OwnerId" },
                values: new object[,]
                {
                    { 3, "74ea7ef1-0444-447a-9780-0b3a0126a20b" },
                    { 3, "94084a54-4f4e-4e86-805c-0ba0abdb1ec6" },
                    { 4, "94084a54-4f4e-4e86-805c-0ba0abdb1ec6" },
                    { 6, "94084a54-4f4e-4e86-805c-0ba0abdb1ec6" },
                    { 7, "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f" },
                    { 10, "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f" },
                    { 5, "e8952694-1ca9-44b1-a8fa-73988bb4eee5" },
                    { 6, "edebb245-2066-4126-b9e4-dc020ffdafe7" },
                    { 8, "f02b000c-622d-4c3f-b215-7e08cea2469c" },
                    { 9, "f02b000c-622d-4c3f-b215-7e08cea2469c" },
                    { 3, "f4352621-5ced-4afa-854f-49a10819d206" }
                });

            migrationBuilder.InsertData(
                table: "CarWashShopsServices",
                columns: new[] { "CarWashShopId", "ServiceId" },
                values: new object[,]
                {
                    { 10, 1 },
                    { 10, 2 },
                    { 10, 3 },
                    { 10, 4 },
                    { 9, 5 },
                    { 9, 6 },
                    { 9, 7 },
                    { 8, 8 },
                    { 8, 9 },
                    { 8, 10 },
                    { 7, 11 },
                    { 7, 12 },
                    { 7, 13 },
                    { 6, 14 },
                    { 6, 15 },
                    { 6, 16 },
                    { 5, 17 },
                    { 5, 18 },
                    { 5, 19 },
                    { 4, 20 },
                    { 4, 21 },
                    { 4, 22 },
                    { 3, 23 },
                    { 3, 24 },
                    { 3, 25 },
                    { 2, 26 },
                    { 2, 27 },
                    { 2, 28 },
                    { 1, 29 },
                    { 1, 30 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CarWashShopId",
                table: "Bookings",
                column: "CarWashShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ConsumerId",
                table: "Bookings",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceId",
                table: "Bookings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CarWashShopsOwners_CarWashShopId",
                table: "CarWashShopsOwners",
                column: "CarWashShopId");

            migrationBuilder.CreateIndex(
                name: "IX_CarWashShopsServices_CarWashShopId",
                table: "CarWashShopsServices",
                column: "CarWashShopId");

            migrationBuilder.CreateIndex(
                name: "IX_CarWashsShops_Name",
                table: "CarWashsShops",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerRemovalRequests_CarWashShopId",
                table: "OwnerRemovalRequests",
                column: "CarWashShopId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerRemovalRequests_OwnerToBeRemovedId",
                table: "OwnerRemovalRequests",
                column: "OwnerToBeRemovedId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerRemovalRequests_RequesterId",
                table: "OwnerRemovalRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopRemovalRequests_CarWashShopId",
                table: "ShopRemovalRequests",
                column: "CarWashShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopRemovalRequests_OwnerId",
                table: "ShopRemovalRequests",
                column: "OwnerId");


            // STORED PROCEDURE TO GET SHOP OVERVIEW OF DAILY,WEEKLY,MONTHLY,YEARLY INCOME

            string shopCreateProcedure = @" CREATE PROCEDURE spGetShopIncome

			@UserName nvarchar(100),
			@ShopId int,
			@DateType varchar(10),
			@Year varchar(10),
			@RecordsPerSearch int,
			@StartingDate datetime = NULL,
			@EndingDate datetime = NULL

			AS
			BEGIN
			SET NOCOUNT ON;

			SELECT TOP(@RecordsPerSearch)
			b.CarWashShopId,
			cws.Name CarWashShopName,
			CASE 
				WHEN @DateType = 'DAY' THEN DATEPART(DAYOFYEAR,ScheduledDateTime)
				WHEN @DateType = 'WEEK' THEN DATEPART(WEEK, ScheduledDateTime)
				WHEN @DateType = 'MONTH' THEN DATEPART(MONTH, ScheduledDateTime)
				WHEN @DateType = 'YEAR' THEN DATEPART(YEAR, ScheduledDateTime)
			END Calendar,

			CASE 
				WHEN @DateType = 'DAY' THEN ScheduledDateTime
				ELSE '10000101'
			END Date,
			SUM(s.Price) Income


			FROM dbo.Bookings b
			JOIN dbo.Services s ON s.Id = b.ServiceId
			JOIN dbo.CarWashShopsOwners o ON o.CarWashShopId = b.CarWashShopId
			JOIN dbo.CarWashsShops cws ON cws.Id = b.CarWashShopId
			WHERE o.OwnerId = 
				(
				SELECT u.Id
				FROM AspNetUsers u
				WHERE u.UserName = @UserName
				)
				AND b.CarWashShopId = @ShopId 
				AND b.IsPaid = 1
				AND YEAR(b.ScheduledDateTime) = @Year
				AND (CAST(b.ScheduledDateTime AS DATE) >= @StartingDate OR @StartingDate IS NULL)
				AND (CAST(b.ScheduledDateTime AS DATE) <= @EndingDate OR @EndingDate IS NULL)
			GROUP BY 
			CASE
				WHEN @DateType = 'DAY' THEN DATEPART(DAYOFYEAR, ScheduledDateTime)
				WHEN @DateType = 'WEEK' THEN DATEPART(WEEK, ScheduledDateTime)
				WHEN @DateType = 'MONTH' THEN DATEPART(MONTH, ScheduledDateTime)
				WHEN @DateType = 'YEAR' THEN DATEPART(YEAR, ScheduledDateTime)
			END,
			CASE 
				WHEN @DateType = 'DAY' THEN ScheduledDateTime
				ELSE '10000101'
			END,
			b.CarWashShopId,
			cws.Name

			ORDER BY Calendar 
			END";



            string shopDropProcedure = @"IF OBJECT_ID('spGetShopIncome') IS NOT NULL DROP PROCEDURE spGetShopIncome";

            migrationBuilder.Sql(shopDropProcedure);
            migrationBuilder.Sql(shopCreateProcedure);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "CarWashShopsOwners");

            migrationBuilder.DropTable(
                name: "CarWashShopsServices");

            migrationBuilder.DropTable(
                name: "Income");

            migrationBuilder.DropTable(
                name: "OwnerRemovalRequests");

            migrationBuilder.DropTable(
                name: "ShopRemovalRequests");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CarWashsShops");
        }
    }
}
