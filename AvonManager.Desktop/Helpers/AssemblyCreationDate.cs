using System;
using System.Reflection;

namespace AvonManager.Helpers
{
    internal static class AssemblyCreationDate
    {
        public static readonly DateTime Value;

        static AssemblyCreationDate()
        {
            AssemblyName assemblyName = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            Version version = assemblyName.Version;
            Value = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
        }
    }

}
