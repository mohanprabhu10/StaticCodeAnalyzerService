using Contracts.ToolExecuter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simian.ToolExecuterLib
{
    public class SimianToolExecuter : IToolExecuter
    {
        public bool ExecuteTool(string userName, string path)
        {
            if (path.Contains(".git"))
                path = @"C:\StaticAnalysisData\Results\" + userName;

            if (!Directory.Exists(path))
            {
                return false;
            }

            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\StaticAnalysisData\BatchFiles\SimianToolRun.bat";
            proc.StartInfo.Arguments = String.Format("{0} {1} {2}", @"C:\StaticAnalysisData\Results\" + userName + @"\SimianResult.txt", userName, path + @"/**\*.cs");
            proc.Start();
            proc.WaitForExit();

            if (!File.Exists(@"C:\StaticAnalysisData\Results\" + userName + @"\SimianResult.txt"))
            {
                return false;
            }
            return true;
        }


    }
}
