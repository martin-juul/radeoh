using System.Reflection;

namespace Radeoh.Logging
{
    public interface ILogService
    {
        void Initialize(Assembly assembly, string assemblyName);
    }
}