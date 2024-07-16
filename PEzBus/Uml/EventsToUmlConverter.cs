using System.Collections.Concurrent;
using System.Text;
using PEzBus.EventBus.Events;
using PEzBus.EventBus.Register;

namespace PEzBus.Uml;

public class EventsToUmlConverter : IConverter<ConcurrentDictionary<Type,List<InstanceInfos>>,string>
{
    public string Convert(IEnumerable<ConcurrentDictionary<Type, List<InstanceInfos>>> objects)
    {
        throw new NotImplementedException();
    }
    public string Convert(ConcurrentDictionary<Type, List<InstanceInfos>> @object)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("@startuml\n");
        stringBuilder.Append("nwdiag { \n");
        foreach (var entry in @object)
        {
            stringBuilder.Append($"network {entry.Key.Name} {{ \n");
            
            foreach (var instanceInfo in entry.Value.GroupBy(x => x.Target?.GetType().Name).Select(x => x.First()))
            {
                stringBuilder.Append($"{instanceInfo.Target?.GetType().Name}.{instanceInfo.Method.Name};\n");
            }
            
            stringBuilder.Append("}\n");
        }
        
        stringBuilder.Append("}\n");
        stringBuilder.Append("@enduml");

        return stringBuilder.ToString();
    }
}