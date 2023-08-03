namespace Arch.Nullability.Domain.Discounts;

public abstract class Discount
{
	public DiscountId Id { get; private init; }

	public string Name { get; private init; }

	protected Discount(DiscountId id, string name)
	{
		Id = id;
		Name = name;
	}

	/// <summary>
	/// Apply VAT to given <see cref="amount"/> and return total amount.
	/// </summary>
	/// <param name="amount"></param>
	/// <returns></returns>
	public abstract decimal ApplyDiscount(decimal amount);
}
