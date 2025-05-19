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

    
    public virtual DbSet<Audtrail> Audtrails { get; set; }

    public virtual DbSet<BoScopeMap> BoScopeMaps { get; set; }

    public virtual DbSet<BocwScope> BocwScopes { get; set; }

    
   

    public virtual DbSet<Mastreg> Mastregs { get; set; }

    public virtual DbSet<Maststate> Maststates { get; set; }

   
    public virtual DbSet<Ncactaken> Ncactakens { get; set; }

    public virtual DbSet<Ncaction> Ncactions { get; set; }

    public virtual DbSet<Ncaudet> Ncaudets { get; set; }

    public virtual DbSet<Ncaudmast> Ncaudmasts { get; set; }

    public virtual DbSet<Ncaumap> Ncaumaps { get; set; }

    public virtual DbSet<Ncbocw> Ncbocws { get; set; }

    public virtual DbSet<Nccontr> Nccontrs { get; set; }

    public virtual DbSet<Ncfile> Ncfiles { get; set; }

   

    public virtual DbSet<Nclocmap> Nclocmaps { get; set; }

    public virtual DbSet<Ncmloc> Ncmlocs { get; set; }

    public virtual DbSet<Ncmlocbo> Ncmlocbos { get; set; }

    public virtual DbSet<Ncmorg> Ncmorgs { get; set; }

    public virtual DbSet<Ncreg> Ncregs { get; set; }

    public virtual DbSet<Ncret> Ncrets { get; set; }

    public virtual DbSet<Nctempcnt> Nctempcnts { get; set; }

    public virtual DbSet<Nctempret> Nctemprets { get; set; }

    public virtual DbSet<Ncumap> Ncumaps { get; set; }

    public virtual DbSet<Ncuser> Ncusers { get; set; }

  
    public virtual DbSet<Statusmaster> Statusmasters { get; set; }

    public virtual DbSet<TrackScope> TrackScopes { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

       


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
            entity.Property(e => e.FileName).IsUnicode(false);
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

   

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
