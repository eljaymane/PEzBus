// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using PEzBus.Benchmark;
using PEzBus.EventBus.Queue;
using PEzBus.Uml;

// using PEzBus;
// using PEzBus.Benchmark;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

var eventBus = new PEzBus.EventBus.PEzBus();





   eventBus.Register(new UserEventsHandler(5));
    eventBus.Register(new OrderEventsHandler(4));


 var converter = new EventsToUmlConverter();

var uml = converter.Convert(eventBus.InstanceRegister);
Console.WriteLine(uml);
 public class PEzEventBus
 {
 }