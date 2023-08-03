namespace Arch.Nullability.Domain.Invoices;

public class Invoice
{
    private readonly List<Discount> _discounts = new ();
    private readonly List<Item> _items = new();

    public InvoiceId Id { get; private init; } = InvoiceId.Null;

    public IReadOnlyList<Discount> Discounts => _discounts.AsReadOnly();
    public IReadOnlyList<Item> Items => _items.AsReadOnly();

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public void AddDiscount(Discount discount)
    {
        _discounts.Add(discount);
    }
}