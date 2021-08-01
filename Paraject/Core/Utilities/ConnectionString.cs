using System.Configuration;

namespace Paraject.Core.Utilities
{
    public static class ConnectionString
    {
        public static readonly string config = ConfigurationManager.ConnectionStrings["ParajectDb"].ConnectionString;
    }
}
