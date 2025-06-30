using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

var invoiceParserScript = @"
public class InvoiceParser : IInvoiceParser {
    public IEnumerable<string> ParseInvoice(IEnumerable<string> lines) {
        return lines.Select(line => ""Parsed: "" + line);
    }
}
return new InvoiceParser();";

var options = ScriptOptions.Default
    .AddReferences(typeof(IInvoiceParser).Assembly)
    .AddImports(
        typeof(System.Linq.Enumerable).Namespace,
        typeof(System.Collections.Generic.IEnumerable<>).Namespace
    );

var script = CSharpScript.Create<IInvoiceParser>(invoiceParserScript, options);
var instance = script.RunAsync().Result.ReturnValue;

// Drumroll...
var result = instance.ParseInvoice(["Line 1", "Line 2", "Line 3"]);

Console.WriteLine(string.Join(Environment.NewLine, result));

// This goes at the very end of our Program.cs
public interface IInvoiceParser
{
    IEnumerable<string> ParseInvoice(IEnumerable<string> lines);
}