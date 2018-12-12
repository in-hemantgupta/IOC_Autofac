using System;
using System.Collections.Generic;
using Autofac;

namespace AutofacSamples
{
    public interface ILog
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }


    public class ConsoleLog1 : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine("hello" + message);
        }
    }


    public class Engine
    {
        private ILog log;
        private int id;

        public Engine(ILog log)
        {
            this.log = log;
            id = new Random().Next();
        }

        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }

    public class Car
    {
        private Engine engine;
        private ILog log;

        public Car(Engine engine, ILog log)
        {
            this.engine = engine;
            this.log = log;
        }

        public void Go()
        {
            engine.Ahead(100);
            log.Write("Car going forward...");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {

            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().As<ILog>().AsSelf();

            //AUtofac register last register type as default, if we want to change that add PreserveExistingDefaults so that previous default will not be changed
            builder.RegisterType<ConsoleLog1>().As<ILog>().PreserveExistingDefaults();
            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            var log = container.Resolve<ILog>();
            log.Write("test");
            var car = container.Resolve<Car>();
            car.Go();

            Console.ReadLine();
        }
    }
}