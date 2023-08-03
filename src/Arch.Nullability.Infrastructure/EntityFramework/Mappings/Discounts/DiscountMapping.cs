using Arch.Nullability.Domain.Discounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arch.Nullability.Infrastructure.EntityFramework.Mappings.Discounts;

public class DiscountMapping : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discounts");

        builder.HasKey("Id");
        builder.Property<int>("Id").IsRequired();

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<RateDiscount>(nameof(RateDiscount))
            .HasValue<WeekendDiscount>(nameof(WeekendDiscount))
            .IsComplete()
            ;

        builder.HasData(
            new RateDiscount(new DiscountId(1), "5% discount for regular customers", 5),
            new RateDiscount(new DiscountId(1), "Fake 10% discount for Black friday", 10),
            new RateDiscount(new DiscountId(1), "Fake 15% discount for Black friday", 15),
            new WeekendDiscount(new DiscountId(1), "Weekend discount", 20)
        );
    }
}

public class RateDiscountMapping : IEntityTypeConfiguration<RateDiscount>
{
    public void Configure(EntityTypeBuilder<RateDiscount> builder)
    {
        builder.Property(t => t.Rate);
    }
}