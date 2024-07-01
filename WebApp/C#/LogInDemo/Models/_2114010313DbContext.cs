using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace LogInDemo.Models;

public partial class _2114010313DbContext : DbContext
{
    public _2114010313DbContext()
    {
    }

    public _2114010313DbContext(DbContextOptions<_2114010313DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Stucourse1> Stucourse1s { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("persist security info=True;data source=rm-bp1tg219t7o5j5ex8fo.rwlb.rds.aliyuncs.com;port=3306;initial catalog=2114010313_db;user id=2114010313;password=2114010313;character set=utf8;allow zero datetime=true;convert zero datetime=true;pooling=true;maximumpoolsize=3000", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PRIMARY");

            entity.ToTable("course");

            entity.Property(e => e.CourseId)
                .HasMaxLength(20)
                .HasColumnName("CourseID");
            entity.Property(e => e.CourseName).HasMaxLength(10);
        });

        modelBuilder.Entity<Stucourse1>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("stucourse1");

            entity.Property(e => e.StudentId)
                .HasMaxLength(20)
                .HasColumnName("StudentID");
            entity.Property(e => e.CourseId)
                .HasMaxLength(20)
                .HasColumnName("courseID");
            entity.Property(e => e.CourseName).HasMaxLength(10);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.ToTable("student");

            entity.Property(e => e.StudentId)
                .HasMaxLength(20)
                .HasColumnName("StudentID");
            entity.Property(e => e.InitPassword).HasMaxLength(10);
            entity.Property(e => e.StudentClass).HasMaxLength(10);
            entity.Property(e => e.StudentName).HasMaxLength(10);

            entity.HasMany(d => d.Courses).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "Stucourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("stucourse_course_CourseID_fk"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("stucourse_student_StudentID_fk"),
                    j =>
                    {
                        j.HasKey("StudentId", "CourseId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("stucourse");
                        j.HasIndex(new[] { "CourseId" }, "stucourse_course_CourseID_fk");
                        j.IndexerProperty<string>("StudentId")
                            .HasMaxLength(20)
                            .HasColumnName("studentID");
                        j.IndexerProperty<string>("CourseId")
                            .HasMaxLength(20)
                            .HasColumnName("courseID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
