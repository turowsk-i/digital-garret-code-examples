using System;
using System.CodeDom.Compiler;
using Common;

namespace Main
{
    internal class Program
    {
        private static string _compilerLanguage = "CSharp";
        private static string _interfaceDllName = "Common.dll";
        private static string _externalCodeClass = "InvoiceParser.InvoiceParser";
        private static string _externalCodeFile = "ExternalCode.txt";

        static void Main(string[] args)
        {
            CompilerResults compilerResults;

            using (var codeDomProvider = CodeDomProvider
                    .CreateProvider(_compilerLanguage))
            {
                var code = System.IO.File.ReadAllText(_externalCodeFile);
                var compileParams = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true
                };
                compileParams.ReferencedAssemblies.Add(_interfaceDllName);
                compilerResults = codeDomProvider
                        .CompileAssemblyFromSource(compileParams, code);
            }

            if (compilerResults.Errors.Count > 0)
            {
                throw new Exception("External code failed to compile.");
            }

            var parser = compilerResults.CompiledAssembly
                    .CreateInstance(_externalCodeClass) as IInvoiceParser;
            if (parser == null)
            {
                throw new Exception("Parser failed to be instantiated.");
            }

            var input = new string[]
            {
                "First line",
                "Second line",
                "Third line"
            };

            var output = parser.ParseInvoice(input);
            Console.WriteLine(string.Join(Environment.NewLine, output));
            Console.ReadKey();
        }
    }
}