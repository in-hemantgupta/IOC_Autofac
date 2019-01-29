using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Module = Autofac.Module;

namespace AutofacSamples
{
    interface ICommand
    {
        void Execute();
    }

    class SaveCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Save me");
        }
    }

    class OpenCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("open me");
        }
    }

    class Button
    {
        private ICommand cmd;

        public Button(ICommand cmd)
        {
            this.cmd = cmd;
        }

        public void Click()
        {
            cmd.Execute();
        }
    }

    class Editor
    {
        IEnumerable<Button> btns;
        public Editor(IEnumerable<Button> btns)
        {
            this.btns = btns;
        }
        public void ClickAll()
        {
            foreach (var item in btns)
            {
                item.Click();
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var container = new ContainerBuilder();
            container.RegisterType<SaveCommand>().As<ICommand>().WithMetadata("name", "save clicked");
            container.RegisterType<OpenCommand>().As<ICommand>().WithMetadata("name", "open clicked");
            container.RegisterAdapter<ICommand, Button>(c => new Button(c));
            //container.RegisterAdapter<ICommand, Button>(c => new Button(c));
            container.RegisterType<Editor>();

            using (var b = container.Build())
            {
                var btn = b.Resolve<Editor>();
                btn.ClickAll();
            }
            Console.Read();
        }
    }
}