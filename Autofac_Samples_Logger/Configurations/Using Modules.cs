using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Module = Autofac.Module;

namespace AutofacSamples
{

    public interface IDriver
    {
        void Drive();
    }

    public interface IVehicle
    {
        void Go();
    }

    public class SaneDriver : IDriver
    {

        public void Drive()
        {
            Console.WriteLine("driver safely reached.");
        }
    }

    public class CrazyDriver : IDriver
    {
        public void Drive()
        {
            Console.WriteLine("going very speedy, crashed...");
        }
    }

    class Truck : IVehicle
    {
        private IDriver _driver;

        public Truck(IDriver driver)
        {
            _driver = driver;
        }
        public void Go()
        {
            _driver.Drive();
        }
    }

    public class TransportModule : Autofac.Module
    {
        public bool obaySpeedLimit { get; set; }
        protected override void Load(ContainerBuilder builder)
        {
            if (obaySpeedLimit)
            {
                builder.RegisterType<SaneDriver>().As<IDriver>();
            }
            else {
                builder.RegisterType<CrazyDriver>().As<IDriver>();
            }

            builder.RegisterType<Truck>().As<IVehicle>();
        }

    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            var container = new ContainerBuilder();
            //container.RegisterModule<TransportModule>();
            container.RegisterModule(new TransportModule { obaySpeedLimit = true });
            var build = container.Build();
            var t = build.Resolve<IVehicle>();
            t.Go();
            Console.Read();
        }
    }
}