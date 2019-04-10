using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TraverseConstructorDependencyTree
{
    /// <summary>
    /// Class to inspect an assembly.
    /// </summary>
    public class AssemblyInspector
    {
        /// <summary>
        /// Recurse through the constructors of each type found in the constructor of the given type
        /// </summary>
        /// <param name="typeToInspect">Type to inspect</param>
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
}
