namespace Arch.Nullability.Domain.Discounts;

public class RateDiscount : Discount
{
	/// <summary>
	/// Discount rate as % (decimal number).
	/// </summary>
	public decimal Rate { get; private init; }

	public RateDiscount(DiscountId id, string name, decimal rate)
		: base(id, name)
	{
		Rate = rate;
	}

	public override decimal ApplyDiscount(decimal amount)
	{
		return Math.Max(0, amount - amount * Rate);
	}
}
