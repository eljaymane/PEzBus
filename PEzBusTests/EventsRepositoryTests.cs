using PEzBus.EventBus.Events;
using PEzBus.EventBus.Repository;
using PEzBus.EventsPubSub.Repository;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PEzBusTest;

public class EventsRepositoryTests
{
    
    [Fact]
    public void Should_Get_all_matching_references_for_event()
    {
        var repository = new EventsRepository();
        var handlerInstance1 = new TestHandler("1");
        var handlerInstance2 = new TestHandler("2");
        var handlerInstance3 = new TestHandler("3"); 
        repository.Register([handlerInstance1, handlerInstance2,handlerInstance3]);
        var testEvent = new TestEvent();

        var matchingReferences = repository.GetMatchingMethodsAndTargets(testEvent);
        //Each of the TestHandler class instances have 1 method to handle events (1*3)
        Assert.AreEqual(matchingReferences.Count(), 3 );
    }
}