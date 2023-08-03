namespace Arch.Nullability.Application.Orders.QueryOrders;

public class OrderQueryHandler
{
	public async Task<OrderResult> Handle(OrderQuery query)
	{
		Console.WriteLine("query executed");
		return new OrderResult(0);
	}
}
