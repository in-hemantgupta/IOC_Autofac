using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;

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

    public class Reporting
    {
        Meta<ILog, Settings> _log;
        public Reporting(Meta<ILog, Settings> log)
        {
            _log = log;
        }

        public void Report(string message)
        {
            _log.Value.Write("Logging: " + message);
            //if (_log.Metadata["mode"] as string == "verbose")
            if (_log.Metadata.Mode == "verbose")
            {
                _log.Value.Write("logging in verbose mode");
            }
        }
    }

    public class Settings
    {
        public string Mode { get; set; }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().As<ILog>().WithMetadata<Settings>(s => s.For(x => x.Mode, "verbose"));
            builder.RegisterType<Reporting>();

            using (var container = builder.Build())
            {
                var report = container.Resolve<Reporting>();
                report.Report("this is my first log");
            }

            Console.Read();
        }
    }
}