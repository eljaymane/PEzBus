namespace EasyBus
{
    public interface IEventHandler<T> where T : IEvent
    {
        void handle(T @event);
    }
}
