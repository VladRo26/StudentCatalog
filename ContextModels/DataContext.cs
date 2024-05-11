using StudentCatalog.Models;
using Microsoft.EntityFrameworkCore;
using Azure.Messaging;
using Org.BouncyCastle.Crypto.Tls;

namespace StudentCatalog.ContextModels;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options){}

    public DbSet<UserModel> Useri {  get; set; }
    public DbSet<GroupModel> Grupe { get; set; }
    public DbSet<CourseModel> Cursuri { get; set; }
    public DbSet<MessagesModel> Mesaje { get; set; }
    public DbSet<StudentModel> Studenti { get; set; }
    public DbSet<StudentCoursesModel> CursuriStudenti { get; set; }

    public DbSet<StudentCertificateModel> Adeverinte { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessagesModel>()
            .HasOne(m => m.Sender)
            .WithMany() // Optionally add navigation property for a collection of sent messages in UserModel
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<MessagesModel>()
            .HasOne(m => m.Receiver)
            .WithMany() // Optionally add navigation property for a collection of received messages in UserModel
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<StudentCoursesModel>()
           .HasOne(m => m.Course)
           .WithMany() // Optionally add navigation property for a collection of received messages in UserModel
           .HasForeignKey(m => m.CourseId)
           .OnDelete(DeleteBehavior.Restrict)
           .IsRequired(false);

        modelBuilder.Entity<StudentCoursesModel>()
              .Property(sc => sc.CourseId)
               .IsRequired(false);


        modelBuilder.Entity<MessagesModel>()
           .HasOne(m => m.Receiver)
           .WithMany() // Optionally add navigation property for a collection of received messages in UserModel
           .HasForeignKey(m => m.ReceiverId)
           .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<CourseModel>()
            .Property(c => c.TeacherId)
            .IsRequired(false);
    }


}
