using PEzbus.CustomAttributes;
using PEzBus.EventBus.Events;

namespace PEzBus.Benchmark;
 public class UserCreatedEvent : IEvent
    {
        public int Id { get; }
        public string? Argument { get; set; }
        public UserCreatedEvent(int id)
        {
            Id = id;
        }
    }

    public class UserDeletedEvent: IEvent
    {
        public int Id { get; }
        public string? Argument { get; set; }
        public UserDeletedEvent(int id)
        {
            Id = id;
        }
    }

    public class OrderCreatedEvent : IEvent
    {
        public int Id { get; }
        public string Description { get; }

        public OrderCreatedEvent(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }
     public class OrderUpdatedEvent : IEvent
        {
            public int Id { get; }
            public string Description { get; }
    
            public OrderUpdatedEvent(int id, string description)
            {
                Id = id;
                Description = description;
            }
        }

    public sealed class OrderEventsHandler
    {
        public int Id { get; set; }

        public OrderEventsHandler(int id)
        {
            Id = id;
        }

        [Subscribe(typeof(OrderCreatedEvent))]
        public void UserCreated(UserCreatedEvent userCreatedEvent)
        {
            //testEvent.Argument = "voil�";
            //Console.WriteLine($"Handler one : {testEvent.Argument}");
        }

        [Subscribe(typeof(OrderUpdatedEvent))]
        public void UserDeleted(OrderUpdatedEvent testEvent)
        {
            //testEvent.Argument = "voil�";
        }
    }

    public sealed class UserEventsHandler
    {
        public int Id { get; set; }

    public UserEventsHandler(int id)
    {
        Id = id;
    }

    [Subscribe(typeof(UserCreatedEvent))]
    public void UserCreated(UserCreatedEvent userCreatedEvent)
    {
        //testEvent.Argument = "voil�";
        //Console.WriteLine($"Handler one : {testEvent.Argument}");
    }

    [Subscribe(typeof(UserDeletedEvent))]
    public void UserDeleted(UserDeletedEvent testEvent)
    {
        //testEvent.Argument = "voil�";
    }

    [Subscribe(typeof(OrderCreatedEvent))]
    public void UpdateUserBalance(OrderCreatedEvent e)
    {
        
    }
    }