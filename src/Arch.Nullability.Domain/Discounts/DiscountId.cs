namespace Arch.Nullability.Domain.Discounts;

public record DiscountId(int Id)
{
    public static DiscountId Null = new(0);
}