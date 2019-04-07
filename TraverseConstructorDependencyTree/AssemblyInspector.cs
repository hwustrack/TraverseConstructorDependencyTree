using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TraverseConstructorDependencyTree
{
    public class AssemblyInspector
    {
        private Assembly _assemblyToInspect;

        public AssemblyInspector(Assembly assemblyToInspect)
        {
            _assemblyToInspect = assemblyToInspect ?? throw new ArgumentNullException(nameof(assemblyToInspect));
        }

        public void TraverseContructorDependencies(Type typeToInspect)
        {
            Console.WriteLine(typeToInspect.Name);
            Console.WriteLine(new string('-', 5));
            RecurseConstructorDepdencies(typeToInspect, "");
        }

        private void RecurseConstructorDepdencies(Type typeToInspect, string outputPrefix)
        {
            foreach (ConstructorInfo constructorInfo in typeToInspect.GetConstructors())
            {
                foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                {
                    if (parameterInfo.ParameterType.IsInterface)
                    {
                        Type interfaceType = parameterInfo.ParameterType;
                        IEnumerable<Type> assignableTypes = interfaceType.Assembly.GetLoadableTypes()
                            .Where(t => !t.IsInterface && interfaceType.IsAssignableFrom(t));
                        if (assignableTypes.Any())
                        {
                            foreach (Type assignableType in assignableTypes)
                            {
                                Console.WriteLine($"{outputPrefix}{interfaceType.Name} - {assignableType.Name}");
                                RecurseConstructorDepdencies(assignableType, outputPrefix + "\t");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{outputPrefix}{interfaceType.Name} - Could not find assignable type");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{outputPrefix}{parameterInfo.Name}");
                        RecurseConstructorDepdencies(parameterInfo.ParameterType, outputPrefix + "\t");
                    }
                }
            }
        }
    }

    // https://stackoverflow.com/a/29379834
    public static class TypeLoaderExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
