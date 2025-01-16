using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echeckdem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACTS",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    LNAME = table.Column<string>(type: "char(150)", unicode: false, fixedLength: true, maxLength: 150, nullable: true),
                    SNAME = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    YEAR = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    OTHERLEG = table.Column<string>(type: "char(250)", unicode: false, fixedLength: true, maxLength: 250, nullable: true),
                    OBJECTIVES = table.Column<string>(type: "varchar(3000)", unicode: false, maxLength: 3000, nullable: true),
                    APPLICABILITY = table.Column<string>(type: "varchar(3000)", unicode: false, maxLength: 3000, nullable: true),
                    ATYPE = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    STCODE = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    LASTAMEND = table.Column<string>(type: "char(200)", unicode: false, fixedLength: true, maxLength: 200, nullable: true),
                    ALEGTYPE = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    PARLEG = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    AACTIVE = table.Column<int>(type: "int", nullable: true),
                    LEGTYPE = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    APPL = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    NODOC = table.Column<int>(type: "int", nullable: true),
                    NOCHK = table.Column<int>(type: "int", nullable: true),
                    FILENAME = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACTS", x => x.CODE)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ALINKS",
                columns: table => new
                {
                    ACODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    LCODE = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    MODULE = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AUDTRAIL",
                columns: table => new
                {
                    TINDEX = table.Column<int>(type: "int", nullable: false),
                    UIDS = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    ORIGIN = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    ACTIVITY = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    TTABLE = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    DETAILS = table.Column<string>(type: "char(200)", unicode: false, fixedLength: true, maxLength: 200, nullable: true),
                    TDATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    TTIME = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDTRAIL", x => x.TINDEX)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "BOCW_SCOPE",
                columns: table => new
                {
                    ScopeID = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ScopeName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    ScopeActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOCW_SCOPE", x => x.ScopeID);
                });

            migrationBuilder.CreateTable(
                name: "CALENDAR",
                columns: table => new
                {
                    OBLIG = table.Column<decimal>(type: "decimal(6,0)", nullable: false),
                    OBLDATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    WARNDATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    REMARKS = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALENDAR", x => new { x.OBLIG, x.OBLDATE })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "CONTACT",
                columns: table => new
                {
                    USERID = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: false),
                    CCODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    FNAME = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    LNAME = table.Column<string>(type: "char(25)", unicode: false, fixedLength: true, maxLength: 25, nullable: true),
                    ORGCODE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    LOCATION = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    PHONE = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: true),
                    FAX = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    EMAILID = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    ALTEMAIL = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    DESIG = table.Column<string>(type: "char(75)", unicode: false, fixedLength: true, maxLength: 75, nullable: true),
                    PASS = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    USERLEVEL = table.Column<decimal>(type: "decimal(5,0)", nullable: true),
                    UACTIVE = table.Column<decimal>(type: "decimal(2,0)", nullable: true),
                    LASTPASS = table.Column<DateTime>(type: "datetime", nullable: true),
                    STCODE = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    FULLNAME = table.Column<string>(type: "char(45)", unicode: false, fixedLength: true, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTACT", x => x.USERID)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "CUSERS",
                columns: table => new
                {
                    userid = table.Column<string>(type: "char(60)", unicode: false, fixedLength: true, maxLength: 60, nullable: false),
                    uno = table.Column<int>(type: "int", nullable: false),
                    uname = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    uorg = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    utype = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    ulevel = table.Column<int>(type: "int", nullable: true),
                    ustates = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    uactive = table.Column<int>(type: "int", nullable: true),
                    defstate = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    crdate = table.Column<DateOnly>(type: "date", nullable: true),
                    expdate = table.Column<DateOnly>(type: "date", nullable: true),
                    mail = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    password = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSERS", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "CUSERS2",
                columns: table => new
                {
                    uno = table.Column<int>(type: "int", nullable: false),
                    utype = table.Column<int>(type: "int", nullable: true),
                    utype1 = table.Column<int>(type: "int", nullable: true),
                    ucrdate = table.Column<DateOnly>(type: "date", nullable: true),
                    uexdate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DOCLINKS",
                columns: table => new
                {
                    OCODE = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    DCODE = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCLINKS", x => new { x.OCODE, x.DCODE })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "DOCLIST",
                columns: table => new
                {
                    DCODE = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    ACODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    DTITLE = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    DDESC = table.Column<string>(type: "char(150)", unicode: false, fixedLength: true, maxLength: 150, nullable: true),
                    DOCNAME = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    DIRNAME = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    DTYPE = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    DCAT = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    OACODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    DINDEX = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCLIST", x => x.DCODE)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "DORG",
                columns: table => new
                {
                    ORGCODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    ORGDESC = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true),
                    LSTARTDATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    LENDDATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    ACTIVE = table.Column<int>(type: "int", nullable: true),
                    STATES = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "echkTrail",
                columns: table => new
                {
                    TINDEX = table.Column<int>(type: "int", nullable: false),
                    UIDS = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    ORIGIN = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    ACTIVITY = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    TTABLE = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    DETAILS = table.Column<string>(type: "char(200)", unicode: false, fixedLength: true, maxLength: 200, nullable: true),
                    TDATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    TTIME = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_echkTrail", x => x.TINDEX)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "emaillist",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emaillist", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "ENQUIRY_table",
                columns: table => new
                {
                    ENID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ename = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Edesignation = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Eorganization = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Eemail = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Eintrest = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Ereference = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Edate = table.Column<DateOnly>(type: "date", nullable: false),
                    Emessage = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENQUIRY_table", x => x.ENID);
                });

            migrationBuilder.CreateTable(
                name: "FTLINK",
                columns: table => new
                {
                    SECCODE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    MODULE = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    LCODE = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FTLINK", x => new { x.SECCODE, x.MODULE, x.LCODE })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "FTLINKS",
                columns: table => new
                {
                    OCODE = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    SECCODE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FTLINKS", x => new { x.OCODE, x.SECCODE })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "FULLTEXT",
                columns: table => new
                {
                    SECCODE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ACT = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    CHAPTER = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    SECTION = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    TITLE = table.Column<string>(type: "char(120)", unicode: false, fixedLength: true, maxLength: 120, nullable: true),
                    FTEXT = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    SINDEX = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FULLTEXT", x => x.SECCODE)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "LLAUTH",
                columns: table => new
                {
                    AUCODE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ACODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    AUTH = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    DUTIES = table.Column<string>(type: "char(255)", unicode: false, fixedLength: true, maxLength: 255, nullable: true),
                    POWERS = table.Column<string>(type: "char(255)", unicode: false, fixedLength: true, maxLength: 255, nullable: true),
                    JURISDICTION = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    SECTION = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LLAUTH", x => x.AUCODE);
                });

            migrationBuilder.CreateTable(
                name: "LLDEF",
                columns: table => new
                {
                    SECCODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ACODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SECTION = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    TITLE = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    DTEXT = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LLDEF", x => x.SECCODE);
                });

            migrationBuilder.CreateTable(
                name: "LLFAQ",
                columns: table => new
                {
                    QCODE = table.Column<int>(type: "int", nullable: false),
                    QUES = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    QANSWER = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CAT1 = table.Column<string>(type: "char(25)", unicode: false, fixedLength: true, maxLength: 25, nullable: true),
                    CAT2 = table.Column<string>(type: "char(25)", unicode: false, fixedLength: true, maxLength: 25, nullable: true),
                    QACTIVE = table.Column<int>(type: "int", nullable: true),
                    CRDATE = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LLINKS",
                columns: table => new
                {
                    LID = table.Column<int>(type: "int", nullable: false),
                    LCAT = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    LTITLE = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    LDESC = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    LURL = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    LACTIVE = table.Column<int>(type: "int", nullable: true),
                    CRDATE = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LLOFF",
                columns: table => new
                {
                    OFFCODE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    ACODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    OFFENCE = table.Column<string>(type: "char(255)", unicode: false, fixedLength: true, maxLength: 255, nullable: true),
                    PENALTY = table.Column<string>(type: "char(255)", unicode: false, fixedLength: true, maxLength: 255, nullable: true),
                    SECTION = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: true),
                    MAXFINE = table.Column<int>(type: "int", nullable: true),
                    MAXPRISON = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LLOFF", x => x.OFFCODE);
                });

            migrationBuilder.CreateTable(
                name: "LOcation",
                columns: table => new
                {
                    location = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    code = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MASTREG",
                columns: table => new
                {
                    RTYPE = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    RDESC = table.Column<string>(type: "char(40)", unicode: false, fixedLength: true, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MASTREG", x => x.RTYPE);
                });

            migrationBuilder.CreateTable(
                name: "MASTSTATES",
                columns: table => new
                {
                    STATEID = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    STATEDESC = table.Column<string>(type: "char(25)", unicode: false, fixedLength: true, maxLength: 25, nullable: true),
                    STACTIVE = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MWCAT",
                columns: table => new
                {
                    stid = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    catid = table.Column<int>(type: "int", nullable: false),
                    catgrp = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    catname = table.Column<string>(type: "char(75)", unicode: false, fixedLength: true, maxLength: 75, nullable: true),
                    catdesc = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    catlnk = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWCAT_1", x => new { x.stid, x.catid });
                });

            migrationBuilder.CreateTable(
                name: "MWDATA",
                columns: table => new
                {
                    STID = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    catid = table.Column<int>(type: "int", nullable: false),
                    stdate = table.Column<DateOnly>(type: "date", nullable: false),
                    endate = table.Column<DateOnly>(type: "date", nullable: true),
                    basic = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    da = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    monthly = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    daily = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z2b = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z2da = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z2m = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z2d = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z3b = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z3da = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z3m = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z3d = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    notdate = table.Column<DateOnly>(type: "date", nullable: true),
                    notfile = table.Column<string>(type: "char(80)", unicode: false, fixedLength: true, maxLength: 80, nullable: true),
                    hra = table.Column<double>(type: "float", nullable: true),
                    spallowence = table.Column<double>(type: "float", nullable: true),
                    z2hra = table.Column<double>(type: "float", nullable: true),
                    z2spallowence = table.Column<double>(type: "float", nullable: true),
                    z3hra = table.Column<double>(type: "float", nullable: true),
                    z3spallowence = table.Column<double>(type: "float", nullable: true),
                    z4b = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z4da = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z4hra = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z4spallowence = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z4m = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z4d = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    other = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z2other = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z3other = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    z4other = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWDATA", x => new { x.STID, x.catid, x.stdate });
                });

            migrationBuilder.CreateTable(
                name: "MWNOTES",
                columns: table => new
                {
                    STID = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    CATGRP = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    TITLE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TP1 = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    TP2 = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    Z1N = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Z2N = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Z3N = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    STNOTES = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    STCATS = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Z4N = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWNOTES", x => new { x.STID, x.CATGRP });
                });

            migrationBuilder.CreateTable(
                name: "NCACTAKEN",
                columns: table => new
                {
                    ACTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACID = table.Column<int>(type: "int", nullable: false),
                    ACDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ACTAKEN = table.Column<string>(type: "text", nullable: true),
                    NACDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    UNO = table.Column<int>(type: "int", nullable: true),
                    ACCRDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    SHOWCLIENT = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCACTION",
                columns: table => new
                {
                    ACID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OID = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ACLINK = table.Column<int>(type: "int", nullable: true),
                    LCODE = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    ACTP = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    ACTITLE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ACDETAIL = table.Column<string>(type: "text", nullable: true),
                    ACSHOW = table.Column<int>(type: "int", nullable: true),
                    ACSTATUS = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    ACISTATUS = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    ACIDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ATARGDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ADOCDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ACCLDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ACREMARKS = table.Column<string>(type: "text", nullable: true),
                    ACRUSER = table.Column<int>(type: "int", nullable: true),
                    ACRDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    SBTP = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    tpp = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCAUDET",
                columns: table => new
                {
                    AID = table.Column<int>(type: "int", nullable: false),
                    CIID = table.Column<int>(type: "int", nullable: false),
                    ACHK = table.Column<int>(type: "int", nullable: true),
                    ARMK = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCAUDMAST",
                columns: table => new
                {
                    AID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OID = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    LCODE = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    APERIOD = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    AYEAR = table.Column<int>(type: "int", nullable: true),
                    ADATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ABY = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    AUSER = table.Column<int>(type: "int", nullable: true),
                    ASCHDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ATIME = table.Column<int>(type: "int", nullable: true),
                    AREPDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ACLDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    ACLOSED = table.Column<int>(type: "int", nullable: true),
                    ACOMPLETE = table.Column<double>(type: "float", nullable: true),
                    ASCORE = table.Column<double>(type: "float", nullable: true),
                    NOOFEMPS = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    AUDREP = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ADREMARKS = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCAUMAP",
                columns: table => new
                {
                    ACID = table.Column<int>(type: "int", nullable: false),
                    UNO = table.Column<int>(type: "int", nullable: false),
                    UAUTH = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCCONTR",
                columns: table => new
                {
                    CONTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OID = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    LCODE = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    TP = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    PERIOD = table.Column<int>(type: "int", nullable: false),
                    CYEAR = table.Column<int>(type: "int", nullable: false),
                    FREQ = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    LD = table.Column<int>(type: "int", nullable: true),
                    LASTDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    AMOUNT = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CHQDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    CHQNO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DEPDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    FILEUP = table.Column<int>(type: "int", nullable: true),
                    UPLOADDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    FILENAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    REMARKS = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCFILES",
                columns: table => new
                {
                    FID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OID = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    FTP = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    FLINK = table.Column<int>(type: "int", nullable: true),
                    FNAME = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FUPDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    FTITLE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    FARC = table.Column<int>(type: "int", nullable: true),
                    LFILE = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCFIN",
                columns: table => new
                {
                    RTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OID = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    LCODE = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    RYEAR = table.Column<int>(type: "int", nullable: false),
                    RCODE = table.Column<int>(type: "int", nullable: false),
                    LASTDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    DEPDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    penalty_delay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    reason_delay = table.Column<string>(type: "text", nullable: true),
                    FILEUP = table.Column<int>(type: "int", nullable: true),
                    UPLOADDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    FILENAME = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    REMARKS = table.Column<string>(type: "text", nullable: true),
                    tag_usr = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    exc_usr = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCLOCMAP",
                columns: table => new
                {
                    oid = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    lcode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    oblig = table.Column<int>(type: "int", nullable: false),
                    acode = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCMLOC",
                columns: table => new
                {
                    lcode = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    oid = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    lname = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    lstate = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    lcity = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    lregion = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    iscentral = table.Column<int>(type: "int", nullable: true),
                    doi = table.Column<DateOnly>(type: "date", nullable: true),
                    laddress = table.Column<string>(type: "text", nullable: true),
                    lcontact = table.Column<string>(type: "text", nullable: true),
                    lconno = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    lconemail = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    cemail = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    iemail = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    lgroup2 = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    lgroup3 = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    stdate = table.Column<DateOnly>(type: "date", nullable: true),
                    enddate = table.Column<DateOnly>(type: "date", nullable: true),
                    lactive = table.Column<int>(type: "int", nullable: true),
                    lsetup = table.Column<int>(type: "int", nullable: true),
                    Fin_esclatn_cnt = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Fin_esclatn_mail = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Fin_resp_prsn = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Fin_resp_email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ltype = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    iscloc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ncmloc_key", x => x.lcode);
                });

            migrationBuilder.CreateTable(
                name: "NCMORG",
                columns: table => new
                {
                    oid = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Spoc_eml = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Spoc = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    oname = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    oactive = table.Column<int>(type: "int", nullable: true),
                    otype = table.Column<int>(type: "int", nullable: true),
                    styear = table.Column<int>(type: "int", nullable: true),
                    Echeck = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Edoc = table.Column<bool>(type: "bit", nullable: false),
                    Vcheck = table.Column<bool>(type: "bit", nullable: false),
                    Client = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Leed = table.Column<bool>(type: "bit", nullable: false),
                    Contname = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Contemail = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NCMORG", x => x.oid);
                });

            migrationBuilder.CreateTable(
                name: "NCREG",
                columns: table => new
                {
                    UID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OID = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    LCODE = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    TP = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    STATUS = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    RNO = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DOI = table.Column<DateOnly>(type: "date", nullable: true),
                    DOE = table.Column<DateOnly>(type: "date", nullable: true),
                    NOE = table.Column<int>(type: "int", nullable: true),
                    NMOE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DOLR = table.Column<DateOnly>(type: "date", nullable: true),
                    RENHIST = table.Column<string>(type: "text", nullable: true),
                    FILENAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    REMARKS = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCRET",
                columns: table => new
                {
                    RTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OID = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    LCODE = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    RYEAR = table.Column<int>(type: "int", nullable: false),
                    RCODE = table.Column<int>(type: "int", nullable: false),
                    LASTDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    DEPDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    FILEUP = table.Column<int>(type: "int", nullable: true),
                    UPLOADDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    FILENAME = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    REMARKS = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCTEMPCNT",
                columns: table => new
                {
                    CID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CSTATE = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    TP = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    FREQ = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    PERIOD = table.Column<int>(type: "int", nullable: false),
                    LD = table.Column<int>(type: "int", nullable: false),
                    MOFFSET = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCTEMPFIN",
                columns: table => new
                {
                    RCODE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RSTATE = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    RTYPE = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    RD = table.Column<int>(type: "int", nullable: true),
                    RM = table.Column<int>(type: "int", nullable: true),
                    YROFF = table.Column<int>(type: "int", nullable: true),
                    RTITLE = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    RFORM = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: true),
                    RDESC = table.Column<string>(type: "text", nullable: true),
                    ROBLIG = table.Column<int>(type: "int", nullable: true),
                    RACT = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    triggr = table.Column<int>(type: "int", nullable: true, defaultValue: -5),
                    frequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCTEMPRET",
                columns: table => new
                {
                    RCODE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RSTATE = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    RTYPE = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    RD = table.Column<int>(type: "int", nullable: true),
                    RM = table.Column<int>(type: "int", nullable: true),
                    YROFF = table.Column<int>(type: "int", nullable: true),
                    RTITLE = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    RFORM = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: true),
                    RDESC = table.Column<string>(type: "text", nullable: true),
                    ROBLIG = table.Column<int>(type: "int", nullable: true),
                    RACT = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: true),
                    RACTIVE = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCUMAP",
                columns: table => new
                {
                    uno = table.Column<int>(type: "int", nullable: false),
                    oid = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    lcode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ulevel = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCUSER",
                columns: table => new
                {
                    USERID = table.Column<string>(type: "char(200)", unicode: false, fixedLength: true, maxLength: 200, nullable: false),
                    UNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OID = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    UNAME = table.Column<string>(type: "char(40)", unicode: false, fixedLength: true, maxLength: 40, nullable: true),
                    PASSWORD = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    USERLEVEL = table.Column<int>(type: "int", nullable: true),
                    UACTIVE = table.Column<int>(type: "int", nullable: true),
                    STCODE = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    EMAILID = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NCUSER", x => x.USERID);
                });

            migrationBuilder.CreateTable(
                name: "NOTI",
                columns: table => new
                {
                    DCODE = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    ACODE = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    DTITLE = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: true),
                    DDATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    SECTION = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DDESC = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DETAIL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FOLDER = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FILENAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CRDATE = table.Column<DateOnly>(type: "date", nullable: true),
                    NACTIVE = table.Column<int>(type: "int", nullable: true),
                    state = table.Column<string>(type: "char(25)", unicode: false, fixedLength: true, maxLength: 25, nullable: true),
                    wefdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    applictn = table.Column<string>(type: "text", nullable: true),
                    exempt = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OBLIGATI",
                columns: table => new
                {
                    CODE = table.Column<int>(type: "int", nullable: false),
                    ACT = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    SECTION = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    TITLE = table.Column<string>(type: "char(60)", unicode: false, fixedLength: true, maxLength: 60, nullable: true),
                    OBLIGE = table.Column<string>(type: "char(250)", unicode: false, fixedLength: true, maxLength: 250, nullable: true),
                    TYPE = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: true),
                    ACTION = table.Column<string>(type: "varchar(3000)", unicode: false, maxLength: 3000, nullable: true),
                    TIMING = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    AUDITS = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    SUPPORT = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    DSOURCE = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    PARENTCODE = table.Column<int>(type: "int", nullable: true),
                    OBLINDEX = table.Column<int>(type: "int", nullable: true),
                    RISKRAT = table.Column<int>(type: "int", nullable: true),
                    OACTIVE = table.Column<int>(type: "int", nullable: true, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBLIGATI", x => x.CODE)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "REGS",
                columns: table => new
                {
                    stid = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    tp = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    rindex = table.Column<int>(type: "int", nullable: false),
                    rtitle = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    ritem = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    rfn = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    ractive = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REGS", x => new { x.stid, x.tp, x.rindex });
                });

            migrationBuilder.CreateTable(
                name: "RRDET",
                columns: table => new
                {
                    rrid = table.Column<int>(type: "int", nullable: false),
                    stid = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    f1 = table.Column<string>(type: "text", nullable: true),
                    f2 = table.Column<string>(type: "text", nullable: true),
                    f3 = table.Column<string>(type: "text", nullable: true),
                    f4 = table.Column<string>(type: "text", nullable: true),
                    f5 = table.Column<string>(type: "text", nullable: true),
                    f6 = table.Column<string>(type: "text", nullable: true),
                    f7 = table.Column<string>(type: "text", nullable: true),
                    f8 = table.Column<string>(type: "text", nullable: true),
                    f9 = table.Column<string>(type: "text", nullable: true),
                    f10 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RRLIST",
                columns: table => new
                {
                    rrid = table.Column<int>(type: "int", nullable: false),
                    rrtitle = table.Column<string>(type: "char(25)", unicode: false, fixedLength: true, maxLength: 25, nullable: true),
                    rrname = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    rractive = table.Column<int>(type: "int", nullable: true),
                    rrorder = table.Column<int>(type: "int", nullable: true),
                    rrremarks = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    rrtp = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RRLIST", x => x.rrid);
                });

            migrationBuilder.CreateTable(
                name: "RRSTRUC",
                columns: table => new
                {
                    RRID = table.Column<int>(type: "int", nullable: false),
                    F1T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F1F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F2T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F2F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F3T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F3F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F4T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F4F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F5T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F5F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F6T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F6F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F7T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F7F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F8T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F8F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F9T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F9F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    F10T = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    F10F = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RRSTRUC", x => x.RRID);
                });

            migrationBuilder.CreateTable(
                name: "Servc_map",
                columns: table => new
                {
                    uno = table.Column<int>(type: "int", nullable: false),
                    vuno = table.Column<int>(type: "int", nullable: true),
                    echkuno = table.Column<int>(type: "int", nullable: true),
                    vchkuno = table.Column<int>(type: "int", nullable: true),
                    edoc = table.Column<int>(type: "int", nullable: true),
                    vcoid = table.Column<int>(type: "int", nullable: true),
                    ecoid = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    edoid = table.Column<int>(type: "int", nullable: true),
                    vproid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TRIGLINK",
                columns: table => new
                {
                    TCODE = table.Column<decimal>(type: "decimal(4,0)", nullable: false),
                    OCODE = table.Column<decimal>(type: "decimal(5,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIGLINK", x => new { x.TCODE, x.OCODE })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "TRIGS",
                columns: table => new
                {
                    TCODE = table.Column<decimal>(type: "decimal(4,0)", nullable: false),
                    TDESC = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    TTYPE = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIGS", x => x.TCODE)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "UserActivation",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActivationCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivation", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Webinar",
                columns: table => new
                {
                    email = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    active = table.Column<int>(type: "int", nullable: true),
                    Reference = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NCMLOCBO",
                columns: table => new
                {
                    ProjectCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    lcode = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    OvalID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GeneralContractor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProjectAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NatureofWork = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProjectArea = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ProjectCost_est = table.Column<double>(type: "float", nullable: true),
                    ProjectStartDate_est = table.Column<DateOnly>(type: "date", nullable: true),
                    ProjectEndDate_est = table.Column<DateOnly>(type: "date", nullable: true),
                    VendorCount = table.Column<int>(type: "int", nullable: false),
                    WorkerHeadCount = table.Column<int>(type: "int", nullable: false),
                    ProjectLead = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NCMLOCBO", x => x.ProjectCode);
                    table.ForeignKey(
                        name: "FK_NCMLOCBO_NCMLOC",
                        column: x => x.lcode,
                        principalTable: "NCMLOC",
                        principalColumn: "lcode");
                });

            migrationBuilder.CreateIndex(
                name: "acts0",
                table: "ACTS",
                columns: new[] { "CODE", "SNAME", "LNAME" });

            migrationBuilder.CreateIndex(
                name: "acts00",
                table: "ACTS",
                columns: new[] { "CODE", "ATYPE", "STCODE", "SNAME", "YEAR" });

            migrationBuilder.CreateIndex(
                name: "ACT",
                table: "FULLTEXT",
                column: "ACT");

            migrationBuilder.CreateIndex(
                name: "SINDEX",
                table: "FULLTEXT",
                column: "SINDEX");

            migrationBuilder.CreateIndex(
                name: "notistate",
                table: "MASTSTATES",
                columns: new[] { "STATEID", "STATEDESC" },
                unique: true,
                filter: "[STATEDESC] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NCMLOCBO_lcode",
                table: "NCMLOCBO",
                column: "lcode");

            migrationBuilder.CreateIndex(
                name: "CODEACT",
                table: "OBLIGATI",
                columns: new[] { "CODE", "ACT" });

            migrationBuilder.CreateIndex(
                name: "obl",
                table: "OBLIGATI",
                columns: new[] { "CODE", "ACT", "TITLE", "OBLINDEX" });

            migrationBuilder.CreateIndex(
                name: "obligati0",
                table: "OBLIGATI",
                columns: new[] { "ACT", "CODE" });

            migrationBuilder.CreateIndex(
                name: "obligati000",
                table: "OBLIGATI",
                column: "CODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACTS");

            migrationBuilder.DropTable(
                name: "ALINKS");

            migrationBuilder.DropTable(
                name: "AUDTRAIL");

            migrationBuilder.DropTable(
                name: "BOCW_SCOPE");

            migrationBuilder.DropTable(
                name: "CALENDAR");

            migrationBuilder.DropTable(
                name: "CONTACT");

            migrationBuilder.DropTable(
                name: "CUSERS");

            migrationBuilder.DropTable(
                name: "CUSERS2");

            migrationBuilder.DropTable(
                name: "DOCLINKS");

            migrationBuilder.DropTable(
                name: "DOCLIST");

            migrationBuilder.DropTable(
                name: "DORG");

            migrationBuilder.DropTable(
                name: "echkTrail");

            migrationBuilder.DropTable(
                name: "emaillist");

            migrationBuilder.DropTable(
                name: "ENQUIRY_table");

            migrationBuilder.DropTable(
                name: "FTLINK");

            migrationBuilder.DropTable(
                name: "FTLINKS");

            migrationBuilder.DropTable(
                name: "FULLTEXT");

            migrationBuilder.DropTable(
                name: "LLAUTH");

            migrationBuilder.DropTable(
                name: "LLDEF");

            migrationBuilder.DropTable(
                name: "LLFAQ");

            migrationBuilder.DropTable(
                name: "LLINKS");

            migrationBuilder.DropTable(
                name: "LLOFF");

            migrationBuilder.DropTable(
                name: "LOcation");

            migrationBuilder.DropTable(
                name: "MASTREG");

            migrationBuilder.DropTable(
                name: "MASTSTATES");

            migrationBuilder.DropTable(
                name: "MWCAT");

            migrationBuilder.DropTable(
                name: "MWDATA");

            migrationBuilder.DropTable(
                name: "MWNOTES");

            migrationBuilder.DropTable(
                name: "NCACTAKEN");

            migrationBuilder.DropTable(
                name: "NCACTION");

            migrationBuilder.DropTable(
                name: "NCAUDET");

            migrationBuilder.DropTable(
                name: "NCAUDMAST");

            migrationBuilder.DropTable(
                name: "NCAUMAP");

            migrationBuilder.DropTable(
                name: "NCCONTR");

            migrationBuilder.DropTable(
                name: "NCFILES");

            migrationBuilder.DropTable(
                name: "NCFIN");

            migrationBuilder.DropTable(
                name: "NCLOCMAP");

            migrationBuilder.DropTable(
                name: "NCMLOCBO");

            migrationBuilder.DropTable(
                name: "NCMORG");

            migrationBuilder.DropTable(
                name: "NCREG");

            migrationBuilder.DropTable(
                name: "NCRET");

            migrationBuilder.DropTable(
                name: "NCTEMPCNT");

            migrationBuilder.DropTable(
                name: "NCTEMPFIN");

            migrationBuilder.DropTable(
                name: "NCTEMPRET");

            migrationBuilder.DropTable(
                name: "NCUMAP");

            migrationBuilder.DropTable(
                name: "NCUSER");

            migrationBuilder.DropTable(
                name: "NOTI");

            migrationBuilder.DropTable(
                name: "OBLIGATI");

            migrationBuilder.DropTable(
                name: "REGS");

            migrationBuilder.DropTable(
                name: "RRDET");

            migrationBuilder.DropTable(
                name: "RRLIST");

            migrationBuilder.DropTable(
                name: "RRSTRUC");

            migrationBuilder.DropTable(
                name: "Servc_map");

            migrationBuilder.DropTable(
                name: "TRIGLINK");

            migrationBuilder.DropTable(
                name: "TRIGS");

            migrationBuilder.DropTable(
                name: "UserActivation");

            migrationBuilder.DropTable(
                name: "Webinar");

            migrationBuilder.DropTable(
                name: "NCMLOC");
        }
    }
}
