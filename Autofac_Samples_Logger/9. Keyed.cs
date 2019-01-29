using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Autofac.Features.Indexed;
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
        IIndex<string, ILog> _log;
        public Reporting(IIndex<string, ILog> log)
        {
            _log = log;
        }

        public void Report(string message)
        {
            _log["sms"].Write(message);
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
            builder.RegisterType<ConsoleLog>().Keyed<ILog>("cmd");
            builder.Register(c=> new SMSLog("123456")).Keyed<ILog>("sms");
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