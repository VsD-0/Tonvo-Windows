using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tonvo.DataBase.Context;

public partial class DbTonvoContext : DbContext
{
    public DbTonvoContext()
    {
    }

    public DbTonvoContext(DbContextOptions<DbTonvoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Applicant> Applicants { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<LevelEducation> LevelEducations { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Responder> Responders { get; set; }

    public virtual DbSet<StatusApplicant> StatusApplicants { get; set; }

    public virtual DbSet<Vacancy> Vacancies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=2,can&trA1xE;database=db_tonvo", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("applicants");

            entity.HasIndex(e => e.CityId, "fk_city");

            entity.HasIndex(e => e.DesiredProfessionId, "fk_desired_profession");

            entity.HasIndex(e => e.EducationId, "fk_education");

            entity.HasIndex(e => e.StatusId, "fk_status_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("birth_date");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.DesiredProfessionId).HasColumnName("desired_profession_id");
            entity.Property(e => e.DesiredSalary)
                .HasPrecision(14, 2)
                .HasColumnName("desired_salary");
            entity.Property(e => e.EducationId).HasColumnName("education_id");
            entity.Property(e => e.Email)
                .HasMaxLength(257)
                .HasColumnName("email");
            entity.Property(e => e.Experience)
                .HasDefaultValueSql("'0'")
                .HasColumnName("experience");
            entity.Property(e => e.Information)
                .HasColumnType("text")
                .HasColumnName("information");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .HasColumnName("patronymic");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(80)
                .HasColumnName("surname");

            entity.HasOne(d => d.City).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_city");

            entity.HasOne(d => d.DesiredProfession).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.DesiredProfessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_desired_profession");

            entity.HasOne(d => d.Education).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.EducationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_education");

            entity.HasOne(d => d.Status).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_status_id");

            entity.HasMany(d => d.Vacancies).WithMany(p => p.Applicants)
                .UsingEntity<Dictionary<string, object>>(
                    "Favorite",
                    r => r.HasOne<Vacancy>().WithMany()
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_favorite_vacancy_id"),
                    l => l.HasOne<Applicant>().WithMany()
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_favorite_applicant_id"),
                    j =>
                    {
                        j.HasKey("ApplicantId", "VacancyId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("favorites");
                        j.HasIndex(new[] { "VacancyId" }, "fk_favorite_vacancy_id_idx");
                        j.IndexerProperty<int>("ApplicantId").HasColumnName("applicant_id");
                        j.IndexerProperty<int>("VacancyId").HasColumnName("vacancy_id");
                    });
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cities");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City1)
                .HasMaxLength(45)
                .HasColumnName("city");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("companies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(257)
                .HasColumnName("email");
            entity.Property(e => e.Information)
                .HasColumnType("text")
                .HasColumnName("information");
            entity.Property(e => e.NameCompany)
                .HasMaxLength(50)
                .HasColumnName("name_company");
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<LevelEducation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("level_education");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Education)
                .HasMaxLength(80)
                .HasColumnName("education");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("professions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        modelBuilder.Entity<Responder>(entity =>
        {
            entity.HasKey(e => new { e.ApplicantId, e.VacancyId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("responders");

            entity.HasIndex(e => e.VacancyId, "fk_vacancy_id");

            entity.Property(e => e.ApplicantId).HasColumnName("applicant_id");
            entity.Property(e => e.VacancyId).HasColumnName("vacancy_id");
            entity.Property(e => e.RespondDate)
                .HasColumnType("datetime")
                .HasColumnName("respond_date");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");

            entity.HasOne(d => d.Applicant).WithMany(p => p.Responders)
                .HasForeignKey(d => d.ApplicantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_applicant_id");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.Responders)
                .HasForeignKey(d => d.VacancyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vacancy_id");
        });

        modelBuilder.Entity<StatusApplicant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("status_applicant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(80);
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vacancies");

            entity.HasIndex(e => e.CompanyId, "fk_company_id");

            entity.HasIndex(e => e.ProfessionId, "fk_profession");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.DesiredExperience)
                .HasDefaultValueSql("'0'")
                .HasColumnName("desired_experience");
            entity.Property(e => e.Information)
                .HasColumnType("text")
                .HasColumnName("information");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.ProfessionId).HasColumnName("profession_id");
            entity.Property(e => e.Salary)
                .HasMaxLength(50)
                .HasColumnName("salary");
            entity.Property(e => e.СreationDate)
                .HasColumnType("datetime")
                .HasColumnName("сreation_date");

            entity.HasOne(d => d.Company).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_company_id");

            entity.HasOne(d => d.Profession).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.ProfessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profession");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
