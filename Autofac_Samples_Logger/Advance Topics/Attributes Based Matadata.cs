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
using Autofac.Features.AttributeFilters;
using Autofac;
using Autofac.Extras.AttributeMetadata;

public class AgeMataDataAttribute : Attribute
{
    public int Age { get; set; }

    public AgeMataDataAttribute(int age)
    {
        Age = age;
    }
}

public interface IArtWork
{
    void Display();
}

[AgeMataData(100)]
public class CenturyArtwork : IArtWork
{
    public void Display()
    {
        Console.WriteLine("diplaying century old piece");
    }
}

[AgeMataData(1000)]
public class MillanialArt : IArtWork
{
    public void Display()
    {
        Console.WriteLine("dipalaying miallains");
    }
}

public class ArtDisplay
{
    public IArtWork artwork;

    public ArtDisplay([MetadataFilter("Age", 100)] IArtWork artWork)
    {
        artwork = artWork;
    }

    public void Display()
    {
        artwork.Display();
    }
}

public class Solution
{

    static void Main(string[] args)
    {
        var b = new ContainerBuilder();

        b.RegisterModule<AttributedMetadataModule>();

        b.RegisterType<CenturyArtwork>().As<IArtWork>();
        //b.RegisterType<MillanialArt>().As<IArtWork>();

        b.RegisterType<ArtDisplay>().WithAttributeFiltering();

        using (var c = b.Build())
        {
            var x = c.Resolve<IArtWork>();
            x.Display();
            c.Resolve<ArtDisplay>().Display();

        }
        Console.Read();
    }
}
