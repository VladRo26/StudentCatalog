using StudentCatalog.Models;
using Microsoft.EntityFrameworkCore;
using Azure.Messaging;

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
           .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<MessagesModel>()
           .HasOne(m => m.Receiver)
           .WithMany() // Optionally add navigation property for a collection of received messages in UserModel
           .HasForeignKey(m => m.ReceiverId)
           .OnDelete(DeleteBehavior.ClientSetNull);

    }




}
