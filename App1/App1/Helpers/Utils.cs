using System.IO;
using System.Reflection;

namespace App1.Helpers
{
    public static class Utils
    {
        public static void ExtractSaveResource(string filename, string location)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            using (Stream resFilestream = a.GetManifestResourceStream(filename))
            {
                if (resFilestream != null)
                {
                    string full = Path.Combine(location, filename);

                    using (FileStream stream = File.Create(full))
                    {
                        resFilestream.CopyTo(stream);
                    }

                }
            }
        }
    }
}
