using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mosaic.Models
{
    public partial class MosaicContext : DbContext
    {
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<Email> Email { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Announcement> Announcement { get; set; }

        public MosaicContext ()
        {

        }
        public MosaicContext(DbContextOptions<MosaicContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=KAELS-LENOVO-YO\KB_SQLSERVER;Initial Catalog=MosaicContext;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.AnnouncementText)
                    .HasColumnName("announcementText")
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ClassCode)
                    .HasColumnName("classCode")
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.ProfUsername)
                    .HasColumnName("profUsername")
                    .IsRequired()
                    .IsUnicode(false);

            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.ClassCode);

                entity.Property(e => e.ClassCode)
                    .HasColumnName("classCode")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasColumnName("className")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MaxEnroll).HasColumnName("maxEnroll");

                entity.Property(e => e.NumEnrolled).HasColumnName("numEnrolled");

                entity.Property(e => e.ProfessorId)
                    .HasColumnName("professorID")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Email>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .IsUnicode(false);

                entity.Property(e => e.Receiver)
                    .IsRequired()
                    .HasColumnName("receiver")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Sender)
                    .IsRequired()
                    .HasColumnName("sender")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassOne)
                    .HasColumnName("classOne")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstName")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("lastName")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(148)
                    .IsUnicode(false);

                entity.HasOne(d => d.ClassOneNavigation)
                    .WithMany(p => p.Professor)
                    .HasForeignKey(d => d.ClassOne)
                    .HasConstraintName("FK_Professor_Class");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassOne)
                    .HasColumnName("classOne")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ClassTwo)
                    .HasColumnName("classTwo")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstName")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("lastName")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(148)
                    .IsUnicode(false);

                entity.HasOne(d => d.ClassOneNavigation)
                    .WithMany(p => p.StudentClassOneNavigation)
                    .HasForeignKey(d => d.ClassOne)
                    .HasConstraintName("FK_User_User");

                entity.HasOne(d => d.ClassTwoNavigation)
                    .WithMany(p => p.StudentClassTwoNavigation)
                    .HasForeignKey(d => d.ClassTwo)
                    .HasConstraintName("FK_User_Class");
            });
        }
    }
}
