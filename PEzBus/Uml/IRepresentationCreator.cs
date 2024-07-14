namespace PEzBus.Uml;

public interface IRepresentationCreator
{
    public bool Print<T>(IEnumerable<T> data);
}