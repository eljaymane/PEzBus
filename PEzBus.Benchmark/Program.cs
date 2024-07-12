// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
// using PEzBus;
// using PEzBus.Benchmark;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

// var eventBus = new PEzEventBus();
// var handler = new TestEventHandler();
// eventBus.Register(handler);
//
//
// for(int i =0; i < 5_000; i++)
//     eventBus.Publish(new TestEvent(i));