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
            Console.WriteLine("ConsoleLog "+ message);
        }
    }


    public class ConsoleLog1 : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine("ConsoleLog1 "+ message);
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
        private ILog logs;

        //public Car(Engine engine, IEnumerable<ILog> logs)
        public Car(Engine engine, ILog logs)
        {
            this.engine = engine;
            this.logs = logs;
        }

        public void Go()
        {
            engine.Ahead(100);
            //foreach (var log in logs)
            //{
            logs.Write("Car going forward...");
            //}
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {

            var builder = new ContainerBuilder();
            //AUtofac register last register type as default, if we want to change that add PreserveExistingDefaults so that previous default will not be changed

            builder.RegisterType<ConsoleLog1>().As<ILog>().AsSelf();//.PreserveExistingDefaults();
            builder.RegisterType<ConsoleLog>().As<ILog>();//.AsSelf();
            builder.RegisterType<Engine>();
            //builder.RegisterType<Car>();
            //builder.RegisterType<Car>().UsingConstructor(typeof(Engine), typeof(ConsoleLog1));
            builder.Register<Car>(c=> new Car(c.Resolve<Engine>(), c.Resolve<ConsoleLog1>()));
            IContainer container = builder.Build();

            //var log = container.Resolve<ILog>();
            //log.Write("test");
            var car = container.Resolve<Car>();
            car.Go();

            Console.ReadLine();
        }
    }
}