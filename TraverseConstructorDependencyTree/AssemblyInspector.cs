using System;
using System.Collections.Generic;
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
            RecurseConstructorDepdencies(typeToInspect, "");
        }

        private void RecurseConstructorDepdencies(Type typeToInspect, string outputPrefix)
        {
            foreach (ConstructorInfo constructorInfo in typeToInspect.GetConstructors())
            {
                Console.WriteLine($"{outputPrefix}{constructorInfo.Name}");
                foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                {
                    RecurseConstructorDepdencies(parameterInfo.ParameterType, outputPrefix + "\t");
                }
            }
        }
    }
}
