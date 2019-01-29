using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;

namespace AutofacSamples
{
    interface IOperation
    {
        float Calculate(float a, float b);
    }

    class Addition : IOperation
    {
        public float Calculate(float a, float b)
        {
            return a + b;
        }
    }

    class Multiplication : IOperation
    {
        public float Calculate(float a, float b)
        {
            return a * b;
        }
    }

    class CalculationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Addition>().As<IOperation>();
            builder.RegisterType<Multiplication>().As<IOperation>();
        }

    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json");

            var config = configBuilder.Build();

            var containerBuilder = new ContainerBuilder();
            var configModule = new ConfigurationModule(config);

            containerBuilder.RegisterModule(configModule);

            using (var container = containerBuilder.Build())
            {
                float a = 3, b = 4;
                foreach (IOperation op in container.Resolve<IList<IOperation>>())
                {
                    Console.WriteLine($"{op.GetType().Name} of {a} and {b} = {op.Calculate(a, b)}");
                }
            }

            Console.Read();
        }
    }
}