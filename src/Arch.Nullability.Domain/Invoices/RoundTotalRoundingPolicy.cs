namespace Arch.Nullability.Domain.Invoices;

public class RoundTotalRoundingPolicy : RoundingPolicy
{
    public RoundTotalRoundingPolicy(byte decimals)
        : base(decimals)
    {
    }

    public override decimal RoundItem(decimal price)
    {
        return price;
    }

    public override decimal RoundTotal(decimal price)
    {
        return Math.Round(price, Decimals);
    }
}