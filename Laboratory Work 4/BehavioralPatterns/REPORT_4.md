# Behavioral Design Patterns

**Author:** Patricia Moraru  
**Course:** TMPS (Techniques and Mechanisms of Software Design)  
**Laboratory Work:** #4 - Behavioral Design Patterns

---

## Objectives

1. Study and understand the Behavioral Design Patterns
2. Choose a specific domain for implementation
3. Implement at least 3 Behavioral Design Patterns for the chosen domain
4. Demonstrate the integration and interaction between multiple patterns

---

## Domain

The chosen domain is a **Restaurant Order Management System**. This system manages customer orders in a restaurant environment, handling order creation, kitchen processing, status transitions, and real-time notifications to staff members.

The domain naturally fits behavioral patterns because:

- Orders flow through multiple kitchen stations that process them sequentially (Chain of Responsibility pattern)
- Order operations (prepare, mark ready, serve, cancel) can be encapsulated as commands (Command pattern)
- Multiple staff members need to be notified when order status changes (Observer pattern)
- The system requires flexible communication between objects while maintaining loose coupling

---

## Used Design Patterns

### 1. **Chain of Responsibility Pattern**

Allows an order to pass through a chain of kitchen station handlers (Drink Station → Grill Station → Dessert Station), where each station processes relevant items and passes the order to the next station.

### 2. **Command Pattern**

Encapsulates order operations as command objects (PrepareOrderCommand, MarkReadyCommand, ServeOrderCommand, CancelOrderCommand) that can be executed, queued, or potentially undone.

### 3. **Observer Pattern**

Implements a publish-subscribe mechanism where observers (Waiter, Kitchen Display) are automatically notified whenever an order's status changes, ensuring all staff members stay synchronized.

---

## Implementation

### 1. Chain of Responsibility Pattern

The Chain of Responsibility pattern is implemented through a chain of kitchen station handlers. Each handler processes order items relevant to its station and passes the order to the next handler in the chain.

**Key Classes:**

**Handler.cs** - Abstract handler defining the chain structure:

```csharp
public abstract class Handler
{
    protected Handler? successor;

    public void SetSuccessor(Handler successor)
    {
        this.successor = successor;
    }

    public abstract void Handle(Order order);
}
```

**DrinkStationHandler.cs** - Concrete handler for drinks:

```csharp
public class DrinkStationHandler : Handler
{
    public override void Handle(Order order)
    {
        if (order.Items.Any(i => i.Category == ItemCategory.Drink))
        {
            Console.WriteLine($"[Drink Station] Preparing drinks for order {order.Id}.");
        }

        successor?.Handle(order);
    }
}
```

**GrillStationHandler.cs** - Concrete handler for grilled items:

```csharp
public class GrillStationHandler : Handler
{
    public override void Handle(Order order)
    {
        if (order.Items.Any(i => i.Category == ItemCategory.Grill))
        {
            Console.WriteLine($"[Grill Station] Grilling items for order {order.Id}.");
        }

        successor?.Handle(order);
    }
}
```

**DessertStationHandler.cs** - Concrete handler for desserts:

```csharp
public class DessertStationHandler : Handler
{
    public override void Handle(Order order)
    {
        if (order.Items.Any(i => i.Category == ItemCategory.Dessert))
        {
            Console.WriteLine($"[Dessert Station] Preparing desserts for order {order.Id}.");
        }

        successor?.Handle(order);
    }
}
```

**Chain Setup in OrderService.cs:**

```csharp
public OrderService(OrderRepository repository, OrderNotifier notifier)
{
    this.repository = repository;
    this.notifier = notifier;

    // Build the GoF CoR chain
    var drink = new DrinkStationHandler();
    var grill = new GrillStationHandler();
    var dessert = new DessertStationHandler();

    drink.SetSuccessor(grill);
    grill.SetSuccessor(dessert);

    kitchenChain = drink;
}

public void SendToKitchen(Order order)
{
    Console.WriteLine($"[System] Sending order {order.Id} to kitchen chain.");
    kitchenChain.Handle(order);
}
```

This pattern enables:

- **Dynamic chain configuration**: Stations can be added or removed easily
- **Decoupling**: Each handler only knows about its successor, not the entire chain
- **Flexible processing**: Each station processes only relevant items
- **Easy extension**: New station types can be added without modifying existing ones

**Chain Flow:**

```
Order → DrinkStation → GrillStation → DessertStation → Complete
         (Cola)         (Steak)         (Cake)
         (Lemonade)     (Burger)        (Ice Cream)
```

---

### 2. Command Pattern

The Command pattern encapsulates order operations as objects, allowing parameterization of clients with different requests and supporting queuing, logging, and potentially undoable operations.

**Key Classes:**

**Command.cs** - Abstract command base class:

```csharp
public abstract class Command
{
    protected readonly OrderService service;
    protected readonly Order order;

    protected Command(OrderService service, Order order)
    {
        this.service = service;
        this.order = order;
    }

    public abstract void Execute();
}
```

**Invoker.cs** - Command invoker:

```csharp
// GoF Invoker
public class Invoker
{
    private Command? _command;

    public void SetCommand(Command command)
    {
        _command = command;
    }

    public void Invoke()
    {
        _command?.Execute();
    }
}
```

**Concrete Commands:**

**PrepareOrderCommand.cs:**

```csharp
public class PrepareOrderCommand : Command
{
    public PrepareOrderCommand(OrderService service, Order order)
        : base(service, order)
    {
    }

    public override void Execute()
    {
        service.ChangeStatus(order, OrderStatus.InPreparation);
    }
}
```

**MarkReadyCommand.cs:**

```csharp
public class MarkReadyCommand : Command
{
    public MarkReadyCommand(OrderService service, Order order)
        : base(service, order)
    {
    }

    public override void Execute()
    {
        service.ChangeStatus(order, OrderStatus.Ready);
    }
}
```

**ServeOrderCommand.cs:**

```csharp
public class ServeOrderCommand : Command
{
    public ServeOrderCommand(OrderService service, Order order)
        : base(service, order)
    {
    }

    public override void Execute()
    {
        service.ChangeStatus(order, OrderStatus.Served);
    }
}
```

**CancelOrderCommand.cs:**

```csharp
public class CancelOrderCommand : Command
{
    public CancelOrderCommand(OrderService service, Order order)
        : base(service, order)
    {
    }

    public override void Execute()
    {
        service.ChangeStatus(order, OrderStatus.Cancelled);
    }
}
```

**Usage Example in Program.cs:**

```csharp
case "4":
    if (lastOrder != null)
    {
        invoker.SetCommand(new PrepareOrderCommand(service, lastOrder));
        invoker.Invoke();
    }
    break;

case "5":
    if (lastOrder != null)
    {
        invoker.SetCommand(new MarkReadyCommand(service, lastOrder));
        invoker.Invoke();
    }
    break;
```

This pattern provides:

- **Decoupling**: Invoker doesn't need to know the concrete operation details
- **Extensibility**: New commands can be added without modifying existing code
- **Command queuing**: Commands can be stored and executed later
- **Undo/Redo potential**: Commands can be stored in history for reversible operations
- **Macro commands**: Multiple commands can be grouped and executed as a batch

---

### 3. Observer Pattern

The Observer pattern implements a one-to-many dependency where multiple observers are automatically notified when the subject's state changes. This ensures all restaurant staff members stay synchronized with order status updates.

**Key Classes:**

**OrderNotifier.cs** - Subject (Observable):

```csharp
public delegate void OrderStatusChangedHandler(object sender, OrderStatusChangedEventArgs e);

public class OrderNotifier
{
    // Subject in GoF Observer
    public event OrderStatusChangedHandler? StatusChanged;

    public void NotifyStatusChanged(Order order)
    {
        StatusChanged?.Invoke(this, new OrderStatusChangedEventArgs(order));
    }
}
```

**OrderStatusChangedEventArgs.cs** - Event data:

```csharp
public class OrderStatusChangedEventArgs : EventArgs
{
    public Order Order { get; }

    public OrderStatusChangedEventArgs(Order order)
    {
        Order = order;
    }
}
```

**WaiterObserver.cs** - Concrete observer:

```csharp
public class WaiterObserver
{
    // Observer method
    public void Update(object sender, OrderStatusChangedEventArgs e)
    {
        var order = e.Order;
        if (order.Status == OrderStatus.Ready)
        {
            Console.WriteLine($"[Waiter] Order {order.Id} for table {order.TableNumber} is READY!");
        }
        else
        {
            Console.WriteLine($"[Waiter] Order {order.Id} changed status to {order.Status}.");
        }
    }
}
```

**KitchenDisplayObserver.cs** - Concrete observer:

```csharp
public class KitchenDisplayObserver
{
    public void Update(object sender, OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"[Kitchen Display] Order {e.Order.Id} status: {e.Order.Status}.");
    }
}
```

**Observer Registration in Program.cs:**

```csharp
static void Main()
{
    var repository = new OrderRepository();
    var notifier = new OrderNotifier();

    // Observers
    var waiter = new WaiterObserver();
    var kitchenDisplay = new KitchenDisplayObserver();

    notifier.StatusChanged += waiter.Update;
    notifier.StatusChanged += kitchenDisplay.Update;

    var service = new OrderService(repository, notifier);
    // ...
}
```

**Notification Trigger in OrderService.cs:**

```csharp
public void ChangeStatus(Order order, OrderStatus newStatus)
{
    Console.WriteLine($"[System] Changing status of order {order.Id} from {order.Status} to {newStatus}.");
    order.Status = newStatus;
    notifier.NotifyStatusChanged(order);
}
```

This pattern enables:

- **Loose coupling**: Subject doesn't need to know concrete observer classes
- **Dynamic subscription**: Observers can be added or removed at runtime
- **Broadcast communication**: All observers are notified simultaneously
- **Event-driven architecture**: System reacts to state changes automatically
- **Multiple observers**: Any number of observers can subscribe to the same subject

**Observer Flow:**

```
OrderService.ChangeStatus()
       ↓
OrderNotifier.NotifyStatusChanged()
       ↓
   ┌───┴───┐
   ↓       ↓
Waiter  Kitchen Display
(Update) (Update)
```

---

## Domain Model

### Order.cs - Core domain entity:

```csharp
public class Order
{
    public int Id { get; }
    public int TableNumber { get; }
    public List<OrderItem> Items { get; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.New;

    public Order(int id, int tableNumber)
    {
        Id = id;
        TableNumber = tableNumber;
    }

    public override string ToString()
        => $"Order {Id} | Table {TableNumber} | Status: {Status} | Items: {Items.Count}";
}
```

### OrderItem.cs - Order line item:

```csharp
public class OrderItem
{
    public string Name { get; }
    public ItemCategory Category { get; }
    public int Quantity { get; }

    public OrderItem(string name, ItemCategory category, int quantity)
    {
        Name = name;
        Category = category;
        Quantity = quantity;
    }

    public override string ToString()
        => $"{Quantity} x {Name} ({Category})";
}
```

### OrderStatus.cs - Order lifecycle states:

```csharp
public enum OrderStatus
{
    New,
    InPreparation,
    Ready,
    Served,
    Cancelled
}
```

### ItemCategory.cs - Food categorization:

```csharp
public enum ItemCategory
{
    Drink,
    Grill,
    Dessert,
    Other
}
```

---

## Pattern Integration

One of the key achievements of this implementation is how the three behavioral patterns work together seamlessly to create a cohesive restaurant management system:

```
           ┌──────────────┐
           │   Program    │
           │  (Client)    │
           └──────┬───────┘
                  │
        ┌─────────┴─────────┐
        ↓                   ↓
┌───────────────┐   ┌──────────────┐
│ OrderService  │   │   Invoker    │
│               │   │  (Command)   │
└───────┬───────┘   └──────┬───────┘
        │                  │
        ↓                  ↓
┌──────────────────┐  ┌─────────────┐
│ Kitchen Chain    │  │  Commands   │
│ (CoR Pattern)    │  │  - Prepare  │
│  Drink → Grill   │  │  - Ready    │
│  → Dessert       │  │  - Serve    │
└──────────────────┘  │  - Cancel   │
                      └─────────────┘
        │
        ↓
┌──────────────────┐
│ OrderNotifier    │
│ (Observer)       │
└────────┬─────────┘
         │
    ┌────┴────┐
    ↓         ↓
┌─────────┐ ┌──────────────┐
│ Waiter  │ │Kitchen Display│
└─────────┘ └──────────────┘
```

**Integration Flow Example:**

1. **Client** creates an order and adds items (Cola, Steak, Cake)
2. **Client** sends order to kitchen → triggers **Chain of Responsibility**
   - Drink Station handles Cola
   - Grill Station handles Steak
   - Dessert Station handles Cake
3. **Client** invokes **PrepareOrderCommand** via Invoker
   - Command executes → changes status to InPreparation
   - Status change triggers **Observer Pattern**
   - Waiter and Kitchen Display are notified
4. **Client** invokes **MarkReadyCommand** via Invoker
   - Command executes → changes status to Ready
   - Observers receive notification
   - Waiter displays special "READY!" alert
5. **Client** invokes **ServeOrderCommand** via Invoker
   - Command executes → changes status to Served
   - All observers are synchronized

This integration demonstrates:
- **Separation of concerns**: Each pattern handles a specific responsibility
- **Loose coupling**: Patterns interact through well-defined interfaces
- **Flexibility**: Any pattern can be modified without affecting others
- **Extensibility**: New handlers, commands, or observers can be added easily

---

## System Architecture

```
┌─────────────────────────────────────────────────────────┐
│                       Program.cs                        │
│                   (Main Entry Point)                    │
└──────────────────────┬──────────────────────────────────┘
                       │
         ┌─────────────┼─────────────┐
         ↓             ↓             ↓
┌────────────┐  ┌─────────────┐  ┌──────────────┐
│OrderService│  │   Invoker   │  │OrderNotifier │
│            │  │  (Command)  │  │  (Observer)  │
└─────┬──────┘  └──────┬──────┘  └──────┬───────┘
      │                │                │
      │         ┌──────┴──────┐         │
      │         ↓             ↓         │
      │    ┌─────────┐  ┌──────────┐   │
      │    │Commands │  │Order     │   │
      │    │- Prepare│  │          │   │
      │    │- Ready  │  └──────────┘   │
      │    │- Serve  │                 │
      │    │- Cancel │                 │
      │    └─────────┘                 │
      │                                │
      ↓                                ↓
┌──────────────┐              ┌───────────────┐
│Kitchen Chain │              │   Observers   │
│   (CoR)      │              │- Waiter       │
│- DrinkStation│              │- KitchenDisp. │
│- GrillStation│              └───────────────┘
│- DessertStat.│
└──────────────┘

      ↓
┌──────────────┐
│OrderRepository│
│   (Storage)   │
└──────────────┘
```

---

## Results / Screenshots

### Menu Interface:

```
=== Restaurant Order Management (GoF style) ===
1. Create new order
2. Add demo items to last order
3. Send last order to kitchen (CoR)
4. Prepare last order (Command)
5. Mark last order ready (Command)
6. Serve last order (Command)
7. Cancel last order (Command)
8. List all orders
9. Process order (Kitchen → Prepare → Ready)
0. Exit
Choose option:
```

### Complete Order Flow:

```
>>> Option 1: Create Order
Choose option: 1
Table number: 15
[System] Created Order 1 | Table 15 | Status: New | Items: 0

>>> Option 2: Add Demo Items
Choose option: 2
Demo items added to last order.

>>> Option 3: Send to Kitchen (Chain of Responsibility)
Choose option: 3
[System] Sending order 1 to kitchen chain.
[Drink Station] Preparing drinks for order 1.
[Grill Station] Grilling items for order 1.
[Dessert Station] Preparing desserts for order 1.

>>> Option 4: Prepare Order (Command Pattern)
Choose option: 4
[System] Changing status of order 1 from New to InPreparation.
[Waiter] Order 1 changed status to InPreparation.
[Kitchen Display] Order 1 status: InPreparation.

>>> Option 5: Mark Ready (Command + Observer)
Choose option: 5
[System] Changing status of order 1 from InPreparation to Ready.
[Waiter] Order 1 for table 15 is READY!
[Kitchen Display] Order 1 status: Ready.

>>> Option 6: Serve Order (Command + Observer)
Choose option: 6
[System] Changing status of order 1 from Ready to Served.
[Waiter] Order 1 changed status to Served.
[Kitchen Display] Order 1 status: Served.

>>> Option 8: List All Orders
Choose option: 8
Order 1 | Table 15 | Status: Served | Items: 6
```

### Automated Workflow (Option 9):

```
>>> Option 9: Process Order (Combined Workflow)
Choose option: 9
[Workflow] Starting automated processing...
[System] Sending order 2 to kitchen chain.
[Drink Station] Preparing drinks for order 2.
[Grill Station] Grilling items for order 2.
[Dessert Station] Preparing desserts for order 2.
[System] Changing status of order 2 from New to InPreparation.
[Waiter] Order 2 changed status to InPreparation.
[Kitchen Display] Order 2 status: InPreparation.
[System] Changing status of order 2 from InPreparation to Ready.
[Waiter] Order 2 for table 3 is READY!
[Kitchen Display] Order 2 status: Ready.
[Workflow] Order is now ready for serving!
```

This demonstrates:

- **Chain of Responsibility**: Multiple stations processing the order sequentially
- **Command Pattern**: Each status change is a separate command execution
- **Observer Pattern**: Waiter and Kitchen Display receive real-time notifications
- **Pattern Integration**: All three patterns working together seamlessly

### Multi-Order Management:

```
>>> Managing Multiple Orders
Choose option: 1
Table number: 15
[System] Created Order 1 | Table 15 | Status: New | Items: 0

Choose option: 1
Table number: 3
[System] Created Order 2 | Table 3 | Status: New | Items: 0

Choose option: 8
Order 1 | Table 15 | Status: Served | Items: 6
Order 2 | Table 3 | Status: Cancelled | Items: 6
```

### Order Cancellation:

```
>>> Option 7: Cancel Order
Choose option: 7
[System] Changing status of order 2 from Served to Cancelled.
[Waiter] Order 2 changed status to Cancelled.
[Kitchen Display] Order 2 status: Cancelled.
```

---

## Conclusions

This laboratory work successfully demonstrates the implementation and integration of three behavioral design patterns in a cohesive restaurant order management system:

1. **Chain of Responsibility Pattern** enables flexible request handling by passing orders through a chain of kitchen station handlers. Each station processes relevant items independently and forwards the order to the next station. This pattern provides excellent decoupling and makes the system easily extensible with new station types.

2. **Command Pattern** encapsulates order operations as objects, separating the invoker from the execution logic. This provides flexibility for queuing commands, implementing undo/redo functionality, and creating macro commands. The pattern makes the system highly maintainable and testable by isolating business logic in discrete command objects.

3. **Observer Pattern** implements a publish-subscribe mechanism that automatically notifies all interested parties (waiters, kitchen displays) when order status changes. This ensures real-time synchronization across the system without tight coupling between the subject and observers. New observers can be added dynamically without modifying existing code.

### Key Learnings:

- **Pattern Synergy**: The three patterns complement each other perfectly. Chain of Responsibility handles request routing, Command encapsulates operations, and Observer broadcasts state changes. Together they create a flexible, maintainable system.

- **Loose Coupling**: Behavioral patterns excel at reducing dependencies between objects. The restaurant system demonstrates how senders and receivers of requests can be completely decoupled while maintaining clear communication channels.

- **Real-World Applicability**: These patterns solve genuine problems in restaurant operations—coordinating multiple kitchen stations, managing order workflows, and keeping staff synchronized. The solution is not just academically correct but practically useful.

- **Extensibility**: The system is highly extensible. New kitchen stations can be added to the chain, new commands can be created for additional operations, and new observers can subscribe to order events—all without modifying existing code.

- **GoF Compliance**: The implementation strictly follows Gang of Four design principles, using abstract base classes, proper inheritance hierarchies, and well-defined interfaces that match the canonical pattern structures.

- **C# Integration**: The Observer pattern leverages C#'s native event system (delegates and events), demonstrating how design patterns can be idiomatically adapted to language-specific features while maintaining their core principles.

### Practical Benefits:

- **Maintainability**: Clear separation of concerns makes the codebase easy to understand and modify
- **Testability**: Each pattern component can be tested independently
- **Scalability**: New features can be added without breaking existing functionality
- **Flexibility**: Order processing logic can be changed at runtime
- **Reusability**: Pattern implementations can be adapted to other domains

This laboratory work demonstrates that behavioral design patterns are not just theoretical concepts but powerful tools for building robust, maintainable software systems that model real-world workflows effectively.

