using PEzBus.EventBus.Events;
using PEzBus.EventBus.Register;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PEzBusTest;

public class InstancesRegisterTests
{
    
    [Fact]
    public void Should_Get_All_Valid_Registered_Instances_for_event()
    {
        var repository = new InstancesRegister();
        var handlerInstance1 = new TestHandler("1");
        var handlerInstance2 = new TestHandler("2");
        var handlerInstance3 = new TestHandler("3"); 
        repository.Register([handlerInstance1, handlerInstance2,handlerInstance3]);
        var testEvent = new TestEvent();

        var matchingReferences = repository.GetValidInstances(testEvent, x => x.IsAlive);
        //Each of the TestHandler class instances have 1 method to handle events (1*3)
        Assert.AreEqual(matchingReferences.Count(), 3 );
    }
}