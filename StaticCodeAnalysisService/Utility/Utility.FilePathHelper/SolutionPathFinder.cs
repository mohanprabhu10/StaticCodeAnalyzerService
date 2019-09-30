using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.FilePathHelper
{
    public class SolutionPathFinder
    {
        public string SolutionFinder(string path,string extrnsion)
        {

            if (!Directory.Exists(path))
                return string.Empty;

            string[] files = System.IO.Directory.GetFiles(path, extrnsion, SearchOption.AllDirectories);

            if (files.Any())
                return files.First();
            return string.Empty;

        }
    }
}
