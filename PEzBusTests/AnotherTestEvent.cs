using PEzBus.EventBus.Events;

namespace PEzBusTest;

public class AnotherTestEvent: IEvent
{
    public string Args { get; set; }
    public AnotherTestEvent(string args)
    {
        Args = args;
    }
}