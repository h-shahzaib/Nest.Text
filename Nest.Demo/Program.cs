using Nest.Text;
using System;

// dotnet nuget push "C:\Users\hassasha\source\repos\Nest.Text\Nest.Text\bin\Release\Nest.Text.1.0.7.nupkg" --source https://api.nuget.org/v3/index.json --api-key $env:NUGET_API_KEY

// TODO: Add a method to get a new instance of ITextBuilder/IChainBuilder inherited from some other text builder
// TODO: Add a way to do .Append directly on ITextBuilder/IChainBuilder instead of an Action of those
// TODO: Write a method to get IChainBuilder out of ITextBuilder instead of having of write .L() to get access

namespace Nest.Demo
{
    internal class Program
    {
        static void Main()
        {
            var _ = TextBuilder.Create();
            _.Options.BlockStyle = BlockStyle.CurlyBraces;
            _.Options.IndentSize = 4;
            _.Options.IndentChar = ' ';
            _.Options.LineBreak = "\r\n";
            _.Options.RegisterCharReplacement('`', ' ');

            var chain1 = _.GetChainBuilder();
            chain1.L("[TestSpace]");
            chain1.L("namespace Generated").B(_ =>
            {
                _.L("public static class Hello").B(_ =>
                {
                    _.L("public static void SayHi() => Console.WriteLine(`Hello from the generator!`);");
                });
            });

            var chain2 = _.GetChainBuilder();
            chain2.L("[TestSpace]");
            chain2.L("namespace Generated").B(_ =>
            {
                _.L("public static class Hello").B(_ =>
                {
                    _.L("public static void SayHi() => Console.WriteLine(`Hello from the generator!`);");
                });
            });

            Console.WriteLine(_.ToString());
        }
    }
}
