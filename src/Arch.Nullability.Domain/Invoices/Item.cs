using Arch.Nullability.Domain.VatRates;

namespace Arch.Nullability.Domain.Invoices;

public record Item(string Name, decimal Price, VatRate VatRate);