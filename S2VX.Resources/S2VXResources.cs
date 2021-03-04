using System;
using System.Reflection;

[assembly: CLSCompliant(false)]
namespace S2VX.Resources {
    public static class S2VXResources {
        public static Assembly ResourceAssembly => typeof(S2VXResources).Assembly;
    }
}
