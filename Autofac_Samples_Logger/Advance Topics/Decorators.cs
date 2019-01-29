
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using Autofac;

class Solution
{

    public interface IReportingService
    {
        void Report();

    }

    public class ReportingService : IReportingService
    {
        public void Report()
        {
            Console.WriteLine("Here is your report");
        }
    }

    public class ReportingServiceWithLogging : IReportingService
    {
        IReportingService decorated;
        public ReportingServiceWithLogging(IReportingService reportingService)
        {
            decorated = reportingService;
        }

        public void Report()
        {
            Console.WriteLine("Commencing log...");
            decorated.Report();
            Console.WriteLine("Ending log...");
        }
    }

    static void Main(string[] args)
    {
        var b = new ContainerBuilder();
        b.RegisterType<ReportingService>().Named<IReportingService>("reporting");

        //b.RegisterDecorator<IReportingService>(
        //    (ctx, service) => 
        //    new ReportingServiceWithLogging(service), "reporting"
        //    );

        b.RegisterType<ReportingServiceWithLogging>().As<IReportingService>();
           
        using (var s = b.Build()) {
            var l = s.Resolve<IReportingService>();
            l.Report();
        }

        Console.Read();
    }
}
