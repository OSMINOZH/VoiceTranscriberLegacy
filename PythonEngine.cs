using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Transcriber
{
    internal class PythonEngine
    {
        static void Main(string[] args)
        {
            var engine = Python.CreateEngine();

            // Укажите путь к папке, где находится ваш Python-код
            var searchPaths = engine.GetSearchPaths();
            searchPaths.Add(@"path_to_folder_containing_python_script");
            engine.SetSearchPaths(searchPaths);

            var scope = engine.CreateScope();
            scope.SetVariable("x", 10);

            var source = engine.CreateScriptSourceFromFile(@"D:\repos\Transcriber\ILRMA.py");

            var compiledCode = source.Compile();
            compiledCode.Execute(scope);

            foreach (var varName in scope.GetVariableNames())
            {
                Console.WriteLine($"Variable: {varName}, Value: {scope.GetVariable(varName)}");
            }
        }
    }
}
