using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrismWorkApp.Services.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bldResourseCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bldResourseCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OGRN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contacts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleCode = table.Column<int>(type: "int", nullable: false),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fathername = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   // ImageFile = table.Column<byte[]>(type: "varbinary(max) ", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });
            migrationBuilder.Sql("ALTER table [dbo].[Pictures] add [ImageFile] varbinary(max) FILESTREAM not null");
            migrationBuilder.Sql("ALTER table [dbo].[Pictures] add constraint [DF_Pictures_ImageFile] default(0x) [ImageFile]");

            migrationBuilder.CreateTable(
                name: "ResponsibleEmployeeRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleCode = table.Column<int>(type: "int", nullable: false),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsibleEmployeeRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Levels = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Axes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConstructionCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SROIssuingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConstructionCompanies_Companies_Id",
                        column: x => x.Id,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConstructionCompanies_Companies_SROIssuingCompanyId",
                        column: x => x.SROIssuingCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeePositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "EmployeePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_People_Id",
                        column: x => x.Id,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Laboriousness = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScopeOfWork = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConstructionCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_ConstructionCompanies_ConstructionCompanyId",
                        column: x => x.ConstructionCompanyId,
                        principalTable: "ConstructionCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participants_ParticipantRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ParticipantRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participants_Projects_bldProjectId",
                        column: x => x.bldProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResponsibleEmployees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NRSId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocConfirmingTheAthorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    bldParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldAOSRDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldConstructionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldWorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsibleEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponsibleEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResponsibleEmployees_Participants_bldParticipantId",
                        column: x => x.bldParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResponsibleEmployees_Projects_bldProjectId",
                        column: x => x.bldProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResponsibleEmployees_ResponsibleEmployeeRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ResponsibleEmployeeRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AOSRDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    bldWorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AOSRDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExecutiveSchemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bldWorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutiveSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaboratoryReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LaboratoryReportPeeoperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bldWorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeometryParameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ControlingParament = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegulationDocumentsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialCertificates_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasportDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasportDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bldProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Projects_bldProjectId",
                        column: x => x.bldProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Laboriousness = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScopeOfWork = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bldProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldProjectDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objects_Objects_bldObjectId",
                        column: x => x.bldObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Objects_ProjectDocuments_bldProjectDocumentId",
                        column: x => x.bldProjectDocumentId,
                        principalTable: "ProjectDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Objects_Projects_bldProjectId",
                        column: x => x.bldProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Objects_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bldObjectbldParticipant",
                columns: table => new
                {
                    BuildingObjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bldObjectbldParticipant", x => new { x.BuildingObjectsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_bldObjectbldParticipant_Objects_BuildingObjectsId",
                        column: x => x.BuildingObjectsId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bldObjectbldParticipant_Participants_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Constructions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Laboriousness = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScopeOfWork = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    bldObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldConstructionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldProjectDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Constructions_Constructions_bldConstructionId",
                        column: x => x.bldConstructionId,
                        principalTable: "Constructions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Constructions_Objects_bldObjectId",
                        column: x => x.bldObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Constructions_ProjectDocuments_bldProjectDocumentId",
                        column: x => x.bldProjectDocumentId,
                        principalTable: "ProjectDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Constructions_Projects_bldProjectId",
                        column: x => x.bldProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Constructions_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bldConstructionbldParticipant",
                columns: table => new
                {
                    ConstructionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bldConstructionbldParticipant", x => new { x.ConstructionsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_bldConstructionbldParticipant_Constructions_ConstructionsId",
                        column: x => x.ConstructionsId,
                        principalTable: "Constructions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bldConstructionbldParticipant_Participants_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Laboriousness = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScopeOfWork = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    bldConstructionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_Constructions_bldConstructionId",
                        column: x => x.bldConstructionId,
                        principalTable: "Constructions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Works_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Works_WorkAreas_WorkAreaId",
                        column: x => x.WorkAreaId,
                        principalTable: "WorkAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bldParticipantbldWork",
                columns: table => new
                {
                    ParticipantsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bldParticipantbldWork", x => new { x.ParticipantsId, x.WorksId });
                    table.ForeignKey(
                        name: "FK_bldParticipantbldWork_Participants_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bldParticipantbldWork_Works_WorksId",
                        column: x => x.WorksId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bldProjectDocumentbldWork",
                columns: table => new
                {
                    ProjectDocumentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bldWorksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bldProjectDocumentbldWork", x => new { x.ProjectDocumentsId, x.bldWorksId });
                    table.ForeignKey(
                        name: "FK_bldProjectDocumentbldWork_ProjectDocuments_ProjectDocumentsId",
                        column: x => x.ProjectDocumentsId,
                        principalTable: "ProjectDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bldProjectDocumentbldWork_Works_bldWorksId",
                        column: x => x.bldWorksId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bldResource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    bldWorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bldResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bldResource_bldResourseCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "bldResourseCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bldResource_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bldResource_Works_bldWorkId",
                        column: x => x.bldWorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreToNexWorksTable",
                columns: table => new
                {
                    NextWorksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousWorksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreToNexWorksTable", x => new { x.NextWorksId, x.PreviousWorksId });
                    table.ForeignKey(
                        name: "FK_PreToNexWorksTable_Works_NextWorksId",
                        column: x => x.NextWorksId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreToNexWorksTable_Works_PreviousWorksId",
                        column: x => x.PreviousWorksId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bldDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PagesNumber = table.Column<int>(type: "int", nullable: false),
                    RegId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldConstructionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bldProjectId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bldDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bldDocument_bldDocument_bldDocumentId",
                        column: x => x.bldDocumentId,
                        principalTable: "bldDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bldDocument_bldResource_bldMaterialId",
                        column: x => x.bldMaterialId,
                        principalTable: "bldResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bldDocument_Constructions_bldConstructionId",
                        column: x => x.bldConstructionId,
                        principalTable: "Constructions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bldDocument_Objects_bldObjectId",
                        column: x => x.bldObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bldDocument_Pictures_ImageFileId",
                        column: x => x.ImageFileId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bldDocument_Projects_bldProjectId1",
                        column: x => x.bldProjectId1,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegulationtDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulationtDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegulationtDocuments_bldDocument_Id",
                        column: x => x.Id,
                        principalTable: "bldDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bldRegulationtDocumentbldWork",
                columns: table => new
                {
                    RegulationDocumentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bldWorksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bldRegulationtDocumentbldWork", x => new { x.RegulationDocumentsId, x.bldWorksId });
                    table.ForeignKey(
                        name: "FK_bldRegulationtDocumentbldWork_RegulationtDocuments_RegulationDocumentsId",
                        column: x => x.RegulationDocumentsId,
                        principalTable: "RegulationtDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bldRegulationtDocumentbldWork_Works_bldWorksId",
                        column: x => x.bldWorksId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AOSRDocuments_bldWorkId",
                table: "AOSRDocuments",
                column: "bldWorkId",
                unique: true,
                filter: "[bldWorkId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_bldConstructionbldParticipant_ParticipantsId",
                table: "bldConstructionbldParticipant",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_bldDocument_bldConstructionId",
                table: "bldDocument",
                column: "bldConstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_bldDocument_bldDocumentId",
                table: "bldDocument",
                column: "bldDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_bldDocument_bldMaterialId",
                table: "bldDocument",
                column: "bldMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_bldDocument_bldObjectId",
                table: "bldDocument",
                column: "bldObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_bldDocument_bldProjectId1",
                table: "bldDocument",
                column: "bldProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_bldDocument_ImageFileId",
                table: "bldDocument",
                column: "ImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_bldObjectbldParticipant_ParticipantsId",
                table: "bldObjectbldParticipant",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_bldParticipantbldWork_WorksId",
                table: "bldParticipantbldWork",
                column: "WorksId");

            migrationBuilder.CreateIndex(
                name: "IX_bldProjectDocumentbldWork_bldWorksId",
                table: "bldProjectDocumentbldWork",
                column: "bldWorksId");

            migrationBuilder.CreateIndex(
                name: "IX_bldRegulationtDocumentbldWork_bldWorksId",
                table: "bldRegulationtDocumentbldWork",
                column: "bldWorksId");

            migrationBuilder.CreateIndex(
                name: "IX_bldResource_bldWorkId",
                table: "bldResource",
                column: "bldWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_bldResource_CategoryId",
                table: "bldResource",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_bldResource_UnitOfMeasurementId",
                table: "bldResource",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionCompanies_SROIssuingCompanyId",
                table: "ConstructionCompanies",
                column: "SROIssuingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Constructions_bldConstructionId",
                table: "Constructions",
                column: "bldConstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_Constructions_bldObjectId",
                table: "Constructions",
                column: "bldObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Constructions_bldProjectDocumentId",
                table: "Constructions",
                column: "bldProjectDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Constructions_bldProjectId",
                table: "Constructions",
                column: "bldProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Constructions_UnitOfMeasurementId",
                table: "Constructions",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutiveSchemes_bldWorkId",
                table: "ExecutiveSchemes",
                column: "bldWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryReports_bldWorkId",
                table: "LaboratoryReports",
                column: "bldWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCertificates_UnitOfMeasurementId",
                table: "MaterialCertificates",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_bldObjectId",
                table: "Objects",
                column: "bldObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_bldProjectDocumentId",
                table: "Objects",
                column: "bldProjectDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_bldProjectId",
                table: "Objects",
                column: "bldProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_UnitOfMeasurementId",
                table: "Objects",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_bldProjectId",
                table: "Participants",
                column: "bldProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ConstructionCompanyId",
                table: "Participants",
                column: "ConstructionCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_RoleId",
                table: "Participants",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreToNexWorksTable_PreviousWorksId",
                table: "PreToNexWorksTable",
                column: "PreviousWorksId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_bldProjectId",
                table: "ProjectDocuments",
                column: "bldProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UnitOfMeasurementId",
                table: "Projects",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_bldAOSRDocumentId",
                table: "ResponsibleEmployees",
                column: "bldAOSRDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_bldConstructionId",
                table: "ResponsibleEmployees",
                column: "bldConstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_bldObjectId",
                table: "ResponsibleEmployees",
                column: "bldObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_bldParticipantId",
                table: "ResponsibleEmployees",
                column: "bldParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_bldProjectId",
                table: "ResponsibleEmployees",
                column: "bldProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_bldWorkId",
                table: "ResponsibleEmployees",
                column: "bldWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_DocConfirmingTheAthorityId",
                table: "ResponsibleEmployees",
                column: "DocConfirmingTheAthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_EmployeeId",
                table: "ResponsibleEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleEmployees_RoleId",
                table: "ResponsibleEmployees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_bldConstructionId",
                table: "Works",
                column: "bldConstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_UnitOfMeasurementId",
                table: "Works",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_WorkAreaId",
                table: "Works",
                column: "WorkAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibleEmployees_AOSRDocuments_bldAOSRDocumentId",
                table: "ResponsibleEmployees",
                column: "bldAOSRDocumentId",
                principalTable: "AOSRDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibleEmployees_bldDocument_DocConfirmingTheAthorityId",
                table: "ResponsibleEmployees",
                column: "DocConfirmingTheAthorityId",
                principalTable: "bldDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibleEmployees_Constructions_bldConstructionId",
                table: "ResponsibleEmployees",
                column: "bldConstructionId",
                principalTable: "Constructions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibleEmployees_Objects_bldObjectId",
                table: "ResponsibleEmployees",
                column: "bldObjectId",
                principalTable: "Objects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibleEmployees_Works_bldWorkId",
                table: "ResponsibleEmployees",
                column: "bldWorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AOSRDocuments_bldDocument_Id",
                table: "AOSRDocuments",
                column: "Id",
                principalTable: "bldDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AOSRDocuments_Works_bldWorkId",
                table: "AOSRDocuments",
                column: "bldWorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutiveSchemes_bldDocument_Id",
                table: "ExecutiveSchemes",
                column: "Id",
                principalTable: "bldDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutiveSchemes_Works_bldWorkId",
                table: "ExecutiveSchemes",
                column: "bldWorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryReports_bldDocument_Id",
                table: "LaboratoryReports",
                column: "Id",
                principalTable: "bldDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryReports_Works_bldWorkId",
                table: "LaboratoryReports",
                column: "bldWorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialCertificates_bldDocument_Id",
                table: "MaterialCertificates",
                column: "Id",
                principalTable: "bldDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PasportDocuments_bldDocument_Id",
                table: "PasportDocuments",
                column: "Id",
                principalTable: "bldDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDocuments_bldDocument_Id",
                table: "ProjectDocuments",
                column: "Id",
                principalTable: "bldDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDocuments_bldDocument_Id",
                table: "ProjectDocuments");

            migrationBuilder.DropTable(
                name: "bldConstructionbldParticipant");

            migrationBuilder.DropTable(
                name: "bldObjectbldParticipant");

            migrationBuilder.DropTable(
                name: "bldParticipantbldWork");

            migrationBuilder.DropTable(
                name: "bldProjectDocumentbldWork");

            migrationBuilder.DropTable(
                name: "bldRegulationtDocumentbldWork");

            migrationBuilder.DropTable(
                name: "ExecutiveSchemes");

            migrationBuilder.DropTable(
                name: "LaboratoryReports");

            migrationBuilder.DropTable(
                name: "MaterialCertificates");

            migrationBuilder.DropTable(
                name: "PasportDocuments");

            migrationBuilder.DropTable(
                name: "PreToNexWorksTable");

            migrationBuilder.DropTable(
                name: "ResponsibleEmployees");

            migrationBuilder.DropTable(
                name: "RegulationtDocuments");

            migrationBuilder.DropTable(
                name: "AOSRDocuments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "ResponsibleEmployeeRoles");

            migrationBuilder.DropTable(
                name: "EmployeePositions");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "ConstructionCompanies");

            migrationBuilder.DropTable(
                name: "ParticipantRoles");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "bldDocument");

            migrationBuilder.DropTable(
                name: "bldResource");

            migrationBuilder.DropTable(
                name: "Pictures");
         
            migrationBuilder.Sql("ALTER table [dbo].[Pictures] drop constraint [DF_Pictures_ImageFile]");
            migrationBuilder.Sql("ALTER table [dbo].[Pictures] drop [ImageFile] ");

            migrationBuilder.DropTable(
                name: "bldResourseCategory");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Constructions");

            migrationBuilder.DropTable(
                name: "WorkAreas");

            migrationBuilder.DropTable(
                name: "Objects");

            migrationBuilder.DropTable(
                name: "ProjectDocuments");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurements");
        }
    }
}
