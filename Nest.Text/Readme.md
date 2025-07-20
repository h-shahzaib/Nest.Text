# **Nest.Text**

**Nest.Text** is a zero-dependency, fluent text generation library that helps you build structured content — from **C#**, **Python**, and **YAML** to **HTML**, **XML**, and more. It lets you describe what to generate — Nest takes care of how it's formatted.

---

## 📦 Installation

```bash
dotnet add package Nest.Text
```

---

## 🚀 What Is It?

Nest.Text provides a builder-style API to generate code, markup, or any structured text using:

* `.L(...)` – Write one or more lines
* `.B(b => { })` – Begin a block with nested content
* `.L(...).B(b => { })` – Write a logical block of lines

Nest adds or avoids line breaks based on chaining and structure awareness.

---

# Line Break Behavior

Understanding how line breaks work with different usage patterns:

## Usage Patterns

**Lines**
```csharp
_.L("one");
_.L("two");
_.L("one");
_.L("two");
```
- ❌ No line break will be added between these lines

**Multiple Lines**

```csharp
✅ Line break will be added here
_.L(
    "one",
    "two"
);
✅ Line break will be added here
_.L("one");
_.L("two");
```

**Multi-Line raw string literal**

```csharp
✅ Line break will be added here
_.L(
    """
    one
    two
    """
);
✅ Line break will be added here
_.L(
    """
    one
    two
    """
);
✅ Line break will be added here
```

**Strings having line breaks**

```csharp
✅ Line break will be added here
_.L("one \n two");
_.L(); ⚠️ First explicit line break will override implicit line break
_.L(); ✅ Explicit line break will be added here
_.L("one \n two");
✅ Line break will be added here
```

**Method Chaining**
```csharp
✅ Line break will be added here
_.L("one")
 .L("two").B(_ => 
 {
    _.L("three");
 });
✅ Line break will be added here
```
- ❌ No line break between chained calls

---

# ⚙️ Options

```csharp
_.Options.BlockStyle = BlockStyle.Braces; // or IndentOnly (Default)
_.Options.IndentSize = 4;                 // spaces per indent level
```

---

## 🔄 Smart Quote Replacement

You can use backticks (\`) instead of escaped quotes in your strings:

```csharp
_.L("Console.WriteLine(`Hello World!`);"); 
// Outputs: Console.WriteLine("Hello World!");
```

To customize or disable:

```csharp
_.Options.RegisterCharReplacement('`', '"');
_.Options.RemoveCharReplacement('`');
```

---

## ⚙️ Options (Live Behavior & Inheritance)

`Options` in Nest.Text are **live** — changes apply immediately and affect all content added *after* the change.

### 🔧 Live Indent Example

```csharp
_.L("Console.WriteLine(`A`)"); // uses current indent size (e.g. 4)

_.Options.IndentSize = 0;
_.L("Console.WriteLine(`B`)"); // will have no indentation

_.Options.IndentSize = 4;
_.L("Console.WriteLine(`C`)"); // will again be indented
```

Each line reflects the indent size active **at the time it's written**.

---

### 📦 BlockStyle Inheritance

When you change `BlockStyle`, it affects the **current block** and **all nested blocks** by default:

```csharp
_.L("namespace Demo").B(_ =>
{
    _.Options.BlockStyle = BlockStyle.Braces;

    _.L("class A").B(_ =>
    {
        _.L("void Print()").B(_ =>
        {
            _.L("print(`Hello`)");
        });
    });
});
```

✅ `namespace Demo` block itself & all blocks inside it will use now `Braces`.

### ✋ Prevent Inheritance (Limit Scope)

If you want to apply a `BlockStyle` to just the current block **without affecting nested blocks**, reset it at the end:

```csharp
_.L("namespace Demo").B(_ =>
{
    _.L("class A").B(_ =>
    {
        _.L("void Print()").B(_ =>
        {
            _.L("print(`Hello`)");
        });
    });

    _.Options.BlockStyle = BlockStyle.Braces;
});
```

---

### 🔁 Inherited Automatically

Any block you enter inherits the parent’s `Options` by default — this includes indent size, block style, and character replacements. You can override or reset them at any level as needed.

This behavior makes it easy to dynamically shape output without needing multiple builders or deep config trees.

---

# Reusing Common Builder Patterns

Since `.B()` accepts a lambda, you can pass a method that takes an `ITextBuilder` parameter — this lets you keep the structure clean by moving code blocks into reusable methods.

---

### ✅ Example: Extracting an If-Else Block

Instead of writing everything inline:

```csharp
_.L("if (count > 6)").B(_ =>
{
    _.L("Console.WriteLine(`Hello World!`);");
    _.L("Console.WriteLine(`Hello Again!`);");
})
.L("else").B(_ =>
{
    _.L("Console.WriteLine(`Hello World!`);");
});
```

You can extract it into a separate method:

```csharp
void AddIfElse(ITextBuilder _)
{
    _.L("if (count > 6)").B(_ =>
    {
        _.L("Console.WriteLine(`Hello World!`);");
        _.L("Console.WriteLine(`Hello Again!`);");
    })
    .L("else").B(_ =>
    {
        _.L("Console.WriteLine(`Hello World!`);");
    });
}
```

And call it like this:

```csharp
_.L("public static void Main(string[] args)").B(_ => AddIfElse(_));
```

✅ This keeps the `Main` method block clean and readable.

---

### 🧩 Using `.Append()`

`.Append()` allows you to insert multiple external methods cleanly:

```csharp
_.L("public static void Main(string[] args)").B(_ => 
    _.Append(_ => AddIfElse(_)).Append(_ => AddLoop(_))
);
```

Each method `AddIfElse()`, `AddLoop()` still receives the same builder instance, so indentation and formatting stay consistent.

---

### 🔁 Using `.Chain()`

If you're inside a chain, use `.Chain()` and pass a method that takes `IChainBuilder`:

```csharp
void AddMainBody(IChainBuilder _)
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
```

Then call it like:

```csharp
_.L("public static void Main(string[] args)")
    .Chain(_ => AddMainBody(_));
```

---

### Why?

* ✅ Keeps your generation logic modular.
* ✅ Makes complex structures easier to read and maintain.
* ✅ Allows mixing and matching reusable blocks.

This approach is especially helpful when generating large sections like methods, conditionals, loops, or class structures.

---

## 🧪 C# Example (Braces Block Style with Chaining)

```csharp
var _ = TextBuilder.Create();
_.Options.BlockStyle = BlockStyle.Braces;
_.Options.IndentSize = 4;

_.L("using System.Text;");

_.L("namespace MyProgram").B(_ =>
{
    _.L("public class MyProgram").B(_ =>
    {
        _.L("public static void Main(string[] args)").B(_ =>
        {
            _.L("if (count > 6)").B(_ =>
            {
                _.L("Console.WriteLine(`Hello World!`);");
                _.L("Console.WriteLine(`Hello Again!`);");
            })
            .L("else").B(_ =>
            {
                _.L("Console.WriteLine(`Hello World!`);");
            });
        });
    });
});

Console.WriteLine(_.ToString());
```

---

## 🐍 Python Example (IndentOnly)

```csharp
var _ = TextBuilder.Create();
_.Options.BlockStyle = BlockStyle.IndentOnly;

_.L("def greet():").B(_ =>
{
    _.L("print(`Hello World!`)");
    _.L("print(`Hello Again!`)");
});

Console.WriteLine(_.ToString());
```

---

## 🌐 HTML Example

```csharp
var _ = TextBuilder.Create();
_.Options.BlockStyle = BlockStyle.IndentOnly;
_.Options.IndentSize = 2;

_.L("<div>").B(_ =>
{
    _.L("<span>Hello World!</span>");
    _.L("<span>Hello Again!</span>");
}
).L("</div>");

Console.WriteLine(_.ToString());
```

---

## 📄 XML Example

```csharp
var _ = TextBuilder.Create();
_.Options.BlockStyle = BlockStyle.IndentOnly;

_.L("<config>").B(_ =>
{
    _.L("<entry key=`theme`>dark</entry>");
    _.L("<entry key=`lang`>en</entry>");
}
).L("</config>");

Console.WriteLine(_.ToString());
```

---

## 📘 YAML Example

```csharp
var _ = TextBuilder.Create();
_.Options.BlockStyle = BlockStyle.IndentOnly;

_.L("library:").B(_ =>
{
    _.L("name: `Nest`");
    _.L("use: `Structured Text Generation`");

    _.L("features:").B(_ =>
    {
        _.L("- Automated Indentation");
        _.L("- Easy To Use");
        _.L("- Zero Dependency");
    });
});

Console.WriteLine(_.ToString());
```

---

## 🪛 Debugging

You can inspect the builder at any point during generation:

```csharp
_.L("if (count > 0)").B(_ =>
{
    _.L("Console.WriteLine(`Positive`);");

    Console.WriteLine("--- Debug Snapshot ---");
    Console.WriteLine(_.ToString()); // View current generated output
});
```

Alternatively, set a **breakpoint** anywhere and inspect the builder in your debugger.
This is especially useful when checking structure, indentation, or formatting mid-flow.

Since everything is standard C#, you can step through and verify behavior interactively — no custom tooling needed.

---

## 📚 Summary

* Fluent and **chainable** API
* Smart formatting — line breaks where needed, not where not
* Custom indentation and block styles
* Backtick-friendly string writing
* Debuggable at every step
* No dependencies, works anywhere .NET runs

---

## 🔗 Links

* 📦 NuGet: [Nest.Text on NuGet](https://www.nuget.org/packages/Nest.Text/)
* 💻 GitHub: [github.com/h-shahzaib/Nest.Text](https://github.com/h-shahzaib/Nest.Text)

