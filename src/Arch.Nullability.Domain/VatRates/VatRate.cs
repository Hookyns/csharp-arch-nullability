namespace Arch.Nullability.Domain.VatRates;

public class VatRate
{
    public static readonly VatRate None = new(1, 0);
    public static readonly VatRate Low = new(2, 10);
    public static readonly VatRate Mid = new(3, 15);
    public static readonly VatRate High = new(4, 20);

    public static VatRate[] GetMembers() => new [] {
        None,
        Low,
        Mid,
        High
    };

    private int Id { get; init; }

    /// <summary>
    /// VAT rate as % (decimal number).
    /// </summary>
    public decimal Rate { get; private init; }

    private VatRate(int id, decimal rate)
    {
        Id = id;
        Rate = rate;
    }

    /// <summary>
    /// Apply VAT to given <see cref="amount"/> and return total amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public decimal ApplyVat(decimal amount)
    {
        return amount + amount * Rate;
    }
}