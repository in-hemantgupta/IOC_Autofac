using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace IOC_autofac
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

    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
