using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TraverseConstructorDependencyTree
{
    /// <summary>
    /// Extension methods for System.Reflection.Assembly
    /// </summary>
    public static class AssemblyExtensions
    {
        // Taken from https://stackoverflow.com/a/29379834
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
