using Nest.Text;
using System;

namespace Nest.Demo
{
    internal class Program
    {
        static void Main()
        {
            var _ = TextBuilder.Create();
            _.Options.BlockStyle = BlockStyle.Braces;
            _.Options.IndentSize = 4;

            _.L("using System;");

            _.L("[TestSpace]")
            .L("namespace Generated").B(static _ =>
            {
                _.L("public static class Hello").B(static _ =>
                {
                    _.L("public static void SayHi() => Console.WriteLine(`Hello from the generator!`);");
                },
                o => o.BlockStyle = BlockStyle.IndentOnly);
            });

            Console.WriteLine(_.ToString());
        }
    }
}
