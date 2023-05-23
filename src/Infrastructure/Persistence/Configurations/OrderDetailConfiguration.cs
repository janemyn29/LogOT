using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mentor_v1.Infrastructure.Persistence.Configurations;
/*public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x=>x.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x=>x.ClassCourse)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.ClassCourseId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}*/
