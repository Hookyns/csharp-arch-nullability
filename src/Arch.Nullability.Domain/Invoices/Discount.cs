using Arch.Nullability.Domain.Discounts;

namespace Arch.Nullability.Domain.Invoices;

/// <summary>
/// Invoice discount item.
/// </summary>
/// <param name="Name">Name of the discount.</param>
/// <param name="Rate">Discount rate</param>
/// <param name="DiscountId">Reference to the original </param>
public record Discount(string Name, decimal Rate, DiscountId DiscountId);