using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class SubjectCMDContext : DbContext
{
    public DbSet<SubjectDataModel> Subjects { get; set; }
    public SubjectCMDContext(DbContextOptions<SubjectCMDContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubjectDataModel>()
            .OwnsOne(a => a.Description);

        modelBuilder.Entity<SubjectDataModel>()
            .OwnsOne(a => a.Details);

        base.OnModelCreating(modelBuilder);
    }   
}
