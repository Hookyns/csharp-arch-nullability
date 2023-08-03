namespace Arch.Nullability.Domain.Invoices;

public abstract class RoundingPolicy
{
	protected byte Decimals { get; }

	protected RoundingPolicy(byte decimals)
	{
		Decimals = decimals;
	}

	public abstract decimal RoundItem(decimal price);
	public abstract decimal RoundTotal(decimal price);
}
