namespace PEzBus.Uml;

public interface IConverter <T,Out>
{
    Out Convert(IEnumerable<T> objects);
    Out Convert(T @object);
}