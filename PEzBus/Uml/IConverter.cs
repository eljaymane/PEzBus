namespace PEzBus.Uml;

public interface IConverter <T,Out>
{
    Out Convert<T>(IEnumerable<T> objects);
    Out Convert<T>(T @object);
}