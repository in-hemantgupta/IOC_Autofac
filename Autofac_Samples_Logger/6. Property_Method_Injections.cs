using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;

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

    internal class Program
    {
        public delegate void MyDelegate(string s);
        [Obsolete("",)
        public static void s1(string s) { Console.
                WriteLine(s); }
        public static void s2(string s) { Console.WriteLine(s); }
        public static void s3(out int i) { i = 6; }


        public static void Main(string[] args)
        {
            MyDelegate m1;
            int i;
            s3(out i);

            m1 = s1;
            m1 += s2;
            m1 += s3;
            m1("hello");
            return;
            var builder = new ContainerBuilder();
            builder.RegisterType<Parent>();

            //builder.RegisterType<Child>().PropertiesAutowired();


            //      builder.RegisterType<Child>()
            //        .WithProperty("Parent", new Parent());

            //      builder.Register(c =>
            //      {
            //        var child = new Child();
            //        child.SetParent(c.Resolve<Parent>());
            //        return child;
            //      });

            //builder.RegisterType<Child>()
            //  .OnActivated((IActivatedEventArgs<Child> e) =>
            //  {
            //      var p = e.Context.Resolve<Parent>();
            //      e.Instance.SetParent(p);
            //  });
            builder.Register(c => { var ch = new Child(); ch.SetParent(c.Resolve<Parent>()); return ch; });
            builder.RegisterType<Child>().OnActivated(c => { c.Instance.Parent = c.Context.Resolve<Parent>(); });

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent);

            Console.Read();
        }
    }
}