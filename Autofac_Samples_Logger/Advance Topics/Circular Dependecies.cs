
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


    public class ChildWithProp
    {
        public ParentWithProp parent { get; set; }

        public override string ToString()
        {
            return "Child";
        }
    }

    public class ParentWithProp
    {
        public ChildWithProp child { get; set; }
        public override string ToString()
        {
            return "Parent";
        }
    }

    public class ParentWithConstructor
    {
        public ChildWithProp1 Child { get; set; }

        public ParentWithConstructor(ChildWithProp1 ch)
        {
            if (ch == null)
                throw new NotImplementedException();
            Child = ch;
        }

        public override string ToString()
        {
            return "Parent with child prop";
        }
    }


    public class ChildWithProp1
    {
        public ParentWithConstructor Parent { get; set; }


        public override string ToString()
        {
            return "child";
        }
    }

    static void Main(string[] args)
    {
        var b = new ContainerBuilder();

        b.RegisterType<ParentWithConstructor>().InstancePerLifetimeScope();
        b.RegisterType<ChildWithProp1>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);


        b.RegisterType<ParentWithProp>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        b.RegisterType<ChildWithProp>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

        using (var c = b.Build())
        {
            //            Console.WriteLine(c.Resolve<ParentWithConstructor>().Child.Parent);
            Console.WriteLine(c.Resolve<ParentWithProp>().child.parent.child);
        }

        Console.Read();
    }
}