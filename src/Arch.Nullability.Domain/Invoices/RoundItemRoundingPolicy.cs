namespace Arch.Nullability.Domain.Invoices;

public class RoundItemRoundingPolicy : RoundingPolicy
{
	public RoundItemRoundingPolicy(byte decimals)
		: base(decimals)
	{
	}

	public override decimal RoundItem(decimal price)
	{
		return Math.Round(price, Decimals);
	}

	public override decimal RoundTotal(decimal price)
	{
		return price;
	}
}
