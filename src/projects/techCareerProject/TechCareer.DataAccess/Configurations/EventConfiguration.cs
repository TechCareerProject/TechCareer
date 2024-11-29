using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events").HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("EventId");
            builder.Property(c => c.Title).HasColumnName("Title");
            builder.Property(c => c.Description).HasColumnName("Description");
            builder.Property(c => c.ImageUrl).HasColumnName("ImageUrl");
            builder.Property(c => c.StartDate).HasColumnName("StartDate");
            builder.Property(c => c.EndDate).HasColumnName("EndDate");
            builder.Property(c => c.ApplicationDeadLine).HasColumnName("ApplicationDeadLine");
            builder.Property(c => c.ParticipationText).HasColumnName("ParticipationText");
            builder.Property(X => X.CategoryId).HasColumnName("CategoryId");

            builder.HasOne(x => x.Category).WithMany(x => x.Events).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);

            builder.Navigation(x => x.Category).AutoInclude();

        }
    }
}
