using Nest.Text;
using System;

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
            _.Options.RegisterCharReplacement('`', ' ');

            _.L("using System;");

            _.L("[TestSpace]")
            .L("namespace Generated").B(_ =>
            {
                _.L("public static class Hello").B(_ =>
                {
                    _.L("public static void SayHi() => Console.WriteLine(`Hello from the generator!`);");
                });
            },
            o => o.BlockStyle = BlockStyle.IndentOnly);

            _.L("using System;");

            _.L("[TestSpace]")
            .L("namespace Generated").B(_ =>
            {
                _.L("public static class Hello").B(_ =>
                {
                    _.L("public static void SayHi() => Console.WriteLine(`Hello from the generator!`);");
                },
                o => o.BlockStyle = BlockStyle.IndentOnly);
            });

            Console.WriteLine(_.ToString());
        }
    }
}
