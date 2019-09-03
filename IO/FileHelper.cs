using System.IO;

namespace G1Tool.IO
{
    public class FileHelper
    {
        public static FileStream Create( string path )
        {
            path = Path.GetFullPath( path );

            if ( File.Exists( path ) )
                File.Delete( path );
            else if ( Directory.Exists( path ) )
                Directory.Delete( path );

            Directory.CreateDirectory( Path.GetDirectoryName( path ) );
            return File.Create( path );
        }
    }
}
