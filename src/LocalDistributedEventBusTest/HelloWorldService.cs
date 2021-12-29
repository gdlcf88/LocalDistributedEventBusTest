using System;
using Volo.Abp.DependencyInjection;

namespace LocalDistributedEventBusTest
{
    public class HelloWorldService : ITransientDependency
    {
        public void SayHello()
        {
            Console.WriteLine("\tHello World!");
        }
    }
}
