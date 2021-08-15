using System.IO;
using System.Reflection;

namespace Paraject.Core.Utilities
{
    public static class ConnectionString
    {
        public static readonly string installPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //public static readonly string config = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={installPath}\Core\Database\ParajectDb.mdf;Integrated Security=True";



        //test config
        public static readonly string config = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Projects\Paraject\ParajectMain\Paraject\Paraject\Core\Database\ParajectDb.mdf;Integrated Security=True";
    }
}
