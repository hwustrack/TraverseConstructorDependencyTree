using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace TraverseConstructorDependencyTree
{
    class Program
    {
        public const string FileToInspectFlag = "FileToInspect";
        public const string TypeToInspectFlag = "TypeToInspect";
        public static readonly string[] SupportedFileExtensions = new[] { ".exe", ".dll" };
        public static readonly string[] RequiredFlags = new[] { FileToInspectFlag, TypeToInspectFlag };

        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            string assemblyPathToInspect = configuration.GetValue<string>(FileToInspectFlag);
            string typeNameToInspect = configuration.GetValue<string>(TypeToInspectFlag);

            if (string.IsNullOrWhiteSpace(assemblyPathToInspect) || string.IsNullOrWhiteSpace(typeNameToInspect))
            {
                PrintHelpText();
                return;
            }

            if (SupportedFileExtensions.Any(ext => assemblyPathToInspect.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    Assembly assemblyToInspect = Assembly.LoadFrom(assemblyPathToInspect);
                    Type typeToInspect = assemblyToInspect.GetType(typeNameToInspect, true);

                    AssemblyInspector inspector = new AssemblyInspector(assemblyToInspect);
                    inspector.TraverseContructorDependencies(typeToInspect);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return;
                }
            }
            else
            {
                Console.WriteLine("No valid input file provided.");
                PrintHelpText();
                return;
            }

            //if ()
        }

        private static void PrintHelpText()
        {
            Console.WriteLine($"Required arguments specified as /arg=value: {string.Join(", ", RequiredFlags)}");
            Console.WriteLine($"/{FileToInspectFlag} - The file name of an assembly in the local directory or the full path to an assembly. Supported file types: {string.Join(", ", SupportedFileExtensions)}.");
            Console.WriteLine($"/{TypeToInspectFlag} - The full name of a Type within the given assembly.");
        }
    }
}
