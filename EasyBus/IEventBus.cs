using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBus
{
    public interface IEventBus<T> where T : IEvent
    {
        void Publish(T @event);
    }
}
