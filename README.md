# C# Architecture - Fluent Code

> Simple example project demonstrating if-less programming with null-objects.

Do you prefer the "new" `<Nullable>` option to be enabled or disabled?
Have you ever heard of the if-less programing or the null-objects?
Maybe you did but you don't use it because?!...

It's sometimes hard to image a real-life usage of null-objects and if-less code, because we got used to use IFs.
We just write complicated IF statements for business logic, IF statements to check nullability (in better cases) etc.

## Nullability

Which code snippet do you think looks better?

1. This one?
    > `nullable=disable`
    ```csharp
    public static class CreateOrderHandler
    {
        public static OrderCreated Handle(CreateOrder command, IDiscountEventRepository discountEventRepository)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (discountEventRepository == null)
            {
                throw new ArgumentNullException(nameof(discountEventRepository));
            }

            if (command.Items == null)
            {
                return null;
            }

            var order = new Order();
            var eventDiscounts = discountEventRepository.GetCurrentDiscounts();

            foreach (var item in command.Items)
            {
                if (item != null)
                {
                    var orderItem = new Item(item.Id, item.Quantity);

                    if (command.Dicount != null)
                    {
                        orderItem.ApplyDiscount(command.Discount);
                    }

                    if (eventDiscounts?.Any())
                    {
                        foreach (var discount in eventDiscounts)
                        {
                            if (discount != null)
                            {
                                orderItem.ApplyDiscount(discount);
                            }
                        }
                    }

                    order.AddItem(orderItem);
                }
            }

            // ... persist

            return new OrderCreated(order);
        }
    }
    ```

2. Or this one?
   > `nullable=enable`
    ```csharp
    public static class CreateOrderHandler
    {
        public static OrderCreated CreateOrder(CreateOrder command, IDiscountEventRepository discountEventRepository)
        {
            var order = new Order();
            var eventDiscounts = discountEventRepository.GetCurrentDiscounts();

            foreach (var item in command.Items)
            {
                var orderItem = new Item(item.Id, item.Quantity);

                orderItem.ApplyDiscount(command.Discount);

                foreach (var discount in eventDiscounts)
                {
                    orderItem.ApplyDiscount(discount);
                }

                order.AddItem(orderItem);
            }

            // ... persist

            return new OrderCreated(order);
        }
    }
    ```

   Which we can rewrite to something like this (readability over performance):
    ```csharp
    public static class CreateOrderHandler
    {
        public static OrderCreated Handle(CreateOrder command, IDiscountEventRepository discountEventRepository)
        {
            var eventDiscounts = discountEventRepository.GetCurrentDiscounts();

            var order = new Order(
                command.Items
                    .Select(item => new Item(item.Id, item.Quantity))
                    .Tap(item => item.ApplyDiscounts(command.Discount, eventDiscounts))
            );

            // ... persist

            return new OrderCreated(order);
        }
    }
    ```
   `Tap` is an extension method; like foreach but it returns the original collection. (Name inspired by rxjs' `tap`
   operator 🤷‍♂️)
    ```csharp
    public static class EnumerableExtensions

        public static IEnumerable<TItem> Tap<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            foreach (var item in items)
            {
                action(item);
                yield return item;
            }
        }
    }
    ```
   Changes in code to allow the spread-like syntax of discounts.
    ```csharp
    public class Item {
        // ...
        public void ApplyDiscounts(params Discount[][] discounts) {}
    }

    public class Discount {
        // ...
        // Implicit operator allowing cast from discount to array of discounts.
        public static implicit operator Discount[] (Discount discount) => new[] { discount };
    }
    ```

[//]: # (```csharp)

[//]: # (public record VatRate&#40;int Id, decimal Rate&#41; {)

[//]: # (  public static readonly VatRate None = new VatRate&#40;1, 0&#41;;)

[//]: # (  public static readonly VatRate Low = new VatRate&#40;2, 10&#41;;)

[//]: # (  public static readonly VatRate Mid = new VatRate&#40;3, 15&#41;;)

[//]: # (  public static readonly VatRate High = new VatRate&#40;4, 20&#41;;)

[//]: # ()
[//]: # (  public static VatRate[] GetMembers&#40;&#41; => new [] {)

[//]: # (    None,)

[//]: # (    Low,)

[//]: # (    Mid,)

[//]: # (    High)

[//]: # (  };)

[//]: # (})

[//]: # ()
[//]: # (// TODO: EF showcase)

[//]: # ()
[//]: # (```)
