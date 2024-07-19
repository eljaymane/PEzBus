using System.Reflection;
using PEzBus.EventBus.Events;

namespace PEzBus.EventBus.Register.Abstractions
{
    public interface IRegister<TElement, TPriority>
    {
        void Register<T>(T instance);
        void Register<T>(IReadOnlyList<T> instance);
        void Publish(TElement @event, TPriority priority);
    }
}
