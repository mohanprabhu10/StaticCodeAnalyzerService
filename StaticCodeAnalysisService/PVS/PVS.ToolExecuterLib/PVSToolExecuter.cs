using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.ToolExecuter;
using Utility.FilePathHelper;

namespace PVS.ToolExecuterLib
{
    public class PVSToolExecuter : IToolExecuter
    {
        public bool ExecuteTool(string userName, string path)
        {
            if (path.Contains(".git"))
                path = @"C:\StaticAnalysisData\Results\" + userName;
            SolutionPathFinder solnPathFinder = new SolutionPathFinder();
            string solnPath = solnPathFinder.SolutionFinder(path, "*.sln");

            string plogPath= solnPathFinder.SolutionFinder(path, "*.plog");

            Process pvsproc = new Process();

            pvsproc.StartInfo.FileName = @"C:\StaticAnalysisData\BatchFiles\PVS_Studio.bat";

            pvsproc.StartInfo.Arguments = String.Format("{0},{1},{2},{3}", solnPath, @"C:\StaticAnalysisData\Tools\PVS-Studio", @"C:\StaticAnalysisData\Results\" + userName,plogPath);
            pvsproc.Start();
            pvsproc.WaitForExit();


            if (File.Exists(@"C:\StaticAnalysisData\Results\" + userName + @"\Static_Analysis_Tools.plog.txt"))
            {
                return true;
            }
            return false;
        }

        
    }
}
