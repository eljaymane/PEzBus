using PEzBus.EventBus.Repository;

namespace PEzBus.Uml;

public class EventsMethodsConverter : IConverter<ReferenceInfo,string>
{
    public string Convert<T>(IEnumerable<T> objects)
    {
        return string.Empty;
    }

    public string Convert<T>(T @object)
    {
        throw new NotImplementedException();
    }
}