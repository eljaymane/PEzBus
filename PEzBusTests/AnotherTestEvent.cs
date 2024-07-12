using PEzbus;

namespace PEzBusTest;

public class AnotherTestEvent: IPEzEvent
{
    public string Args { get; set; }
    public AnotherTestEvent(string args)
    {
        Args = args;
    }
}