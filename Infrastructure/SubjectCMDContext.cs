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
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SubjectDataModel>(builder =>
        {
            builder.OwnsOne(s => s.Description, d =>
            {
                d.Property(p => p.Value)
                    .HasColumnName("Description") // optional: name the DB column
                    .IsRequired();
            });

            builder.OwnsOne(s => s.Details, d =>
            {
                d.Property(p => p.Value)
                    .HasColumnName("Details")
                    .IsRequired();
            });
        });
    }   
}
