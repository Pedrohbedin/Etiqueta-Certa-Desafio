using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiEtiqueCerta.Models;

public partial class etiquetaCertaContext : DbContext
{
    public etiquetaCertaContext()
    {
    }

    public etiquetaCertaContext(DbContextOptions<etiquetaCertaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ConservationProcess> ConservationProcesses { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<LabelSymbology> LabelSymbologies { get; set; }

    public virtual DbSet<Legislation> Legislations { get; set; }

    public virtual DbSet<ProcessInLegislation> ProcessInLegislations { get; set; }

    public virtual DbSet<Symbology> Symbologies { get; set; }

    public virtual DbSet<SymbologyTranslate> SymbologyTranslates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = DESKTOP-74E5MF8\\SQLEXPRESS; DataBase = EtiquetaCerta; TrustServerCertificate = true; Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConservationProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conserva__3213E83F0D4F2A9F");

            entity.ToTable("ConservationProcess");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Label__3213E83F9F1A6F16");

            entity.ToTable("Label");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Id_legislation).HasColumnName("id_legislation");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdLegislationNavigation).WithMany(p => p.Labels)
                .HasForeignKey(d => d.Id_legislation)
                .HasConstraintName("FK__Label__id_legisl__49C3F6B7");
        });

        modelBuilder.Entity<LabelSymbology>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LabelSym__3213E83FAEDD0ED8");

            entity.ToTable("LabelSymbology");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.IdLabel).HasColumnName("id_label");
            entity.Property(e => e.IdSymbology).HasColumnName("id_symbology");

            entity.HasOne(d => d.IdLabelNavigation).WithMany(p => p.LabelSymbologies)
                .HasForeignKey(d => d.IdLabel)
                .HasConstraintName("FK__LabelSymb__id_la__4E88ABD4");

            entity.HasOne(d => d.IdSymbologyNavigation).WithMany(p => p.LabelSymbologies)
                .HasForeignKey(d => d.IdSymbology)
                .HasConstraintName("FK__LabelSymb__id_sy__4D94879B");
        });

        modelBuilder.Entity<Legislation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Legislat__3213E83F19F4CB25");

            entity.ToTable("Legislation");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Official_language)
                .HasMaxLength(100)
                .HasColumnName("official_language");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<ProcessInLegislation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProcessI__3213E83FF459A522");

            entity.ToTable("ProcessInLegislation");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.IdLegislation).HasColumnName("id_legislation");
            entity.Property(e => e.IdProcess).HasColumnName("id_process");

            entity.HasOne(d => d.IdLegislationNavigation).WithMany(p => p.ProcessInLegislations)
                .HasForeignKey(d => d.IdLegislation)
                .HasConstraintName("FK__ProcessIn__id_le__5812160E");

            entity.HasOne(d => d.IdProcessNavigation).WithMany(p => p.ProcessInLegislations)
                .HasForeignKey(d => d.IdProcess)
                .HasConstraintName("FK__ProcessIn__id_pr__571DF1D5");
        });

        modelBuilder.Entity<Symbology>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Symbolog__3213E83F419D80AD");

            entity.ToTable("Symbology");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.IdProcess).HasColumnName("id_process");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .HasColumnName("url");

            entity.HasOne(d => d.IdProcessNavigation).WithMany(p => p.Symbologies)
                .HasForeignKey(d => d.IdProcess)
                .HasConstraintName("FK__Symbology__id_pr__3F466844");
        });

        modelBuilder.Entity<SymbologyTranslate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Symbolog__3213E83F2CB0B435");

            entity.ToTable("SymbologyTranslate");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.IdLegislation).HasColumnName("id_legislation");
            entity.Property(e => e.IdSymbology).HasColumnName("id_symbology");
            entity.Property(e => e.Translate)
                .HasMaxLength(100)
                .HasColumnName("symbology_translate");

            entity.HasOne(d => d.IdLegislationNavigation).WithMany(p => p.SymbologyTranslates)
                .HasForeignKey(d => d.IdLegislation)
                .HasConstraintName("FK__Symbology__id_le__534D60F1");

            entity.HasOne(d => d.IdSymbologyNavigation).WithMany(p => p.SymbologyTranslates)
                .HasForeignKey(d => d.IdSymbology)
                .HasConstraintName("FK__Symbology__id_sy__52593CB8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
