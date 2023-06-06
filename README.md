# C# Architecture - Nullability
> Simple example project demonstrating if-less programming using null-objects.

Do you prefer "new" `<Nullable>` option to be enabled or disabled? Have you ever heard of if-less programing or null-objects? Maybe you did but you don't use it because?!...

It's sometimes hard to image a real-life usage of null-objects in if-less code, because we get used to use IFs. 
It's part of us to write complicated IF statements for business logic, IF statements to check nullability (in better cases) or any other checks.

Which code looks better for you?

1. This one?
  > `nullable=disable`
  ```csharp
  public OrderCreated CreateOrder(CreateOrder command, IDiscountEventRepository discountEventRepository) {
    if (command == null || command.Items == null) {
      return null;
    }

    var order = new Order();
    var eventDiscounts = discountEventRepository.GetCurrentDiscounts();

    foreach (var item in command.Items) {
      if (item != null) {
        var orderItem = new Item(item.Id, item.Quantity);

        if (command.Dicount != null) {
            orderItem.ApplyDiscount(command.Discount);
        }

        if (eventDiscounts?.Any()) {
          foreach (var discount in eventDiscounts) {
            if (discount != null) {
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

  ```
  
  2. Or this one?
  > `nullable=enable`
  ```csharp
  public OrderCreated CreateOrder(CreateOrder command, IDiscountEventRepository discountEventRepository) {
    var order = new Order();
    var eventDiscounts = discountEventRepository.GetCurrentDiscounts();

    foreach (var item in command.Items) {
      var orderItem = new Item(item.Id, item.Quantity);
      
      orderItem.ApplyDiscount(command.Discount);

      foreach (var discount in eventDiscounts) {
          orderItem.ApplyDiscount(discount);
      }

      order.AddItem(orderItem);
    }
    
    // ... persist
    
    return new OrderCreated(order);
  }
  ```
  
  What we can rewrite to something like this (for those who prefer expensive LINQ):
  
  ```csharp
  public OrderCreated CreateOrder(CreateOrder command, IDiscountEventRepository discountEventRepository) {
    var eventDiscounts = discountEventRepository.GetCurrentDiscounts();
    
    var order = new Order(
      command.Items
        .Select(item => new Item(item.Id, item.Quantity))
        .Select(orderItem => orderItem.ApplyDiscounts(command.Discount, eventDiscounts))
    );
    
    // ... persist
    
    return new OrderCreated(order);
  }
  ```
  
  Yes, to make this "simple" LINQ example I "hacked" the code a little,.. No I don't use it. I just wanted to show some extreme code simplification which is possible thanks to null-objects.
  ```csharp
  public class Discount {
    // ...
    // Implicit operator allowing cast from discount to array of discounts.
    public static implicit operator Discount[] (Discount discount) => new[] { discount };
  }
  
  public class Item {
    // ...
    // Changed signature
    public Item ApplyDiscounts(params Discount[][] discounts) {}
  }
  ```
  
