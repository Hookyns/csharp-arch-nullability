namespace Arch.Nullability.Domain.Discounts;

public class WeekendDiscount : RateDiscount
{
	public WeekendDiscount(DiscountId id, string name, decimal rate)
		: base(id, name, rate)
	{
	}

	public override decimal ApplyDiscount(decimal amount)
	{
		if (DateTime.Now.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
		{
			return base.ApplyDiscount(amount);
		}

		return amount;
	}
}
