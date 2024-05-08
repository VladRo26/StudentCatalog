using StudentCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentCatalog.ContextModels;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options){}

    public DbSet<UserModel> Useri {  get; set; }

    public DbSet<CourseModel> Cursuri { get; set; }

    public DbSet<GroupModel> Grupe { get; set; }
}
