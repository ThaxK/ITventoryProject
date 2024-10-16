using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Itventory.web.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AspNetRoles",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //        PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //        TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
            //        LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
            //        AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Categories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categories", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "OfficeSuites",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Series = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Stock = table.Column<int>(type: "int", nullable: true),
            //        StartDate = table.Column<DateTime>(type: "date", nullable: false),
            //        FinishDate = table.Column<DateTime>(type: "date", nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OfficeSuites", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "OtherPeripherals",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OtherPeripherals", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "WorkAreas",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WorkAreas", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetRoleClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserLogins",
            //    columns: table => new
            //    {
            //        LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserRoles",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserTokens",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Subcategories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CategoryId = table.Column<int>(type: "int", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Subcategories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Subcategories_Categories_CategoryId",
            //            column: x => x.CategoryId,
            //            principalTable: "Categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Employees",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        DocumentNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //        Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        WorkAreaId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Employees", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Employees_WorkAreas_WorkAreaId",
            //            column: x => x.WorkAreaId,
            //            principalTable: "WorkAreas",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Peripherals",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Model = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        Series = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        PeripheralTypeId = table.Column<int>(type: "int", nullable: false),
            //        PeripheralBrandId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Peripherals", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Peripherals_Subcategories_PeripheralBrandId",
            //            column: x => x.PeripheralBrandId,
            //            principalTable: "Subcategories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Peripherals_Subcategories_PeripheralTypeId",
            //            column: x => x.PeripheralTypeId,
            //            principalTable: "Subcategories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SoftwareLicenses",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        Series = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
            //        ProductKey = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
            //        StartDate = table.Column<DateTime>(type: "date", nullable: false),
            //        FinishDate = table.Column<DateTime>(type: "date", nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        SubcategoryId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SoftwareLicenses", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_SoftwareLicenses_Subcategories_SubcategoryId",
            //            column: x => x.SubcategoryId,
            //            principalTable: "Subcategories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "EmployeeOfficeSuite",
            //    columns: table => new
            //    {
            //        EmployeesId = table.Column<int>(type: "int", nullable: false),
            //        OfficeSuitesId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EmployeeOfficeSuite", x => new { x.EmployeesId, x.OfficeSuitesId });
            //        table.ForeignKey(
            //            name: "FK_EmployeeOfficeSuite_Employees_EmployeesId",
            //            column: x => x.EmployeesId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_EmployeeOfficeSuite_OfficeSuites_OfficeSuitesId",
            //            column: x => x.OfficeSuitesId,
            //            principalTable: "OfficeSuites",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Products",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        StartDate = table.Column<DateTime>(type: "date", nullable: false),
            //        FinishDate = table.Column<DateTime>(type: "date", nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        EmployeeId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Products", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Products_Employees_EmployeeId",
            //            column: x => x.EmployeeId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Devices",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DeviceTypeId = table.Column<int>(type: "int", nullable: false),
            //        DeviceBrandId = table.Column<int>(type: "int", nullable: false),
            //        Model = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        Series = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
            //        ProcessorId = table.Column<int>(type: "int", nullable: false),
            //        Ram = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        SolidStateDrive = table.Column<int>(type: "int", nullable: false),
            //        HardDiskDrive = table.Column<int>(type: "int", nullable: false),
            //        WindowsLicenseId = table.Column<int>(type: "int", nullable: true),
            //        AntivirusLicenseId = table.Column<int>(type: "int", nullable: true),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Devices", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Devices_SoftwareLicenses_AntivirusLicenseId",
            //            column: x => x.AntivirusLicenseId,
            //            principalTable: "SoftwareLicenses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Devices_SoftwareLicenses_WindowsLicenseId",
            //            column: x => x.WindowsLicenseId,
            //            principalTable: "SoftwareLicenses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Devices_Subcategories_DeviceBrandId",
            //            column: x => x.DeviceBrandId,
            //            principalTable: "Subcategories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Devices_Subcategories_DeviceTypeId",
            //            column: x => x.DeviceTypeId,
            //            principalTable: "Subcategories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Devices_Subcategories_ProcessorId",
            //            column: x => x.ProcessorId,
            //            principalTable: "Subcategories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "WorkStations",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Pendiente"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        EmployeeId = table.Column<int>(type: "int", nullable: false),
            //        ComputerDeviceId = table.Column<int>(type: "int", nullable: false),
            //        SmartPhoneDeviceId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WorkStations", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_WorkStations_Devices_ComputerDeviceId",
            //            column: x => x.ComputerDeviceId,
            //            principalTable: "Devices",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_WorkStations_Devices_SmartPhoneDeviceId",
            //            column: x => x.SmartPhoneDeviceId,
            //            principalTable: "Devices",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_WorkStations_Employees_EmployeeId",
            //            column: x => x.EmployeeId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Acts",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Disponible"),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        CreateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        UpdateAt = table.Column<DateTime>(type: "date", nullable: false),
            //        WorkStationId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Acts", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Acts_WorkStations_WorkStationId",
            //            column: x => x.WorkStationId,
            //            principalTable: "WorkStations",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "OtherPeripheralWorkStation",
            //    columns: table => new
            //    {
            //        OtherPeripheralsId = table.Column<int>(type: "int", nullable: false),
            //        WorkStationsId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OtherPeripheralWorkStation", x => new { x.OtherPeripheralsId, x.WorkStationsId });
            //        table.ForeignKey(
            //            name: "FK_OtherPeripheralWorkStation_OtherPeripherals_OtherPeripheralsId",
            //            column: x => x.OtherPeripheralsId,
            //            principalTable: "OtherPeripherals",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_OtherPeripheralWorkStation_WorkStations_WorkStationsId",
            //            column: x => x.WorkStationsId,
            //            principalTable: "WorkStations",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PeripheralWorkStation",
            //    columns: table => new
            //    {
            //        PeripheralsId = table.Column<int>(type: "int", nullable: false),
            //        WorkStationsId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PeripheralWorkStation", x => new { x.PeripheralsId, x.WorkStationsId });
            //        table.ForeignKey(
            //            name: "FK_PeripheralWorkStation_Peripherals_PeripheralsId",
            //            column: x => x.PeripheralsId,
            //            principalTable: "Peripherals",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_PeripheralWorkStation_WorkStations_WorkStationsId",
            //            column: x => x.WorkStationsId,
            //            principalTable: "WorkStations",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.InsertData(
            //    table: "Categories",
            //    columns: new[] { "Id", "Name" },
            //    values: new object[,]
            //    {
            //        { 1, "Periféricos" },
            //        { 2, "Licencias" },
            //        { 3, "Dispositivos" },
            //        { 4, "Marcas" },
            //        { 5, "Procesadores de PC" },
            //        { 6, "Procesadores de Telefonos" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "WorkAreas",
            //    columns: new[] { "Id", "CreateAt", "IsDeleted", "Name", "UpdateAt" },
            //    values: new object[,]
            //    {
            //        { 1, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1880), false, "Administración", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1881) },
            //        { 2, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1882), false, "Gerencia", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1883) },
            //        { 3, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1884), false, "It", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1884) },
            //        { 4, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1885), false, "Proyectos Desarrollo Corporativo", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1886) },
            //        { 5, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1886), false, "Servicios Especiales", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1887) },
            //        { 6, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1888), false, "Fleet Managers", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1888) },
            //        { 7, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1889), false, "Instalacion y Mantenimiento", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1890) },
            //        { 8, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1891), false, "Centro de Control", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1891) },
            //        { 9, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1892), false, "Ventas", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1893) }
            //    });

            //migrationBuilder.InsertData(
            //    table: "Subcategories",
            //    columns: new[] { "Id", "CategoryId", "CreateAt", "IsDeleted", "Name", "UpdateAt" },
            //    values: new object[,]
            //    {
            //        { 1, 1, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1772), false, "Mouses", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1783) },
            //        { 2, 1, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1796), false, "Teclados", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1797) },
            //        { 3, 1, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1799), false, "Monitores", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1799) },
            //        { 4, 2, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1801), false, "Licencias de Windows", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1802) },
            //        { 5, 2, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1804), false, "Licencias de Antivirus", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1804) },
            //        { 6, 3, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1818), false, "Computadores", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1818) },
            //        { 7, 3, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1820), false, "SmartPhones", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1821) },
            //        { 8, 4, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1823), false, "HP", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1823) },
            //        { 9, 4, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1825), false, "Apple", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1826) },
            //        { 10, 4, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1828), false, "Lenovo", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1829) },
            //        { 11, 5, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1830), false, "Intel Core i7-11700K (8 Núcleos, 3.6 GHz Frecuencia Base)", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1831) },
            //        { 12, 5, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1833), false, "AMD Ryzen 7 5800X (8 núcleos, 3.8 GHz Frecuencia Base)", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1833) },
            //        { 13, 6, new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1835), false, "Qualcomm Snapdragon", new DateTime(2023, 12, 19, 17, 11, 22, 829, DateTimeKind.Local).AddTicks(1836) }
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Acts_WorkStationId",
            //    table: "Acts",
            //    column: "WorkStationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetRoleClaims_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "RoleNameIndex",
            //    table: "AspNetRoles",
            //    column: "NormalizedName",
            //    unique: true,
            //    filter: "[NormalizedName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserClaims_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserLogins_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "EmailIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedEmail");

            //migrationBuilder.CreateIndex(
            //    name: "UserNameIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedUserName",
            //    unique: true,
            //    filter: "[NormalizedUserName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Devices_AntivirusLicenseId",
            //    table: "Devices",
            //    column: "AntivirusLicenseId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Devices_DeviceBrandId",
            //    table: "Devices",
            //    column: "DeviceBrandId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Devices_DeviceTypeId",
            //    table: "Devices",
            //    column: "DeviceTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Devices_ProcessorId",
            //    table: "Devices",
            //    column: "ProcessorId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Devices_WindowsLicenseId",
            //    table: "Devices",
            //    column: "WindowsLicenseId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EmployeeOfficeSuite_OfficeSuitesId",
            //    table: "EmployeeOfficeSuite",
            //    column: "OfficeSuitesId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employees_WorkAreaId",
            //    table: "Employees",
            //    column: "WorkAreaId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OtherPeripheralWorkStation_WorkStationsId",
            //    table: "OtherPeripheralWorkStation",
            //    column: "WorkStationsId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Peripherals_PeripheralBrandId",
            //    table: "Peripherals",
            //    column: "PeripheralBrandId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Peripherals_PeripheralTypeId",
            //    table: "Peripherals",
            //    column: "PeripheralTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PeripheralWorkStation_WorkStationsId",
            //    table: "PeripheralWorkStation",
            //    column: "WorkStationsId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Products_EmployeeId",
            //    table: "Products",
            //    column: "EmployeeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SoftwareLicenses_SubcategoryId",
            //    table: "SoftwareLicenses",
            //    column: "SubcategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Subcategories_CategoryId",
            //    table: "Subcategories",
            //    column: "CategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_WorkStations_ComputerDeviceId",
            //    table: "WorkStations",
            //    column: "ComputerDeviceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_WorkStations_EmployeeId",
            //    table: "WorkStations",
            //    column: "EmployeeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_WorkStations_SmartPhoneDeviceId",
            //    table: "WorkStations",
            //    column: "SmartPhoneDeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Acts");

            //migrationBuilder.DropTable(
            //    name: "AspNetRoleClaims");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserClaims");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserLogins");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserRoles");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserTokens");

            //migrationBuilder.DropTable(
            //    name: "EmployeeOfficeSuite");

            //migrationBuilder.DropTable(
            //    name: "OtherPeripheralWorkStation");

            //migrationBuilder.DropTable(
            //    name: "PeripheralWorkStation");

            //migrationBuilder.DropTable(
            //    name: "Products");

            //migrationBuilder.DropTable(
            //    name: "AspNetRoles");

            //migrationBuilder.DropTable(
            //    name: "AspNetUsers");

            //migrationBuilder.DropTable(
            //    name: "OfficeSuites");

            //migrationBuilder.DropTable(
            //    name: "OtherPeripherals");

            //migrationBuilder.DropTable(
            //    name: "Peripherals");

            //migrationBuilder.DropTable(
            //    name: "WorkStations");

            //migrationBuilder.DropTable(
            //    name: "Devices");

            //migrationBuilder.DropTable(
            //    name: "Employees");

            //migrationBuilder.DropTable(
            //    name: "SoftwareLicenses");

            //migrationBuilder.DropTable(
            //    name: "WorkAreas");

            //migrationBuilder.DropTable(
            //    name: "Subcategories");

            //migrationBuilder.DropTable(
            //    name: "Categories");
        }
    }
}
