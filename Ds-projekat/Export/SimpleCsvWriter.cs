using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ds_projekat.Export
{
    internal class SimpleCsvWriter
    {
        public void SaveLines(string filePath, List<string> lines)
        {
            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }
    }
}