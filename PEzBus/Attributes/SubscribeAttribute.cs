namespace PEzbus.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class SubscribeAttribute : Attribute
    {
        public Type Event { get; }
        public SubscribeAttribute(Type @event)
        {
            Event = @event;
        }
    }
}
