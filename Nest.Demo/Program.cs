using Nest.Text;
using System;

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

            _.L("using System;");

            _.L("[TestSpace]")
            .L("namespace Generated").B(_ =>
            {
                _.L("public static class Hello").B(_ =>
                {
                    _.L("public static void SayHi() => Console.WriteLine(`Hello from the generator!`);");

                    _.L(
                        """
				        using System;
				        using System.ComponentModel;
				        using System.Collections;
				        using System.Collections.Generic;
				        using System.Collections.ObjectModel;
				        using System.Linq;
				        using System.Reactive.Linq;
				        using System.Text;
				        using System.Threading;
				        using System.Windows;
				        using System.Windows.Controls;
				        using System.Windows.Controls.Primitives;
				        using System.Windows.Data;
				        using System.Windows.Media;
				        using MssCustomControls;
				        using mssAdminData;
				        using mssAdminData.Classes;
				        using mssAdminData.Interfaces;
				        using mssAdminUI.Interfaces;
				        using mssAdminUI.Interfaces.Implementations;
				        using mssAdminUI.Plumbing.DataAdapters;
				        using mssAdminUI.Plumbing.DependencyProperties;
				        using mssAdminUI.ValueConverters;
				        using mssAdminUI.Views;
				        using ReactiveUI;
				        using Container = mssAdminData.Container;
				        """
                    );
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
