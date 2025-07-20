using Nest.Text;

// dotnet build -c Release && dotnet pack -c Release

namespace Nest.Demo
{
    internal class Program
    {
        static void Main()
        {
            var _ = TextBuilder.Create();
            _.Options.BlockStyle = BlockStyle.Braces;
            _.Options.IndentSize = 4;

            _.L("public static void Main(string[] args)")
                .Chain(AddMainBody);

            Console.WriteLine(_.ToString());
        }

        static void AddMainBody(IChainBuilder _)
        {
            _.B(_ =>
            {
                _.L("if (count > 6)").B(_ =>
                {
                    _.L("Console.WriteLine(`Hello World!`);");
                })
                .L("else").B(_ =>
                {
                    _.L("Console.WriteLine(`Goodbye!`);");
                });
            });
        }
    }
}
