using System.Threading;
namespace PEzBus.Util;

 internal static class BackgroundThread
{

    public static Thread Start(ThreadStart startAction, Action? abortAction = null)
    {
        var thread = new Thread(startAction)
        {
            IsBackground = true
        };
        
        thread.Start();
        return thread;
    }
    
    private static ParameterizedThreadStart Wrapper<T>(Action<T> action, Action? abortAction)
    {
        return s =>
        {
            try
            {
                action((T)s!);
            }
            catch (ThreadAbortException ex)
            {
                if (abortAction != null)
                    SafeAbort(abortAction);

                (ex.ExceptionState as EventWaitHandle)?.Set();
            }
            catch (Exception ex)
            {
                
            }
        };
    }

    private static void SafeAbort(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            
        }
    }

    private static ThreadStart Wrapper(ThreadStart action, Action? abortAction)
    {
        return () =>
        {
            try
            {
                action();
            }
            catch (ThreadAbortException ex)
            {
                if (abortAction != null)
                    SafeAbort(abortAction);
            }
        };
    }
}