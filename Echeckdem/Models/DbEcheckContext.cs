using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Models;

public partial class DbEcheckContext : DbContext
{
    public DbEcheckContext()
    {
    }

    public DbEcheckContext(DbContextOptions<DbEcheckContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Act> Acts { get; set; }

    public virtual DbSet<Alink> Alinks { get; set; }

    public virtual DbSet<Audtrail> Audtrails { get; set; }

    public virtual DbSet<BoScopeMap> BoScopeMaps { get; set; }

    public virtual DbSet<BocwScope> BocwScopes { get; set; }

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Cuser> Cusers { get; set; }

    public virtual DbSet<Cusers2> Cusers2s { get; set; }

    public virtual DbSet<Doclink> Doclinks { get; set; }

    public virtual DbSet<Doclist> Doclists { get; set; }

    public virtual DbSet<Dorg> Dorgs { get; set; }

    public virtual DbSet<EchkTrail> EchkTrails { get; set; }

    public virtual DbSet<Emaillist> Emaillists { get; set; }

    public virtual DbSet<EnquiryTable> EnquiryTables { get; set; }

    public virtual DbSet<Ftlink> Ftlinks { get; set; }

    public virtual DbSet<Ftlink1> Ftlinks1 { get; set; }

    public virtual DbSet<Fulltext> Fulltexts { get; set; }

    public virtual DbSet<Llauth> Llauths { get; set; }

    public virtual DbSet<Lldef> Lldefs { get; set; }

    public virtual DbSet<Llfaq> Llfaqs { get; set; }

    public virtual DbSet<Llink> Llinks { get; set; }

    public virtual DbSet<Lloff> Lloffs { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Mastreg> Mastregs { get; set; }

    public virtual DbSet<Maststate> Maststates { get; set; }

    public virtual DbSet<Mwcat> Mwcats { get; set; }

    public virtual DbSet<Mwdatum> Mwdata { get; set; }

    public virtual DbSet<Mwnote> Mwnotes { get; set; }

    public virtual DbSet<Ncactaken> Ncactakens { get; set; }

    public virtual DbSet<Ncaction> Ncactions { get; set; }

    public virtual DbSet<Ncaudet> Ncaudets { get; set; }

    public virtual DbSet<Ncaudmast> Ncaudmasts { get; set; }

    public virtual DbSet<Ncaumap> Ncaumaps { get; set; }

    public virtual DbSet<Ncbocw> Ncbocws { get; set; }

    public virtual DbSet<Nccontr> Nccontrs { get; set; }

    public virtual DbSet<Ncfile> Ncfiles { get; set; }

    public virtual DbSet<Ncfin> Ncfins { get; set; }

    public virtual DbSet<Nclocmap> Nclocmaps { get; set; }

    public virtual DbSet<Ncmloc> Ncmlocs { get; set; }

    public virtual DbSet<Ncmlocbo> Ncmlocbos { get; set; }

    public virtual DbSet<Ncmorg> Ncmorgs { get; set; }

    public virtual DbSet<Ncreg> Ncregs { get; set; }

    public virtual DbSet<Ncret> Ncrets { get; set; }

    public virtual DbSet<Nctempcnt> Nctempcnts { get; set; }

    public virtual DbSet<Nctempfin> Nctempfins { get; set; }

    public virtual DbSet<Nctempret> Nctemprets { get; set; }

    public virtual DbSet<Ncumap> Ncumaps { get; set; }

    public virtual DbSet<Ncuser> Ncusers { get; set; }

    public virtual DbSet<Noti> Notis { get; set; }

    public virtual DbSet<Obligati> Obligatis { get; set; }

    public virtual DbSet<Reg> Regs { get; set; }

    public virtual DbSet<Rrdet> Rrdets { get; set; }

    public virtual DbSet<Rrlist> Rrlists { get; set; }

    public virtual DbSet<Rrstruc> Rrstrucs { get; set; }

    public virtual DbSet<ServcMap> ServcMaps { get; set; }

    public virtual DbSet<Statusmaster> Statusmasters { get; set; }

    public virtual DbSet<TrackScope> TrackScopes { get; set; }

    public virtual DbSet<Trig> Trigs { get; set; }

    public virtual DbSet<Triglink> Triglinks { get; set; }

    public virtual DbSet<UserActivation> UserActivations { get; set; }

    public virtual DbSet<Webinar> Webinars { get; set; }

    public virtual DbSet<ContributionViewModel> ContributionViewModel { get; set; }

    public virtual DbSet<RegistrationViewModel> RegistrationViewModel { get; set; }

    public virtual DbSet<ReturnsViewModel> ReturnsViewModel { get; set; }

    public virtual DbSet<BocwViewModel> BocwViewModel { get; set; }



//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=AGAM\\SQLEXPRESS01;database=DB_echeck;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ReturnsViewModel>()
.HasNoKey()
.ToView(null);

        modelBuilder.Entity<ContributionViewModel>()
       .HasNoKey()
       .ToView(null);

        modelBuilder.Entity<RegistrationViewModel>()
      .HasNoKey()
      .ToView(null);

        modelBuilder.Entity<BocwViewModel>()
      .HasNoKey()
      .ToView(null);


        modelBuilder.Entity<Act>(entity =>
        {
            entity.HasKey(e => e.Code).IsClustered(false);

            entity.ToTable("ACTS");

            entity.HasIndex(e => new { e.Code, e.Sname, e.Lname }, "acts0");

            entity.HasIndex(e => new { e.Code, e.Atype, e.Stcode, e.Sname, e.Year }, "acts00");

            entity.Property(e => e.Code)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CODE");
            entity.Property(e => e.Aactive).HasColumnName("AACTIVE");
            entity.Property(e => e.Alegtype)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ALEGTYPE");
            entity.Property(e => e.Appl)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("APPL");
            entity.Property(e => e.Applicability)
                .HasMaxLength(3000)
                .IsUnicode(false)
                .HasColumnName("APPLICABILITY");
            entity.Property(e => e.Atype)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ATYPE");
            entity.Property(e => e.Filename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FILENAME");
            entity.Property(e => e.Lastamend)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LASTAMEND");
            entity.Property(e => e.Legtype)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LEGTYPE");
            entity.Property(e => e.Lname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LNAME");
            entity.Property(e => e.Nochk).HasColumnName("NOCHK");
            entity.Property(e => e.Nodoc).HasColumnName("NODOC");
            entity.Property(e => e.Objectives)
                .HasMaxLength(3000)
                .IsUnicode(false)
                .HasColumnName("OBJECTIVES");
            entity.Property(e => e.Otherleg)
                .HasMaxLength(250)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OTHERLEG");
            entity.Property(e => e.Parleg)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PARLEG");
            entity.Property(e => e.Sname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SNAME");
            entity.Property(e => e.Stcode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STCODE");
            entity.Property(e => e.Year)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("YEAR");
        });

        modelBuilder.Entity<Alink>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ALINKS");

            entity.Property(e => e.Acode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACODE");
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LCODE");
            entity.Property(e => e.Module)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MODULE");
        });

        modelBuilder.Entity<Audtrail>(entity =>
        {
            entity.HasKey(e => e.Tindex).IsClustered(false);

            entity.ToTable("AUDTRAIL");

            entity.Property(e => e.Tindex)
                .ValueGeneratedOnAdd()
                .HasColumnName("TINDEX");
            entity.Property(e => e.Activity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACTIVITY");
            entity.Property(e => e.Details)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DETAILS");
            entity.Property(e => e.Origin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ORIGIN");
            entity.Property(e => e.Tdate)
                .HasColumnType("datetime")
                .HasColumnName("TDATE");
            entity.Property(e => e.Ttable)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TTABLE");
            entity.Property(e => e.Ttime)
                .HasColumnType("datetime")
                .HasColumnName("TTIME");
            entity.Property(e => e.Uids)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("UIDS");
        });

        modelBuilder.Entity<BoScopeMap>(entity =>
        {
            entity.HasKey(e => e.ScopeMapId);

            entity.ToTable("BO_SCOPE_MAP");

            entity.HasIndex(e => new { e.Lcode, e.ProjectCode, e.ScopeId }, "IX_BoScopeMaps").IsUnique();

            entity.Property(e => e.ScopeMapId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ScopeMapID");
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("lcode");
            entity.Property(e => e.ProjectCode).HasMaxLength(10);
            entity.Property(e => e.ScopeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ScopeID");

            entity.HasOne(d => d.LcodeNavigation).WithMany(p => p.BoScopeMaps)
                .HasForeignKey(d => d.Lcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BO_SCOPE_MAP_NCMLOC");

            entity.HasOne(d => d.ProjectCodeNavigation).WithMany(p => p.BoScopeMaps)
                .HasForeignKey(d => d.ProjectCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BO_SCOPE_MAP_NCMLOCBO");

            entity.HasOne(d => d.Scope).WithMany(p => p.BoScopeMaps)
                .HasForeignKey(d => d.ScopeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BO_SCOPE_MAP_BOCW_SCOPE");
        });

        modelBuilder.Entity<BocwScope>(entity =>
        {
            entity.HasKey(e => e.ScopeId);

            entity.ToTable("BOCW_SCOPE");

            entity.Property(e => e.ScopeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ScopeID");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.Frequency)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FunctionName).IsUnicode(false);
            entity.Property(e => e.ScopeName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => new { e.Oblig, e.Obldate }).IsClustered(false);

            entity.ToTable("CALENDAR");

            entity.Property(e => e.Oblig)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("OBLIG");
            entity.Property(e => e.Obldate)
                .HasColumnType("datetime")
                .HasColumnName("OBLDATE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.Warndate)
                .HasColumnType("datetime")
                .HasColumnName("WARNDATE");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Userid).IsClustered(false);

            entity.ToTable("CONTACT");

            entity.Property(e => e.Userid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USERID");
            entity.Property(e => e.Altemail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ALTEMAIL");
            entity.Property(e => e.Ccode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CCODE");
            entity.Property(e => e.Desig)
                .HasMaxLength(75)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DESIG");
            entity.Property(e => e.Emailid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EMAILID");
            entity.Property(e => e.Fax)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FAX");
            entity.Property(e => e.Fname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FNAME");
            entity.Property(e => e.Fullname)
                .HasMaxLength(45)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FULLNAME");
            entity.Property(e => e.Lastpass)
                .HasColumnType("datetime")
                .HasColumnName("LASTPASS");
            entity.Property(e => e.Lname)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LNAME");
            entity.Property(e => e.Location)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LOCATION");
            entity.Property(e => e.Orgcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ORGCODE");
            entity.Property(e => e.Pass)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PASS");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PHONE");
            entity.Property(e => e.Stcode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STCODE");
            entity.Property(e => e.Uactive)
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("UACTIVE");
            entity.Property(e => e.Userlevel)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("USERLEVEL");
        });

        modelBuilder.Entity<Cuser>(entity =>
        {
            entity.HasKey(e => e.Userid);

            entity.ToTable("CUSERS");

            entity.Property(e => e.Userid)
                .HasMaxLength(60)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("userid");
            entity.Property(e => e.Crdate).HasColumnName("crdate");
            entity.Property(e => e.Defstate)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("defstate");
            entity.Property(e => e.Expdate).HasColumnName("expdate");
            entity.Property(e => e.Mail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("mail");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Uactive).HasColumnName("uactive");
            entity.Property(e => e.Ulevel).HasColumnName("ulevel");
            entity.Property(e => e.Uname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("uname");
            entity.Property(e => e.Uno).HasColumnName("uno");
            entity.Property(e => e.Uorg)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("uorg");
            entity.Property(e => e.Ustates)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ustates");
            entity.Property(e => e.Utype)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("utype");
        });

        modelBuilder.Entity<Cusers2>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CUSERS2");

            entity.Property(e => e.Ucrdate).HasColumnName("ucrdate");
            entity.Property(e => e.Uexdate).HasColumnName("uexdate");
            entity.Property(e => e.Uno).HasColumnName("uno");
            entity.Property(e => e.Utype).HasColumnName("utype");
            entity.Property(e => e.Utype1).HasColumnName("utype1");
        });

        modelBuilder.Entity<Doclink>(entity =>
        {
            entity.HasKey(e => new { e.Ocode, e.Dcode }).IsClustered(false);

            entity.ToTable("DOCLINKS");

            entity.Property(e => e.Ocode)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("OCODE");
            entity.Property(e => e.Dcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DCODE");
        });

        modelBuilder.Entity<Doclist>(entity =>
        {
            entity.HasKey(e => e.Dcode).IsClustered(false);

            entity.ToTable("DOCLIST");

            entity.Property(e => e.Dcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DCODE");
            entity.Property(e => e.Acode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACODE");
            entity.Property(e => e.Dcat)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DCAT");
            entity.Property(e => e.Ddesc)
                .HasMaxLength(150)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DDESC");
            entity.Property(e => e.Dindex).HasColumnName("DINDEX");
            entity.Property(e => e.Dirname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DIRNAME");
            entity.Property(e => e.Docname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCNAME");
            entity.Property(e => e.Dtitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DTITLE");
            entity.Property(e => e.Dtype)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DTYPE");
            entity.Property(e => e.Oacode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OACODE");
        });

        modelBuilder.Entity<Dorg>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DORG");

            entity.Property(e => e.Active).HasColumnName("ACTIVE");
            entity.Property(e => e.Lenddate)
                .HasColumnType("datetime")
                .HasColumnName("LENDDATE");
            entity.Property(e => e.Lstartdate)
                .HasColumnType("datetime")
                .HasColumnName("LSTARTDATE");
            entity.Property(e => e.Orgcode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ORGCODE");
            entity.Property(e => e.Orgdesc)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("ORGDESC");
            entity.Property(e => e.States)
                .HasColumnType("text")
                .HasColumnName("STATES");
        });

        modelBuilder.Entity<EchkTrail>(entity =>
        {
            entity.HasKey(e => e.Tindex).IsClustered(false);

            entity.ToTable("echkTrail");

            entity.Property(e => e.Tindex)
                .ValueGeneratedNever()
                .HasColumnName("TINDEX");
            entity.Property(e => e.Activity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACTIVITY");
            entity.Property(e => e.Details)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DETAILS");
            entity.Property(e => e.Origin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ORIGIN");
            entity.Property(e => e.Tdate)
                .HasColumnType("datetime")
                .HasColumnName("TDATE");
            entity.Property(e => e.Ttable)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TTABLE");
            entity.Property(e => e.Ttime)
                .HasColumnType("datetime")
                .HasColumnName("TTIME");
            entity.Property(e => e.Uids)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("UIDS");
        });

        modelBuilder.Entity<Emaillist>(entity =>
        {
            entity.HasKey(e => e.Email);

            entity.ToTable("emaillist");

            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Category)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<EnquiryTable>(entity =>
        {
            entity.HasKey(e => e.Enid);

            entity.ToTable("ENQUIRY_table");

            entity.Property(e => e.Enid).HasColumnName("ENID");
            entity.Property(e => e.Edesignation)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Eemail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Eintrest)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Emessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Ename)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Eorganization)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Ereference)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ftlink>(entity =>
        {
            entity.HasKey(e => new { e.Seccode, e.Module, e.Lcode }).IsClustered(false);

            entity.ToTable("FTLINK");

            entity.Property(e => e.Seccode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SECCODE");
            entity.Property(e => e.Module)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MODULE");
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LCODE");
        });

        modelBuilder.Entity<Ftlink1>(entity =>
        {
            entity.HasKey(e => new { e.Ocode, e.Seccode }).IsClustered(false);

            entity.ToTable("FTLINKS");

            entity.Property(e => e.Ocode)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("OCODE");
            entity.Property(e => e.Seccode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SECCODE");
        });

        modelBuilder.Entity<Fulltext>(entity =>
        {
            entity.HasKey(e => e.Seccode).IsClustered(false);

            entity.ToTable("FULLTEXT");

            entity.HasIndex(e => e.Act, "ACT");

            entity.HasIndex(e => e.Sindex, "SINDEX");

            entity.Property(e => e.Seccode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SECCODE");
            entity.Property(e => e.Act)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACT");
            entity.Property(e => e.Chapter)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CHAPTER");
            entity.Property(e => e.Ftext)
                .IsUnicode(false)
                .HasColumnName("FTEXT");
            entity.Property(e => e.Section)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SECTION");
            entity.Property(e => e.Sindex).HasColumnName("SINDEX");
            entity.Property(e => e.Title)
                .HasMaxLength(120)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Llauth>(entity =>
        {
            entity.HasKey(e => e.Aucode);

            entity.ToTable("LLAUTH");

            entity.Property(e => e.Aucode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("AUCODE");
            entity.Property(e => e.Acode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACODE");
            entity.Property(e => e.Auth)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("AUTH");
            entity.Property(e => e.Duties)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DUTIES");
            entity.Property(e => e.Jurisdiction)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JURISDICTION");
            entity.Property(e => e.Powers)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("POWERS");
            entity.Property(e => e.Section)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SECTION");
        });

        modelBuilder.Entity<Lldef>(entity =>
        {
            entity.HasKey(e => e.Seccode);

            entity.ToTable("LLDEF");

            entity.Property(e => e.Seccode)
                .HasMaxLength(10)
                .HasColumnName("SECCODE");
            entity.Property(e => e.Acode)
                .HasMaxLength(10)
                .HasColumnName("ACODE");
            entity.Property(e => e.Dtext)
                .IsUnicode(false)
                .HasColumnName("DTEXT");
            entity.Property(e => e.Section)
                .HasMaxLength(6)
                .HasColumnName("SECTION");
            entity.Property(e => e.Title)
                .HasMaxLength(120)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Llfaq>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LLFAQ");

            entity.Property(e => e.Cat1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CAT1");
            entity.Property(e => e.Cat2)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CAT2");
            entity.Property(e => e.Crdate).HasColumnName("CRDATE");
            entity.Property(e => e.Qactive).HasColumnName("QACTIVE");
            entity.Property(e => e.Qanswer)
                .IsUnicode(false)
                .HasColumnName("QANSWER");
            entity.Property(e => e.Qcode).HasColumnName("QCODE");
            entity.Property(e => e.Ques)
                .IsUnicode(false)
                .HasColumnName("QUES");
        });

        modelBuilder.Entity<Llink>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LLINKS");

            entity.Property(e => e.Crdate).HasColumnName("CRDATE");
            entity.Property(e => e.Lactive).HasColumnName("LACTIVE");
            entity.Property(e => e.Lcat)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("LCAT");
            entity.Property(e => e.Ldesc)
                .IsUnicode(false)
                .HasColumnName("LDESC");
            entity.Property(e => e.Lid).HasColumnName("LID");
            entity.Property(e => e.Ltitle)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("LTITLE");
            entity.Property(e => e.Lurl)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("LURL");
        });

        modelBuilder.Entity<Lloff>(entity =>
        {
            entity.HasKey(e => e.Offcode);

            entity.ToTable("LLOFF");

            entity.Property(e => e.Offcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OFFCODE");
            entity.Property(e => e.Acode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACODE");
            entity.Property(e => e.Maxfine).HasColumnName("MAXFINE");
            entity.Property(e => e.Maxprison).HasColumnName("MAXPRISON");
            entity.Property(e => e.Offence)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OFFENCE");
            entity.Property(e => e.Penalty)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PENALTY");
            entity.Property(e => e.Section)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SECTION");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOcation");

            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Location1)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("location");
        });

        modelBuilder.Entity<Mastreg>(entity =>
        {
            entity.HasKey(e => e.Rtype);

            entity.ToTable("MASTREG");

            entity.Property(e => e.Rtype)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Rdesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Maststate>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("pk_stateid");

            entity.ToTable("MASTSTATES");

            entity.HasIndex(e => new { e.Stateid, e.Statedesc }, "notistate").IsUnique();

            entity.Property(e => e.Stateid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATEID");
            entity.Property(e => e.Stactive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STACTIVE");
            entity.Property(e => e.Statedesc)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATEDESC");
        });

        modelBuilder.Entity<Mwcat>(entity =>
        {
            entity.HasKey(e => new { e.Stid, e.Catid }).HasName("PK_MWCAT_1");

            entity.ToTable("MWCAT");

            entity.Property(e => e.Stid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("stid");
            entity.Property(e => e.Catid).HasColumnName("catid");
            entity.Property(e => e.Catdesc)
                .IsUnicode(false)
                .HasColumnName("catdesc");
            entity.Property(e => e.Catgrp)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("catgrp");
            entity.Property(e => e.Catlnk).HasColumnName("catlnk");
            entity.Property(e => e.Catname)
                .HasMaxLength(75)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("catname");
        });

        modelBuilder.Entity<Mwdatum>(entity =>
        {
            entity.HasKey(e => new { e.Stid, e.Catid, e.Stdate });

            entity.ToTable("MWDATA");

            entity.Property(e => e.Stid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STID");
            entity.Property(e => e.Catid).HasColumnName("catid");
            entity.Property(e => e.Stdate).HasColumnName("stdate");
            entity.Property(e => e.Basic)
                .HasDefaultValue(0.0)
                .HasColumnName("basic");
            entity.Property(e => e.Da)
                .HasDefaultValue(0.0)
                .HasColumnName("da");
            entity.Property(e => e.Daily)
                .HasDefaultValue(0.0)
                .HasColumnName("daily");
            entity.Property(e => e.Endate).HasColumnName("endate");
            entity.Property(e => e.Hra).HasColumnName("hra");
            entity.Property(e => e.Monthly)
                .HasDefaultValue(0.0)
                .HasColumnName("monthly");
            entity.Property(e => e.Notdate).HasColumnName("notdate");
            entity.Property(e => e.Notfile)
                .HasMaxLength(80)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("notfile");
            entity.Property(e => e.Other)
                .HasDefaultValue(0.0)
                .HasColumnName("other");
            entity.Property(e => e.Spallowence).HasColumnName("spallowence");
            entity.Property(e => e.Z2b)
                .HasDefaultValue(0.0)
                .HasColumnName("z2b");
            entity.Property(e => e.Z2d)
                .HasDefaultValue(0.0)
                .HasColumnName("z2d");
            entity.Property(e => e.Z2da)
                .HasDefaultValue(0.0)
                .HasColumnName("z2da");
            entity.Property(e => e.Z2hra).HasColumnName("z2hra");
            entity.Property(e => e.Z2m)
                .HasDefaultValue(0.0)
                .HasColumnName("z2m");
            entity.Property(e => e.Z2other)
                .HasDefaultValue(0.0)
                .HasColumnName("z2other");
            entity.Property(e => e.Z2spallowence).HasColumnName("z2spallowence");
            entity.Property(e => e.Z3b)
                .HasDefaultValue(0.0)
                .HasColumnName("z3b");
            entity.Property(e => e.Z3d)
                .HasDefaultValue(0.0)
                .HasColumnName("z3d");
            entity.Property(e => e.Z3da)
                .HasDefaultValue(0.0)
                .HasColumnName("z3da");
            entity.Property(e => e.Z3hra).HasColumnName("z3hra");
            entity.Property(e => e.Z3m)
                .HasDefaultValue(0.0)
                .HasColumnName("z3m");
            entity.Property(e => e.Z3other)
                .HasDefaultValue(0.0)
                .HasColumnName("z3other");
            entity.Property(e => e.Z3spallowence).HasColumnName("z3spallowence");
            entity.Property(e => e.Z4b)
                .HasDefaultValue(0.0)
                .HasColumnName("z4b");
            entity.Property(e => e.Z4d)
                .HasDefaultValue(0.0)
                .HasColumnName("z4d");
            entity.Property(e => e.Z4da)
                .HasDefaultValue(0.0)
                .HasColumnName("z4da");
            entity.Property(e => e.Z4hra)
                .HasDefaultValue(0.0)
                .HasColumnName("z4hra");
            entity.Property(e => e.Z4m)
                .HasDefaultValue(0.0)
                .HasColumnName("z4m");
            entity.Property(e => e.Z4other)
                .HasDefaultValue(0.0)
                .HasColumnName("z4other");
            entity.Property(e => e.Z4spallowence)
                .HasDefaultValue(0.0)
                .HasColumnName("z4spallowence");
        });

        modelBuilder.Entity<Mwnote>(entity =>
        {
            entity.HasKey(e => new { e.Stid, e.Catgrp });

            entity.ToTable("MWNOTES");

            entity.Property(e => e.Stid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STID");
            entity.Property(e => e.Catgrp)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CATGRP");
            entity.Property(e => e.Stcats)
                .IsUnicode(false)
                .HasColumnName("STCATS");
            entity.Property(e => e.Stnotes)
                .IsUnicode(false)
                .HasColumnName("STNOTES");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.Tp1)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TP1");
            entity.Property(e => e.Tp2)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TP2");
            entity.Property(e => e.Z1n)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Z1N");
            entity.Property(e => e.Z2n)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Z2N");
            entity.Property(e => e.Z3n)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Z3N");
            entity.Property(e => e.Z4n)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Z4N");
        });

        modelBuilder.Entity<Ncactaken>(entity =>
        {
            entity.HasKey(e => e.Actid);

            entity.ToTable("NCACTAKEN");

            entity.Property(e => e.Actid).HasColumnName("ACTID");
            entity.Property(e => e.Accrdate).HasColumnName("ACCRDATE");
            entity.Property(e => e.Acdate).HasColumnName("ACDATE");
            entity.Property(e => e.Acid).HasColumnName("ACID");
            entity.Property(e => e.Actaken)
                .HasColumnType("text")
                .HasColumnName("ACTAKEN");
            entity.Property(e => e.Nacdate).HasColumnName("NACDATE");
            entity.Property(e => e.Showclient).HasColumnName("SHOWCLIENT");
            entity.Property(e => e.Uno).HasColumnName("UNO");
        });

        modelBuilder.Entity<Ncaction>(entity =>
        {
            entity.HasKey(e => e.Acid).HasName("pk_ncaction");

            entity.ToTable("NCACTION");

            entity.Property(e => e.Acid).HasColumnName("ACID");
            entity.Property(e => e.Accldate).HasColumnName("ACCLDATE");
            entity.Property(e => e.Acdetail)
                .HasColumnType("text")
                .HasColumnName("ACDETAIL");
            entity.Property(e => e.Acidate).HasColumnName("ACIDATE");
            entity.Property(e => e.Acistatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACISTATUS");
            entity.Property(e => e.Aclink).HasColumnName("ACLINK");
            entity.Property(e => e.Acrdate).HasColumnName("ACRDATE");
            entity.Property(e => e.Acremarks)
                .HasColumnType("text")
                .HasColumnName("ACREMARKS");
            entity.Property(e => e.Acruser).HasColumnName("ACRUSER");
            entity.Property(e => e.Acshow).HasColumnName("ACSHOW");
            entity.Property(e => e.Acstatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACSTATUS");
            entity.Property(e => e.Actitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ACTITLE");
            entity.Property(e => e.Actp)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACTP");
            entity.Property(e => e.Adocdate).HasColumnName("ADOCDATE");
            entity.Property(e => e.Atargdate).HasColumnName("ATARGDATE");
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LCODE");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OID");
            entity.Property(e => e.Sbtp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SBTP");
            entity.Property(e => e.Tpp)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("tpp");
        });

        modelBuilder.Entity<Ncaudet>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NCAUDET");

            entity.Property(e => e.Achk).HasColumnName("ACHK");
            entity.Property(e => e.Aid).HasColumnName("AID");
            entity.Property(e => e.Armk)
                .HasColumnType("text")
                .HasColumnName("ARMK");
            entity.Property(e => e.Ciid).HasColumnName("CIID");
        });

        modelBuilder.Entity<Ncaudmast>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NCAUDMAST");

            entity.Property(e => e.Aby)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ABY");
            entity.Property(e => e.Acldate).HasColumnName("ACLDATE");
            entity.Property(e => e.Aclosed).HasColumnName("ACLOSED");
            entity.Property(e => e.Acomplete).HasColumnName("ACOMPLETE");
            entity.Property(e => e.Adate).HasColumnName("ADATE");
            entity.Property(e => e.Adremarks)
                .HasColumnType("text")
                .HasColumnName("ADREMARKS");
            entity.Property(e => e.Aid)
                .ValueGeneratedOnAdd()
                .HasColumnName("AID");
            entity.Property(e => e.Aperiod)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APERIOD");
            entity.Property(e => e.Arepdate).HasColumnName("AREPDATE");
            entity.Property(e => e.Aschdate).HasColumnName("ASCHDATE");
            entity.Property(e => e.Ascore).HasColumnName("ASCORE");
            entity.Property(e => e.Atime).HasColumnName("ATIME");
            entity.Property(e => e.Audrep)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("AUDREP");
            entity.Property(e => e.Auser).HasColumnName("AUSER");
            entity.Property(e => e.Ayear).HasColumnName("AYEAR");
            entity.Property(e => e.Lcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("LCODE");
            entity.Property(e => e.Noofemps)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NOOFEMPS");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OID");
        });

        modelBuilder.Entity<Ncaumap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NCAUMAP");

            entity.Property(e => e.Acid).HasColumnName("ACID");
            entity.Property(e => e.Uauth).HasColumnName("UAUTH");
            entity.Property(e => e.Uno).HasColumnName("UNO");
        });

        modelBuilder.Entity<Ncbocw>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__NCBOCW__55433A4B862B9DDF");

            entity.ToTable("NCBOCW");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.FileName).IsUnicode(false);
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("lcode");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lname");
            entity.Property(e => e.ProjectCode).HasMaxLength(10);
            entity.Property(e => e.ScopeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ScopeID");
            entity.Property(e => e.Task)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.LcodeNavigation).WithMany(p => p.Ncbocws)
                .HasForeignKey(d => d.Lcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NCBOCW_NCMLOC");

            entity.HasOne(d => d.ProjectCodeNavigation).WithMany(p => p.Ncbocws)
                .HasForeignKey(d => d.ProjectCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NCBOCW_NCMLOCBO");

            entity.HasOne(d => d.Scope).WithMany(p => p.Ncbocws)
                .HasForeignKey(d => d.ScopeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NCBOCW_BOCW_SCOPE");
        });

        modelBuilder.Entity<Nccontr>(entity =>
        {
            entity.HasKey(e => new { e.Contid, e.Lcode, e.Oid }).HasName("PK_nccontr");

            entity.ToTable("NCCONTR");

            entity.Property(e => e.Contid)
                .ValueGeneratedOnAdd()
                .HasColumnName("CONTID");
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LCODE");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OID");
            entity.Property(e => e.Amount)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Chqdate).HasColumnName("CHQDATE");
            entity.Property(e => e.Chqno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CHQNO");
            entity.Property(e => e.Cyear).HasColumnName("CYEAR");
            entity.Property(e => e.Depdate).HasColumnName("DEPDATE");
            entity.Property(e => e.Filename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FILENAME");
            entity.Property(e => e.Fileup).HasColumnName("FILEUP");
            entity.Property(e => e.Freq)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FREQ");
            entity.Property(e => e.Lastdate).HasColumnName("LASTDATE");
            entity.Property(e => e.Ld).HasColumnName("LD");
            entity.Property(e => e.Period).HasColumnName("PERIOD");
            entity.Property(e => e.Remarks)
                .HasColumnType("text")
                .HasColumnName("REMARKS");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Tp)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TP");
            entity.Property(e => e.Uploaddate).HasColumnName("UPLOADDATE");
        });

        modelBuilder.Entity<Ncfile>(entity =>
        {
            entity.HasKey(e => e.Fid);

            entity.ToTable("NCFILES");

            entity.Property(e => e.Fid).HasColumnName("FID");
            entity.Property(e => e.Farc).HasColumnName("FARC");
            entity.Property(e => e.Flink).HasColumnName("FLINK");
            entity.Property(e => e.Fname)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Ftitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FTITLE");
            entity.Property(e => e.Ftp)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FTP");
            entity.Property(e => e.Fupdate).HasColumnName("FUPDATE");
            entity.Property(e => e.Lfile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LFILE");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OID");
        });

        modelBuilder.Entity<Ncfin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NCFIN");

            entity.Property(e => e.Depdate).HasColumnName("DEPDATE");
            entity.Property(e => e.ExcUsr)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("exc_usr");
            entity.Property(e => e.Filename)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("FILENAME");
            entity.Property(e => e.Fileup).HasColumnName("FILEUP");
            entity.Property(e => e.Lastdate).HasColumnName("LASTDATE");
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LCODE");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OID");
            entity.Property(e => e.PenaltyDelay)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("penalty_delay");
            entity.Property(e => e.Rcode).HasColumnName("RCODE");
            entity.Property(e => e.ReasonDelay)
                .HasColumnType("text")
                .HasColumnName("reason_delay");
            entity.Property(e => e.Remarks)
                .HasColumnType("text")
                .HasColumnName("REMARKS");
            entity.Property(e => e.Rtid)
                .ValueGeneratedOnAdd()
                .HasColumnName("RTID");
            entity.Property(e => e.Ryear).HasColumnName("RYEAR");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.TagUsr)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("tag_usr");
            entity.Property(e => e.Uploaddate).HasColumnName("UPLOADDATE");
        });

        modelBuilder.Entity<Nclocmap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NCLOCMAP");

            entity.Property(e => e.Acode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("acode");
            entity.Property(e => e.Lcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("lcode");
            entity.Property(e => e.Oblig).HasColumnName("oblig");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("oid");
        });

        modelBuilder.Entity<Ncmloc>(entity =>
        {
            entity.HasKey(e => e.Lcode).HasName("ncmloc_key");

            entity.ToTable("NCMLOC");

            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("lcode");
            entity.Property(e => e.Cemail)
                .IsUnicode(false)
                .HasColumnName("cemail");
            entity.Property(e => e.Doi).HasColumnName("doi");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.FinEsclatnCnt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Fin_esclatn_cnt");
            entity.Property(e => e.FinEsclatnMail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Fin_esclatn_mail");
            entity.Property(e => e.FinRespEmail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Fin_resp_email");
            entity.Property(e => e.FinRespPrsn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Fin_resp_prsn");
            entity.Property(e => e.Iemail)
                .IsUnicode(false)
                .HasColumnName("iemail");
            entity.Property(e => e.Iscentral).HasColumnName("iscentral");
            entity.Property(e => e.Iscloc).HasColumnName("iscloc");
            entity.Property(e => e.Isesi).HasColumnName("isesi");
            entity.Property(e => e.Ispf).HasColumnName("ispf");
            entity.Property(e => e.Lactive).HasColumnName("lactive");
            entity.Property(e => e.Laddress)
                .HasColumnType("text")
                .HasColumnName("laddress");
            entity.Property(e => e.Lcity)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lcity");
            entity.Property(e => e.Lconemail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lconemail");
            entity.Property(e => e.Lconno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lconno");
            entity.Property(e => e.Lcontact)
                .HasColumnType("text")
                .HasColumnName("lcontact");
            entity.Property(e => e.Lgroup2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lgroup2");
            entity.Property(e => e.Lgroup3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lgroup3");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lname");
            entity.Property(e => e.Lregion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lregion");
            entity.Property(e => e.Lsetup).HasColumnName("lsetup");
            entity.Property(e => e.Lstate)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("lstate");
            entity.Property(e => e.Ltype)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ltype");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("oid");
            entity.Property(e => e.Stdate).HasColumnName("stdate");
        });

        modelBuilder.Entity<Ncmlocbo>(entity =>
        {
            entity.HasKey(e => e.ProjectCode);

            entity.ToTable("NCMLOCBO");

            entity.Property(e => e.ProjectCode).HasMaxLength(10);
            entity.Property(e => e.ClientName).HasMaxLength(100);
            entity.Property(e => e.GeneralContractor).HasMaxLength(100);
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("lcode");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lname");
            entity.Property(e => e.NatureofWork).HasMaxLength(255);
            entity.Property(e => e.OvalId)
                .HasMaxLength(10)
                .HasColumnName("OvalID");
            entity.Property(e => e.ProjectAddress).HasMaxLength(255);
            entity.Property(e => e.ProjectArea).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.ProjectCostEst).HasColumnName("ProjectCost_est");
            entity.Property(e => e.ProjectEndDateEst).HasColumnName("ProjectEndDate_est");
            entity.Property(e => e.ProjectLead).HasMaxLength(255);
            entity.Property(e => e.ProjectStartDateEst).HasColumnName("ProjectStartDate_est");

            entity.HasOne(d => d.LcodeNavigation).WithMany(p => p.Ncmlocbos)
                .HasForeignKey(d => d.Lcode)
                .HasConstraintName("FK_NCMLOCBO_NCMLOC");
        });

        modelBuilder.Entity<Ncmorg>(entity =>
        {
            entity.HasKey(e => e.Oid);

            entity.ToTable("NCMORG");

            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("oid");
            entity.Property(e => e.Client).HasDefaultValue(true);
            entity.Property(e => e.Contemail)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Contname)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Echeck).HasDefaultValue(true);
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.Oactive).HasColumnName("oactive");
            entity.Property(e => e.Oname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("oname");
            entity.Property(e => e.Otype).HasColumnName("otype");
            entity.Property(e => e.Spoc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SpocEml)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Spoc_eml");
            entity.Property(e => e.Styear).HasColumnName("styear");
        });

        modelBuilder.Entity<Ncreg>(entity =>
        {
            entity.HasKey(e => new { e.Uid, e.Oid, e.Lcode }).HasName("pk_your_table");

            entity.ToTable("NCREG");

            entity.Property(e => e.Uid)
                .ValueGeneratedOnAdd()
                .HasColumnName("UID");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OID");
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LCODE");
            entity.Property(e => e.Doe).HasColumnName("DOE");
            entity.Property(e => e.Doi).HasColumnName("DOI");
            entity.Property(e => e.Dolr).HasColumnName("DOLR");
            entity.Property(e => e.Filename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FILENAME");
            entity.Property(e => e.Nmoe)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NMOE");
            entity.Property(e => e.Noe).HasColumnName("NOE");
            entity.Property(e => e.Remarks)
                .HasColumnType("text")
                .HasColumnName("REMARKS");
            entity.Property(e => e.Renhist)
                .HasColumnType("text")
                .HasColumnName("RENHIST");
            entity.Property(e => e.Rno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RNO");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
            entity.Property(e => e.Tp)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TP");
        });

        modelBuilder.Entity<Ncret>(entity =>
        {
            entity.HasKey(e => new { e.Rtid, e.Lcode, e.Oid }).HasName("PK_ncret");

            entity.ToTable("NCRET");

            entity.Property(e => e.Rtid)
                .ValueGeneratedOnAdd()
                .HasColumnName("RTID");
            entity.Property(e => e.Lcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LCODE");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OID");
            entity.Property(e => e.Depdate).HasColumnName("DEPDATE");
            entity.Property(e => e.Filename)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("FILENAME");
            entity.Property(e => e.Fileup).HasColumnName("FILEUP");
            entity.Property(e => e.Lastdate).HasColumnName("LASTDATE");
            entity.Property(e => e.Rcode).HasColumnName("RCODE");
            entity.Property(e => e.Remarks)
                .HasColumnType("text")
                .HasColumnName("REMARKS");
            entity.Property(e => e.Ryear).HasColumnName("RYEAR");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Uploaddate).HasColumnName("UPLOADDATE");
        });

        modelBuilder.Entity<Nctempcnt>(entity =>
        {
            entity.HasKey(e => e.Cid).HasName("pk_cid");

            entity.ToTable("NCTEMPCNT");

            entity.Property(e => e.Cid).HasColumnName("CID");
            entity.Property(e => e.Cstate)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CSTATE");
            entity.Property(e => e.Freq)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FREQ");
            entity.Property(e => e.Ld).HasColumnName("LD");
            entity.Property(e => e.Moffset).HasColumnName("MOFFSET");
            entity.Property(e => e.Period).HasColumnName("PERIOD");
            entity.Property(e => e.Tp)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TP");
        });

        modelBuilder.Entity<Nctempfin>(entity =>
        {
            entity.HasKey(e => e.Rcode).HasName("pk_rcodee");

            entity.ToTable("NCTEMPFIN");

            entity.Property(e => e.Rcode).HasColumnName("RCODE");
            entity.Property(e => e.Frequency)
                .HasMaxLength(50)
                .HasColumnName("frequency");
            entity.Property(e => e.Ract)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RACT");
            entity.Property(e => e.Rd).HasColumnName("RD");
            entity.Property(e => e.Rdesc)
                .HasColumnType("text")
                .HasColumnName("RDESC");
            entity.Property(e => e.Rform)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RFORM");
            entity.Property(e => e.Rm).HasColumnName("RM");
            entity.Property(e => e.Roblig).HasColumnName("ROBLIG");
            entity.Property(e => e.Rstate)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RSTATE");
            entity.Property(e => e.Rtitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RTITLE");
            entity.Property(e => e.Rtype)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RTYPE");
            entity.Property(e => e.Triggr)
                .HasDefaultValue(-5)
                .HasColumnName("triggr");
            entity.Property(e => e.Yroff).HasColumnName("YROFF");
        });

        modelBuilder.Entity<Nctempret>(entity =>
        {
            entity.HasKey(e => e.Rcode).HasName("pk_rcode");

            entity.ToTable("NCTEMPRET");

            entity.Property(e => e.Rcode).HasColumnName("RCODE");
            entity.Property(e => e.Ract)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RACT");
            entity.Property(e => e.Ractive).HasColumnName("RACTIVE");
            entity.Property(e => e.Rd).HasColumnName("RD");
            entity.Property(e => e.Rdesc)
                .HasColumnType("text")
                .HasColumnName("RDESC");
            entity.Property(e => e.Rform)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RFORM");
            entity.Property(e => e.Rm).HasColumnName("RM");
            entity.Property(e => e.Roblig).HasColumnName("ROBLIG");
            entity.Property(e => e.Rstate)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RSTATE");
            entity.Property(e => e.Rtitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RTITLE");
            entity.Property(e => e.Rtype)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RTYPE");
            entity.Property(e => e.Yroff).HasColumnName("YROFF");
        });

        modelBuilder.Entity<Ncumap>(entity =>
        {
            entity.HasKey(e => new { e.Oid, e.Lcode, e.Uno });

            entity.ToTable("NCUMAP");

            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("oid");
            entity.Property(e => e.Lcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("lcode");
            entity.Property(e => e.Uno).HasColumnName("uno");
            entity.Property(e => e.Ulevel)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("ulevel");
        });

        modelBuilder.Entity<Ncuser>(entity =>
        {
            entity.HasKey(e => e.Userid);

            entity.ToTable("NCUSER");

            entity.Property(e => e.Userid)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USERID");
            entity.Property(e => e.Emailid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EMAILID");
            entity.Property(e => e.Oid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OID");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Stcode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STCODE");
            entity.Property(e => e.Uactive).HasColumnName("UACTIVE");
            entity.Property(e => e.Uname)
                .HasMaxLength(40)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("UNAME");
            entity.Property(e => e.Uno)
                .ValueGeneratedOnAdd()
                .HasColumnName("UNO");
            entity.Property(e => e.Userlevel).HasColumnName("USERLEVEL");
        });

        modelBuilder.Entity<Noti>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NOTI");

            entity.Property(e => e.Acode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACODE");
            entity.Property(e => e.Applictn)
                .HasColumnType("text")
                .HasColumnName("applictn");
            entity.Property(e => e.Crdate).HasColumnName("CRDATE");
            entity.Property(e => e.Dcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DCODE");
            entity.Property(e => e.Ddate)
                .HasColumnType("datetime")
                .HasColumnName("DDATE");
            entity.Property(e => e.Ddesc)
                .IsUnicode(false)
                .HasColumnName("DDESC");
            entity.Property(e => e.Detail)
                .IsUnicode(false)
                .HasColumnName("DETAIL");
            entity.Property(e => e.Dtitle)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DTITLE");
            entity.Property(e => e.Exempt)
                .HasColumnType("text")
                .HasColumnName("exempt");
            entity.Property(e => e.Filename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FILENAME");
            entity.Property(e => e.Folder)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FOLDER");
            entity.Property(e => e.Nactive).HasColumnName("NACTIVE");
            entity.Property(e => e.Section)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SECTION");
            entity.Property(e => e.State)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("state");
            entity.Property(e => e.Wefdate)
                .HasColumnType("datetime")
                .HasColumnName("wefdate");
        });

        modelBuilder.Entity<Obligati>(entity =>
        {
            entity.HasKey(e => e.Code).IsClustered(false);

            entity.ToTable("OBLIGATI");

            entity.HasIndex(e => new { e.Code, e.Act }, "CODEACT");

            entity.HasIndex(e => new { e.Code, e.Act, e.Title, e.Oblindex }, "obl");

            entity.HasIndex(e => new { e.Act, e.Code }, "obligati0");

            entity.HasIndex(e => e.Code, "obligati000");

            entity.Property(e => e.Code)
                .ValueGeneratedNever()
                .HasColumnName("CODE");
            entity.Property(e => e.Act)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACT");
            entity.Property(e => e.Action)
                .HasMaxLength(3000)
                .IsUnicode(false)
                .HasColumnName("ACTION");
            entity.Property(e => e.Audits)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("AUDITS");
            entity.Property(e => e.Dsource)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("DSOURCE");
            entity.Property(e => e.Oactive)
                .HasDefaultValue(1)
                .HasColumnName("OACTIVE");
            entity.Property(e => e.Oblige)
                .HasMaxLength(250)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OBLIGE");
            entity.Property(e => e.Oblindex).HasColumnName("OBLINDEX");
            entity.Property(e => e.Parentcode).HasColumnName("PARENTCODE");
            entity.Property(e => e.Riskrat).HasColumnName("RISKRAT");
            entity.Property(e => e.Section)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SECTION");
            entity.Property(e => e.Support)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("SUPPORT");
            entity.Property(e => e.Timing)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TIMING");
            entity.Property(e => e.Title)
                .HasMaxLength(60)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TITLE");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TYPE");
        });

        modelBuilder.Entity<Reg>(entity =>
        {
            entity.HasKey(e => new { e.Stid, e.Tp, e.Rindex });

            entity.ToTable("REGS");

            entity.Property(e => e.Stid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("stid");
            entity.Property(e => e.Tp)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tp");
            entity.Property(e => e.Rindex).HasColumnName("rindex");
            entity.Property(e => e.Ractive).HasColumnName("ractive");
            entity.Property(e => e.Rfn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("rfn");
            entity.Property(e => e.Ritem)
                .IsUnicode(false)
                .HasColumnName("ritem");
            entity.Property(e => e.Rtitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("rtitle");
        });

        modelBuilder.Entity<Rrdet>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RRDET");

            entity.Property(e => e.F1)
                .HasColumnType("text")
                .HasColumnName("f1");
            entity.Property(e => e.F10)
                .HasColumnType("text")
                .HasColumnName("f10");
            entity.Property(e => e.F2)
                .HasColumnType("text")
                .HasColumnName("f2");
            entity.Property(e => e.F3)
                .HasColumnType("text")
                .HasColumnName("f3");
            entity.Property(e => e.F4)
                .HasColumnType("text")
                .HasColumnName("f4");
            entity.Property(e => e.F5)
                .HasColumnType("text")
                .HasColumnName("f5");
            entity.Property(e => e.F6)
                .HasColumnType("text")
                .HasColumnName("f6");
            entity.Property(e => e.F7)
                .HasColumnType("text")
                .HasColumnName("f7");
            entity.Property(e => e.F8)
                .HasColumnType("text")
                .HasColumnName("f8");
            entity.Property(e => e.F9)
                .HasColumnType("text")
                .HasColumnName("f9");
            entity.Property(e => e.Rrid).HasColumnName("rrid");
            entity.Property(e => e.Stid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("stid");
        });

        modelBuilder.Entity<Rrlist>(entity =>
        {
            entity.HasKey(e => e.Rrid);

            entity.ToTable("RRLIST");

            entity.Property(e => e.Rrid)
                .ValueGeneratedNever()
                .HasColumnName("rrid");
            entity.Property(e => e.Rractive).HasColumnName("rractive");
            entity.Property(e => e.Rrname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("rrname");
            entity.Property(e => e.Rrorder).HasColumnName("rrorder");
            entity.Property(e => e.Rrremarks)
                .IsUnicode(false)
                .HasColumnName("rrremarks");
            entity.Property(e => e.Rrtitle)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("rrtitle");
            entity.Property(e => e.Rrtp).HasColumnName("rrtp");
        });

        modelBuilder.Entity<Rrstruc>(entity =>
        {
            entity.HasKey(e => e.Rrid);

            entity.ToTable("RRSTRUC");

            entity.Property(e => e.Rrid)
                .ValueGeneratedNever()
                .HasColumnName("RRID");
            entity.Property(e => e.F10f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F10F");
            entity.Property(e => e.F10t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F10T");
            entity.Property(e => e.F1f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F1F");
            entity.Property(e => e.F1t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F1T");
            entity.Property(e => e.F2f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F2F");
            entity.Property(e => e.F2t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F2T");
            entity.Property(e => e.F3f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F3F");
            entity.Property(e => e.F3t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F3T");
            entity.Property(e => e.F4f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F4F");
            entity.Property(e => e.F4t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F4T");
            entity.Property(e => e.F5f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F5F");
            entity.Property(e => e.F5t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F5T");
            entity.Property(e => e.F6f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F6F");
            entity.Property(e => e.F6t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F6T");
            entity.Property(e => e.F7f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F7F");
            entity.Property(e => e.F7t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F7T");
            entity.Property(e => e.F8f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F8F");
            entity.Property(e => e.F8t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F8T");
            entity.Property(e => e.F9f)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("F9F");
            entity.Property(e => e.F9t)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F9T");
        });

        modelBuilder.Entity<ServcMap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Servc_map");

            entity.Property(e => e.Echkuno).HasColumnName("echkuno");
            entity.Property(e => e.Ecoid)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ecoid");
            entity.Property(e => e.Edoc).HasColumnName("edoc");
            entity.Property(e => e.Edoid).HasColumnName("edoid");
            entity.Property(e => e.Uno).HasColumnName("uno");
            entity.Property(e => e.Vchkuno).HasColumnName("vchkuno");
            entity.Property(e => e.Vcoid).HasColumnName("vcoid");
            entity.Property(e => e.Vproid).HasColumnName("vproid");
            entity.Property(e => e.Vuno).HasColumnName("vuno");
        });

        modelBuilder.Entity<Statusmaster>(entity =>
        {
            entity.HasKey(e => e.Smid);

            entity.ToTable("STATUSMASTER");

            entity.Property(e => e.Smid).HasColumnName("SMID");
            entity.Property(e => e.ScopeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ScopeID");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Scope).WithMany(p => p.Statusmasters)
                .HasForeignKey(d => d.ScopeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_STATUSMASTER_BOCW_SCOPE");
        });

        modelBuilder.Entity<TrackScope>(entity =>
        {
            entity.HasKey(e => e.WorkId).HasName("PK_TrackScope");

            entity.ToTable("Track_Scope");

            entity.Property(e => e.WorkId).HasColumnName("WorkID");
            entity.Property(e => e.Reference)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ScopeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ScopeID");
        });

        modelBuilder.Entity<Trig>(entity =>
        {
            entity.HasKey(e => e.Tcode).IsClustered(false);

            entity.ToTable("TRIGS");

            entity.Property(e => e.Tcode)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("TCODE");
            entity.Property(e => e.Tdesc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TDESC");
            entity.Property(e => e.Ttype)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TTYPE");
        });

        modelBuilder.Entity<Triglink>(entity =>
        {
            entity.HasKey(e => new { e.Tcode, e.Ocode }).IsClustered(false);

            entity.ToTable("TRIGLINK");

            entity.Property(e => e.Tcode)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("TCODE");
            entity.Property(e => e.Ocode)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("OCODE");
        });

        modelBuilder.Entity<UserActivation>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserActivation");

            entity.Property(e => e.UserId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Webinar>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Webinar");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Reference)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
