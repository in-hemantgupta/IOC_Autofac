using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Module = Autofac.Module;

namespace AutofacSamples
{
    public interface ILog
    {
        void Write(string message);
    }

    public interface IConsole
    {

    }

    public class ConsoleLog : ILog, IConsole
    {
        public ConsoleLog()
        {
            Console.WriteLine("Creating a console log!");
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailLog : ILog
    {
        private const string adminEmail = "admin@foo.com";

        public void Write(string message)
        {
            Console.WriteLine($"Email sent to {adminEmail} : {message}");
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

        public Engine(ILog log, int id)
        {
            this.log = log;
            this.id = id;
        }

        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }

    public class SMSLog : ILog
    {
        private string phoneNumber;

        public SMSLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS to {phoneNumber} : {message}");
        }
    }

    public class Car
    {
        private Engine engine;
        private ILog log;

        public Car(Engine engine)
        {
            this.engine = engine;
            this.log = new EmailLog();
        }

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

    public class Parent
    {
        public override string ToString()
        {
            return "I am your father";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

        public void SetParent(Parent parent)
        {
            Parent = parent;
        }
    }

    public class ParentChildModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Parent>();
            builder.Register(c => new Child() { Parent = c.Resolve<Parent>() });
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var container = new ContainerBuilder();
            container.RegisterModule<ParentChildModule>();
            var b = container.Build();
            var c =b.Resolve<Child>();
            Console.WriteLine( c.Parent.ToString());
            /* Single Instance */

            //container.RegisterType<ConsoleLog>().As<ILog>().SingleInstance();
            //using (var scope = container.Build().BeginLifetimeScope())
            //{
            //    scope.Resolve<ILog>().Write("hello");
            //    scope.Resolve<ILog>().Write("hello");
            //}

            /* InstancePerLifetimeScope */

            //container.RegisterType<ConsoleLog>().As<ILog>().InstancePerLifetimeScope();
            //var builder = container.Build();
            //using (var scope = builder.BeginLifetimeScope())
            //{
            //    scope.Resolve<ILog>();
            //    using (var scope1 = builder.BeginLifetimeScope())
            //    {
            //        scope1.Resolve<ILog>();
            //    }
            //}

            ///* InstancePerMatchingLifetimeScope */

            //container.RegisterType<ConsoleLog>().As<ILog>().InstancePerMatchingLifetimeScope("foo");
            //var builder = container.Build();
            //using (var scope = builder.BeginLifetimeScope("foo"))
            //{
            //    scope.Resolve<ILog>();
            //    using (var scope1 = scope.BeginLifetimeScope())
            //    {
            //        scope1.Resolve<ILog>();
            //    }
            //}

            /* InstancePerMatchingLifetimeScope */

            //container.RegisterType<ConsoleLog>().As<ILog>().InstancePerRequest("foo");
            //var builder = container.Build();
            //using (var scope = builder.BeginLifetimeScope("foo"))
            //{
            //    scope.Resolve<ILog>();
            //    using (var scope1 = scope.BeginLifetimeScope())
            //    {
            //        scope1.Resolve<ILog>();
            //    }
            //}

            Console.Read();
        }
    }
}