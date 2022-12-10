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
                    ClosingTime = table.Column<int>(type: "int", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isInRemovalProcess = table.Column<bool>(type: "bit", nullable: false)
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
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarWashShopId = table.Column<int>(type: "int", nullable: false),
                    PaymentDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_CarWashsShops_CarWashShopId",
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
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                    { "06638581-8f0c-4119-a637-e4f3b5bbe858", 0, "Redcliff View 78", "0582d24c-3c85-480b-a72f-4d52a7781d7c", "CustomUser", "wadzanai.bote@gmail.com", false, "Wadzanai", "Bote", false, null, "WADZANAI.BOTE@GMAIL.COM", "WADZA", "AQAAAAEAACcQAAAAEHpo0qcmOwtLWiVDkIcM1QWSpmupoRu7/O94y3JEZKUDojNd/HrcgZDTqcsROAEqdA==", "+1 582-444-7776", false, "Consumer", "e9fad07d-bc0d-40ae-86df-57c851a6b3fe", false, "wadza" },
                    { "1741abab-ff61-4598-a947-9c798c4ff866", 0, "Pinewood Heights 70", "7e900eb6-5fab-475b-9116-e198a89a2ab1", "CustomUser", "vese.calin@gmail.com", false, "Vese", "Calin", false, null, "VESE.CALIN@GMAIL.COM", "VESEC", "AQAAAAEAACcQAAAAEI+CCy//ONWFEQ3DXugFws3aDCGi4ia/8lwL2vatiJ0ujPL5UIWmNDlcvdFSOElj7g==", "+1 312-251-6119", false, "Owner", "984c1731-c6d7-4e3b-b991-b6df2e9a0415", false, "vesec" },
                    { "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", 0, "Whiteroof Valley", "b6b3c38f-7df7-4073-98ca-87c7eb8a6387", "CustomUser", "robert.bradford@gmail.com", false, "Robert", "Bradford", false, null, "ROBERT.BRADFORD@GMAIL.COM", "ROBBER", "AQAAAAEAACcQAAAAEDAkHQ3tS9UtXQtlGUwM1bB5itg79pUL8CU7Gr/fLBoaNGnVzf2lmJVUyLy0U8EEbA==", "+1 304-894-4852", false, "Consumer", "6f6d3a60-1d60-450d-9ba9-29f54bec6d55", false, "robber" },
                    { "24ab6a6c-14f1-4b49-8964-ecfcbce372a3", 0, "9th Street", "7b3054a9-3d35-4d99-bef6-529b31fa0529", "CustomUser", "john.smith@gmail.com", false, "John", "Smith", false, null, "JOHN.SMITH@GMAIL.COM", "JSMITH", "AQAAAAEAACcQAAAAEANl+4bsr3tQVj53c+DKU+Sudpg08zSaB21/rJtsqhVYJSfeuxzE1zUqSlyy9BuT7Q==", "+1 582-282-2749", false, "Admin", "4252c3a7-7c4a-45df-97bf-3de4efc387a0", false, "jsmith" },
                    { "2e5be4cb-41c8-4265-8959-e6558a272b62", 0, "66th Street", "5cbfff07-5de4-4d2b-8798-e617d6868e5d", "CustomUser", "aurimas.trunchinskas@gmail.com", false, "Aurimas", "Trunchinskas", false, null, "AURIMAS.TRUNCHINSKAS@GMAIL.COM", "AURIT", "AQAAAAEAACcQAAAAEFA4+GzB8xdLCnXZYqV4fQBkqTON08bqD/RO3XaoIS1fZzkCE1rErhMCxEm2FAMFvw==", "+1 304-894-4852", false, "Consumer", "88dcf5cb-cf28-4c40-a88b-7cb075cafca2", false, "aurit" },
                    { "36838a09-6809-4423-964e-154dea2e45c0", 0, "Absolute Street 129", "44a5167d-80be-404d-a035-2ac8af0b453d", "CustomUser", "viktor.popov@gmail.com", false, "Viktor", "Popov", false, null, "VIKTOR.POPOV@GMAIL.COM", "VIKTOR", "AQAAAAEAACcQAAAAEA41VoA6/HSnIze4xief7pcP588gZVkiUo1qYgrYOKN7O+u9wFW4M2RUOYyahee+Fg==", "+1 218-955-6366", false, "Consumer", "c601539d-10cc-4980-a0db-8022e9f45a70", false, "viktor" },
                    { "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", 0, "Los Olivos 112", "5ffe38de-e86d-457e-a3fd-f29b4d5b0fc3", "CustomUser", "johnatan.garcia@gmail.com", false, "Johnatan", "Garcia", false, null, "JOHNATAN.GARCIA@GMAIL.COM", "JOHNATAN", "AQAAAAEAACcQAAAAEK+Gk1CukNHWdd0pH+FNfqNaXwHUJWmJaxlGyrNtKPj1ah11d4EtQcNMLVXUTBJ2ag==", "+1 218-955-6366", false, "Consumer", "d23c0f6c-4f84-4687-bd22-cea8f07aeb55", false, "johnatan" },
                    { "6f57119a-6b89-43ed-8df4-4b70d5259548", 0, "66th Street", "edb2225c-8845-44f0-a9df-7e47683a68b8", "CustomUser", "mark.tanarte@gmail.com", false, "Mark", "Tanarte", false, null, "MARK.TANARTE@GMAIL.COM", "MARKTAN", "AQAAAAEAACcQAAAAEARO9b6HoVcurmZ/zPIsja/4Ej9Djss47gmffFp3gIUZHFCl5uUXRh5dsB4tkeVGUg==", "+1 409-861-1005", false, "Owner", "284b8eb6-aff2-43fc-92b5-5c5d61326202", false, "marktan" },
                    { "71a07f92-c8b6-47a8-8f1f-0eb340062e57", 0, "Summerville Lane 19", "d75bc7a1-96bf-4f6a-8c7f-176690f9ae4c", "CustomUser", "linda.jones@gmail.com", false, "Linda", "Jones", false, null, "LINDA.JONES@GMAIL.COM", "LINDA", "AQAAAAEAACcQAAAAECvUoYflh/dh1fZf/2BdDoeUONF414arnk6sId/BbgVOWpxmkzSU9WmOGLAJsPrGwg==", "+1 223-814-3940", false, "Owner", "487b1148-a3e9-4630-adf1-4ce48dfdba4b", false, "linda" },
                    { "74ea7ef1-0444-447a-9780-0b3a0126a20b", 0, "Sintagma Square 106", "3518a1d8-5a16-400b-aeed-d2c830295756", "CustomUser", "ramon.altamiranda@gmail.com", false, "Ramon", "Altamiranda", false, null, "RAMON.ALTAMIRANDA@GMAIL.COM", "ALTARAMON", "AQAAAAEAACcQAAAAEAgF+sswBKLU6pkBAAefOXKNxVkzCu+LKWkaJX3qiay++ccbZIxDBCAHRlAXbsRWoA==", "+1 505-753-6592", false, "Owner", "629f05d9-d964-4264-ab7a-8d87c2124e5b", false, "altaramon" },
                    { "94084a54-4f4e-4e86-805c-0ba0abdb1ec6", 0, "Green Street 99", "024d2356-c329-421d-90d2-19842c0ff0d5", "CustomUser", "ehab.eshaak@gmail.com", false, "Ehab", "Eshaak", false, null, "EHAB.ESHAAK@GMAIL.COM", "EHABES", "AQAAAAEAACcQAAAAEHO9JKJ/pF4iCcvHqMf14187YNpDBl82JYOoL756QOFBQbWbvqmz/ovILY+aRJEavg==", "+1 315-919-1406", false, "Owner", "7ed037fc-f183-442c-af53-d61f78078f1d", false, "ehabes" },
                    { "989b1e73-da14-4218-ac8c-d60aaf816520", 0, "Palmville Heights 24", "470e8105-ca10-4872-ba78-603b2006535d", "CustomUser", "alister.fernandez@gmail.com", false, "Alister", "Fernandez", false, null, "ALISTER.FERNANDEZ@GMAIL.COM", "ALISTER", "AQAAAAEAACcQAAAAEBAQEBbzqtyC3YOYh8k8pC4/2FsSLi8B9Bxrbzpkm5AYb3iNkJzPS1KEbbta/Is83g==", "+1 508-796-7186", false, "Consumer", "99575f6d-6448-46f1-aec2-c576243bbccf", false, "alister" },
                    { "a73fc0f6-3559-4848-9224-099903fcdca2", 0, "Rapsberry Grow 154", "b7dff43f-6b29-4256-bcf1-cb6366eeacb6", "CustomUser", "cindy.shaw@gmail.com", false, "Cindy", "Shaw", false, null, "CINDY.SHAW@GMAIL.COM", "CINDY", "AQAAAAEAACcQAAAAEAlLvT9IVMuM5dIJ/wwv4ECPV8HlFB2OwOdnsdPT9Ic37yweyIsPpdh3o6Cx4H+EBg==", "+1 582-333-4023", false, "Consumer", "1f426c1b-fe9d-4e50-b40c-1433988719cb", false, "cindy" },
                    { "b48c3cdd-dc9a-4d9d-af2c-420a68556126", 0, "Blackpot Square 12", "7ea99012-8e29-43c7-8adc-8d38b55c645a", "CustomUser", "carlos.benavidez@gmail.com", false, "Carlos", "Benavidez", false, null, "CARLOS.BENAVIDEZ@GMAIL.COM", "CARLOS", "AQAAAAEAACcQAAAAEPmkiDpdCXUtviclps9rsAklv8+0opwqojhcVnebTHuH2/GTwb2f4Gestd2ePpud+w==", "+1 215-293-3691", false, "Consumer", "e571d693-f420-4783-a175-ef02e22c8f1b", false, "carlos" },
                    { "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", 0, "Hell's Kitchen 6A", "a9d2a036-e9e2-4527-a8fb-1ad83420ec31", "CustomUser", "martina.salerno@gmail.com", false, "Martina", "Salerno", false, null, "MARTINA.SALERNO@GMAIL.COM", "MARTINA", "AQAAAAEAACcQAAAAEAjvLy5AnWx+fiaMnDLIWfcfiEWjg8SR2bCF2oXpzlh3PGI8ixr1fifW1MZrNXyrQQ==", "+1 505-644-9019", false, "Consumer", "4b873aae-2d43-425d-9809-938c987a4978", false, "martina" },
                    { "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f", 0, "Silverlake Lane 96", "df00461c-f9ac-4745-9178-5983e2158eb8", "CustomUser", "monica.bordei@gmail.com", false, "Monica", "Bordei", false, null, "MONICA.BORDEI@GMAIL.COM", "MONICA", "AQAAAAEAACcQAAAAECTIC6Y91BlqQ/HNPn5/QnhQr/XVIxTz2dbdBaffgB+Qo+PpGAoJ1+Sy9C1/fVFksA==", "+1 582-322-0444", false, "Owner", "28873f0d-9f91-452c-a957-eec3a5bac567", false, "monica" },
                    { "dbf1bf5c-8485-4ebb-9d83-3806149d8048", 0, "22nd Jump Street", "d6b6c200-33b4-48dd-835c-144f20e07327", "CustomUser", "vishnu.gawas@gmail.com", false, "Vishnu", "Gawas", false, null, "VISHNU.GAWAS@GMAIL.COM", "VISHNU", "AQAAAAEAACcQAAAAEJMSLrNKuvVPPsujaPuUWaELFF04z7ZXCkJPhiUX8LmqaoMcK+3bc5d1wHzB90wMhA==", "+1 213-354-2486", false, "Consumer", "d45aa85f-ce2e-4c97-8004-07d1ffc3bca4", false, "vishnu" },
                    { "e8952694-1ca9-44b1-a8fa-73988bb4eee5", 0, "Barksdale Boulevard 506", "4031ceb4-92ea-4b60-b72e-e45fb20953d6", "CustomUser", "mohinder.pathania@gmail.com", false, "Mohinder", "Pathania", false, null, "MOHINDER.PATHANIA@GMAIL.COM", "MOHINDER", "AQAAAAEAACcQAAAAECp1BkRcmo5O5K0mL/qRDjSubvKRHwfphhyrnQWbVa+ZbQ9uKznATGYPRY/mUW939A==", "+1 509-243-9105", false, "Owner", "c9077d2d-23ff-4c2b-9d4a-1eaff40f866b", false, "mohinder" },
                    { "edebb245-2066-4126-b9e4-dc020ffdafe7", 0, "Yellow Roof Street 66", "77b31d9f-e028-405d-a2d1-891036b14874", "CustomUser", "andry.goncharenko@gmail.com", false, "Andry", "Goncharenko", false, null, "ANDRY.GONCHARENKO@GMAIL.COM", "ANDRY", "AQAAAAEAACcQAAAAENvVVBCVdWGE9XMfUcKtjg9NJYf7a1HtdM9tnAjmt4SR6GW+Ce7ZuY/p3GGIE8EUog==", "+1 410-470-4327", false, "Owner", "dd93c354-8956-431e-84be-53072e381ee7", false, "andry" },
                    { "f02b000c-622d-4c3f-b215-7e08cea2469c", 0, "Timberwood Fall 64", "25ea10ef-4701-4839-b49f-d9c0d5c69ece", "CustomUser", "alex.petcu@gmail.com", false, "Alex", "Petcu", false, null, "ALEX.PETCU@GMAIL.COM", "ALEXP", "AQAAAAEAACcQAAAAENpM5GqpzQfw1OqaztWVoJJI3/ZS8Z5kwC+UtyVwgDcUJmp439QqsvxyMiZeH1/nIg==", "+1 423-923-5656", false, "Owner", "a09f133a-e2f1-4b6f-a6c4-10660633f302", false, "alexp" },
                    { "f4352621-5ced-4afa-854f-49a10819d206", 0, "Iron Boulevard 45", "91f3a125-6c3e-4b3d-ba3e-ee792e885c27", "CustomUser", "michael.santos@gmail.com", false, "Michael", "Santos", false, null, "MICHAEL.SANTOS@GMAIL.COM", "MSANTOS", "AQAAAAEAACcQAAAAEJvinmp1jOUoghdkdPAO9TGZAFriPbWkNfkpOu7BExHDJdCHLznNa2BfT9WYzZA8AA==", "+1 262-589-1904", false, "Owner", "8f942c87-e175-4e8d-bba2-a8cb7e81b4b4", false, "msantos" }
                });

            migrationBuilder.InsertData(
                table: "CarWashsShops",
                columns: new[] { "Id", "Address", "AdvertisingDescription", "AmountOfWashingUnits", "ClosingTime", "Name", "OpeningTime", "Revenue", "isInRemovalProcess" },
                values: new object[,]
                {
                    { 1, "Sunshine road 99", "Fast, Clean and Waterloo", 10, 23, "Waterloo", 8, 0m, false },
                    { 2, "Black desert street 75", "Thorough and professional cleaning", 8, 23, "Geyser Blaze", 8, 0m, false },
                    { 3, "Mellwood Pine 44", "Biggest in the city", 25, 23, "Vehicle Washing Center", 8, 0m, false },
                    { 4, "Rocky Mountain 56", "Pure and clean", 6, 23, "Real Wash", 8, 0m, false },
                    { 5, "Main Square 96", "Super fast and furious", 12, 23, "Phantom Wash", 8, 0m, false },
                    { 6, "Pinapple Block 82", "Refresh your vehicle", 5, 23, "BubbleTime", 8, 0m, false },
                    { 7, "Melon Valley 27", "Best you've seen so far", 6, 23, "Purifying Station", 8, 0m, false },
                    { 8, "Dusty Road 33", "Get you done fast and smooth", 8, 23, "EazyPizzy", 8, 0m, false },
                    { 9, "Sunrise Hill 206", "Your car's appearance matters", 10, 23, "Emerald Wash", 8, 0m, false },
                    { 10, "Riverside Downstreet 66", "Can't ignore the quality", 25, 23, "Tsunami Wash", 8, 0m, false }
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
                columns: new[] { "Id", "BookingStatus", "CarWashShopId", "ConsumerId", "DateCreated", "IsPaid", "Price", "ScheduledDateTime", "ServiceId" },
                values: new object[,]
                {
                    { 1, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.99m, new DateTime(2022, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 30 },
                    { 2, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.20m, new DateTime(2022, 1, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 29 },
                    { 3, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.99m, new DateTime(2022, 1, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 30 },
                    { 4, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.99m, new DateTime(2022, 1, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 30 },
                    { 5, 2, 1, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.20m, new DateTime(2022, 1, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), 29 },
                    { 6, 2, 2, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.75m, new DateTime(2022, 1, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 7, 2, 2, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 10.70m, new DateTime(2022, 1, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), 26 },
                    { 8, 2, 2, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.25m, new DateTime(2022, 2, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 27 },
                    { 9, 2, 3, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 19.99m, new DateTime(2022, 2, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 10, 2, 3, "b48c3cdd-dc9a-4d9d-af2c-420a68556126", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.50m, new DateTime(2022, 2, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 11, 2, 2, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.75m, new DateTime(2022, 1, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 12, 2, 2, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 10.70m, new DateTime(2022, 2, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 26 },
                    { 13, 2, 2, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.75m, new DateTime(2022, 2, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 14, 2, 2, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.25m, new DateTime(2022, 2, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 27 },
                    { 15, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 19.99m, new DateTime(2022, 2, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 16, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.50m, new DateTime(2022, 2, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 17, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 11.50m, new DateTime(2022, 2, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 18, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 19.99m, new DateTime(2022, 2, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 19, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.50m, new DateTime(2022, 2, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 20, 2, 3, "06638581-8f0c-4119-a637-e4f3b5bbe858", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 11.50m, new DateTime(2022, 3, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 21, 2, 3, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 19.99m, new DateTime(2022, 2, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 25 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingStatus", "CarWashShopId", "ConsumerId", "DateCreated", "IsPaid", "Price", "ScheduledDateTime", "ServiceId" },
                values: new object[,]
                {
                    { 22, 2, 3, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.50m, new DateTime(2022, 2, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 23, 2, 3, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 11.50m, new DateTime(2022, 2, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 24, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.60m, new DateTime(2022, 3, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 25, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.75m, new DateTime(2022, 3, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 26, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.00m, new DateTime(2022, 3, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 20 },
                    { 27, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.60m, new DateTime(2022, 3, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 28, 2, 4, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.75m, new DateTime(2022, 3, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 29, 2, 5, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 22.90m, new DateTime(2022, 3, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 30, 2, 5, "a73fc0f6-3559-4848-9224-099903fcdca2", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.50m, new DateTime(2022, 3, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 31, 2, 4, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.60m, new DateTime(2022, 3, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 32, 2, 4, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.75m, new DateTime(2022, 3, 19, 10, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 33, 2, 4, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.60m, new DateTime(2022, 3, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 34, 2, 4, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.00m, new DateTime(2022, 3, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 20 },
                    { 35, 2, 5, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 22.90m, new DateTime(2022, 4, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 36, 2, 5, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.50m, new DateTime(2022, 4, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 37, 2, 5, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.75m, new DateTime(2022, 4, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 17 },
                    { 38, 2, 5, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.50m, new DateTime(2022, 4, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 39, 2, 6, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.25m, new DateTime(2022, 4, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 40, 2, 6, "dbf1bf5c-8485-4ebb-9d83-3806149d8048", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.00m, new DateTime(2022, 4, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 41, 2, 5, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 22.90m, new DateTime(2022, 3, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 42, 2, 5, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.50m, new DateTime(2022, 4, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 43, 2, 5, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.75m, new DateTime(2022, 4, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 17 },
                    { 44, 2, 5, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.75m, new DateTime(2022, 4, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 17 },
                    { 45, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.25m, new DateTime(2022, 4, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 46, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.00m, new DateTime(2022, 4, 29, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 47, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 7.99m, new DateTime(2022, 5, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 48, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 7.99m, new DateTime(2022, 5, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 49, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.00m, new DateTime(2022, 5, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 50, 2, 6, "989b1e73-da14-4218-ac8c-d60aaf816520", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.25m, new DateTime(2022, 5, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 51, 2, 5, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 22.90m, new DateTime(2022, 5, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 52, 2, 5, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.50m, new DateTime(2022, 5, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 53, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.25m, new DateTime(2022, 6, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 54, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.00m, new DateTime(2022, 6, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 55, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 7.99m, new DateTime(2022, 6, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 56, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.00m, new DateTime(2022, 6, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 57, 2, 6, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 7.99m, new DateTime(2022, 6, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 58, 2, 7, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 20.00m, new DateTime(2022, 6, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 59, 2, 7, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.75m, new DateTime(2022, 7, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 60, 2, 7, "c4b2e35a-d562-483a-9c89-f4a3d3d59e77", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 5.00m, new DateTime(2022, 7, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 61, 2, 6, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 7.99m, new DateTime(2022, 6, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 62, 2, 6, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.00m, new DateTime(2022, 6, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 63, 2, 6, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.25m, new DateTime(2022, 7, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingStatus", "CarWashShopId", "ConsumerId", "DateCreated", "IsPaid", "Price", "ScheduledDateTime", "ServiceId" },
                values: new object[,]
                {
                    { 64, 2, 6, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.25m, new DateTime(2022, 7, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 65, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 20.00m, new DateTime(2022, 7, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 66, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.75m, new DateTime(2022, 7, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 67, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 5.00m, new DateTime(2022, 7, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 68, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 5.00m, new DateTime(2022, 7, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 69, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.75m, new DateTime(2022, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 70, 2, 7, "56c4a3a6-cc46-4c6d-85cd-2d19a25835df", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 20.00m, new DateTime(2022, 8, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 71, 2, 7, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 20.00m, new DateTime(2022, 6, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 72, 2, 7, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 15.75m, new DateTime(2022, 6, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 73, 2, 7, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 5.00m, new DateTime(2022, 7, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 74, 2, 7, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 5.00m, new DateTime(2022, 7, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 75, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 19.25m, new DateTime(2022, 7, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 76, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.20m, new DateTime(2022, 7, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 77, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 10.00m, new DateTime(2022, 7, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 78, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 10.00m, new DateTime(2022, 7, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 79, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.20m, new DateTime(2022, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 80, 2, 8, "36838a09-6809-4423-964e-154dea2e45c0", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 19.25m, new DateTime(2022, 8, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 81, 2, 8, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.20m, new DateTime(2022, 6, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 82, 2, 8, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 19.25m, new DateTime(2022, 6, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 83, 2, 8, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 10.00m, new DateTime(2022, 7, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 84, 2, 8, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.20m, new DateTime(2022, 7, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 85, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 24.99m, new DateTime(2022, 7, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 86, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.75m, new DateTime(2022, 7, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 87, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.50m, new DateTime(2022, 7, 24, 10, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 88, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 12.50m, new DateTime(2022, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 89, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.75m, new DateTime(2022, 8, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 90, 2, 9, "2e5be4cb-41c8-4265-8959-e6558a272b62", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 24.99m, new DateTime(2022, 8, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 91, 2, 9, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 24.99m, new DateTime(2022, 7, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 92, 2, 9, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 16.75m, new DateTime(2022, 7, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 93, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 10.75m, new DateTime(2022, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 94, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.99m, new DateTime(2022, 8, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 95, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.25m, new DateTime(2022, 8, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 96, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 23.50m, new DateTime(2022, 8, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 97, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 23.50m, new DateTime(2022, 8, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 98, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 18.25m, new DateTime(2022, 8, 21, 10, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 99, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 14.99m, new DateTime(2022, 8, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 100, 2, 10, "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b", new DateTime(2022, 1, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 10.75m, new DateTime(2022, 8, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 1 }
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

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CarWashShopId",
                table: "Transactions",
                column: "CarWashShopId");



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
			t.CarWashShopId,
			c.Name CarWashShopName,
			CASE 
				WHEN @DateType = 'DAY' THEN DATEPART(DAYOFYEAR,PaymentDay)
				WHEN @DateType = 'WEEK' THEN DATEPART(WEEK, PaymentDay)
				WHEN @DateType = 'MONTH' THEN DATEPART(MONTH, PaymentDay)
				WHEN @DateType = 'YEAR' THEN DATEPART(YEAR, PaymentDay)
			END Calendar,

			CASE 
				WHEN @DateType = 'DAY' THEN PaymentDay
				ELSE '10000101'
			END Date,
			SUM(t.Amount) Income


			FROM dbo.CarWashsShops c
			JOIN dbo.CarWashShopsOwners o ON o.CarWashShopId = c.Id
			JOIN dbo.Transactions t ON t.CarWashShopId = c.Id
			WHERE o.OwnerId = 
				(
				SELECT u.Id
				FROM AspNetUsers u
				WHERE u.UserName = @UserName
				)
				AND t.CarWashShopId = @ShopId 
				AND YEAR(t.PaymentDay) = @Year
				AND (CAST(t.PaymentDay AS DATE) >= @StartingDate OR @StartingDate IS NULL)
				AND (CAST(t.PaymentDay AS DATE) <= @EndingDate OR @EndingDate IS NULL)
			GROUP BY 
			CASE
				WHEN @DateType = 'DAY' THEN DATEPART(DAYOFYEAR, PaymentDay)
				WHEN @DateType = 'WEEK' THEN DATEPART(WEEK, PaymentDay)
				WHEN @DateType = 'MONTH' THEN DATEPART(MONTH, PaymentDay)
				WHEN @DateType = 'YEAR' THEN DATEPART(YEAR, PaymentDay)
			END,
			CASE 
				WHEN @DateType = 'DAY' THEN PaymentDay
				ELSE '10000101'
			END,
			t.CarWashShopId,
			c.Name

			ORDER BY Calendar DESC
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
                name: "Transactions");

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
