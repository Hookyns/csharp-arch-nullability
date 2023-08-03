namespace Arch.Nullability.Domain.Invoices;

public record InvoiceId(Guid Id)
{
    public static InvoiceId Null = new(Guid.Empty);
}