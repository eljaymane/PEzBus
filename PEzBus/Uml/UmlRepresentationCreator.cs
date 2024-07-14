namespace PEzBus.Uml;

public class UmlRepresentationCreator<T> : IRepresentationCreator
{
    private readonly IConverter<T,string> _converter;

    public UmlRepresentationCreator(IConverter<T,string> converter)
    {
        _converter = converter;
    }

    public bool Print<T>(IEnumerable<T> data)
    {
        throw new NotImplementedException();
    }
}
