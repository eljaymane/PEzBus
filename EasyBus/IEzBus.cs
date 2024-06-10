using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBus
{
    public interface IEzBus<T> where T : IEzEvent
    {
        void Publish(T @event);
    }
}
