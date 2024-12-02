using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Configurations;

public class VideoEducationConfiguration : IEntityTypeConfiguration<VideoEducation>
{
    public void Configure(EntityTypeBuilder<VideoEducation> builder)
    {
        builder.ToTable("VideoEducations"); // Tablo adı
        builder.HasKey(v => v.Id); // Primary Key

        builder.Property(v => v.Id).HasColumnName("VideoEducationId"); // Kolon adı
        builder.Property(v => v.Title).HasColumnName("Title").IsRequired().HasMaxLength(200);
        builder.Property(v => v.Description).HasColumnName("Description").HasMaxLength(1000);
        builder.Property(v => v.TotalHour).HasColumnName("TotalHour");
        builder.Property(v => v.IsCertified).HasColumnName("IsCertified");
        builder.Property(v => v.Level).HasColumnName("Level").HasConversion<int>(); // Enum int olarak saklanıyor
        builder.Property(v => v.ImageUrl).HasColumnName("ImageUrl").HasMaxLength(500);
        builder.Property(v => v.InstrutorId).HasColumnName("InstrutorId").IsRequired();
        builder.Property(v => v.ProgrammingLanguage).HasColumnName("ProgrammingLanguage").HasMaxLength(100);

        // İlişki
        builder.HasOne(v => v.Instrutor)
               .WithMany(i => i.VideoEducations)
               .HasForeignKey(v => v.InstrutorId)
               .OnDelete(DeleteBehavior.Cascade);

        // Varsayılan değerler ve diğer ayarlar
        builder.Property(v => v.CreatedDate).HasColumnName("CreatedDate").HasDefaultValueSql("GETDATE()");
    }
}