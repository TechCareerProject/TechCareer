using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("EventId");
            builder.Property(c => c.Title).HasColumnName("Title").IsRequired();
            builder.Property(c => c.Description).HasColumnName("Description");
            builder.Property(c => c.ImageUrl).HasColumnName("ImageUrl");
            builder.Property(c => c.StartDate).HasColumnName("StartDate").IsRequired();
            builder.Property(c => c.EndDate).HasColumnName("EndDate").IsRequired();
            builder.Property(c => c.ApplicationDeadLine).HasColumnName("ApplicationDeadLine").IsRequired();
            builder.Property(c => c.ParticipationText).HasColumnName("ParticipationText");
            builder.Property(c => c.CategoryId).HasColumnName("CategoryId").IsRequired();

<<<<<<< HEAD
            
            builder.HasOne(e => e.Category)
                   .WithMany(c => c.Events)
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction);
=======
            builder.HasOne(x => x.Category).WithMany(x => x.Event).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);

            builder.Navigation(x => x.Category).AutoInclude();
>>>>>>> 716439d72e0a58805e01a0ae2e996290e82d92e6

            
            builder.Navigation(e => e.Category).AutoInclude();
        }
    }
}