﻿using System.IO;
using System.Reflection;

namespace App1.Helpers
{
    public static class Utils
    {
        public static void ExtractSaveResource(string filename, string location)
        {
            var a = Assembly.GetExecutingAssembly();
            using (var resFilestream = a.GetManifestResourceStream(filename))
            {
                if (resFilestream != null)
                {
                    string full = Path.Combine(location, filename);

                    using (var stream = File.Create(full))
                    {
                        resFilestream.CopyTo(stream);
                    }

                }
            }
        }
    }
}
