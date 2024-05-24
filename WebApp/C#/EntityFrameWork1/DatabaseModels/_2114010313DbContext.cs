using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace EntityFrameWork1.DatabaseModels;

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

    public virtual DbSet<Stucourse> Stucourses { get; set; }

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

        modelBuilder.Entity<Stucourse>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("stucourse", tb => tb.HasComment("courses that students have selected"));

            entity.HasIndex(e => e.CourseId, "stucourse_course_CourseID_fk");

            entity.HasIndex(e => e.StudentId, "stucourse_student_StudentID_fk");

            entity.Property(e => e.CourseId)
                .HasMaxLength(20)
                .HasColumnName("CourseID");
            entity.Property(e => e.StudentId)
                .HasMaxLength(20)
                .HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany()
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stucourse_course_CourseID_fk");

            entity.HasOne(d => d.Student).WithMany()
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stucourse_student_StudentID_fk");
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
