using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEzbus
{
    public interface IPEzEventBus
    {
        void Register<T>(T instance);
        void Publish(IPEzEvent @event);

    }
}
