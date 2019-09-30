using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.ToolExecuter;
using Utility.FilePathHelper;

namespace Resharper.ToolExecuterLib
{
    public class ResharperToolExecuter : IToolExecuter
    {
        public bool ExecuteTool(string userName, string path)
        {
            if (path.Contains(".git"))
                path = @"C:\StaticAnalysisData\Results\" + userName;
            SolutionPathFinder solnPathFinder = new SolutionPathFinder();
            string solnPath= solnPathFinder.SolutionFinder(path, "*.sln");


            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\StaticAnalysisData\BatchFiles\Resharper.bat";
            proc.StartInfo.Arguments = String.Format("{0},{1}", solnPath, @"C:\StaticAnalysisData\Results\"+userName+@"\ResharperOutput.html");
            proc.Start();
            proc.WaitForExit();

            if (!File.Exists(@"C:\StaticAnalysisData\Results\" + userName + @"\ResharperOutput.html"))
            {
                return false;
            }
            return true;

        }
    }
}
