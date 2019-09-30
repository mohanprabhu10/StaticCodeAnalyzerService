using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.DownloadRepository;

namespace DataAccessLayer.GithubDownloadRepository
{
    public class GithubDownloadRepository : IDownloadRepository
    {
        private const string gitClonePath = @"C:\StaticAnalysisData\Results\";

        public bool Download(string path, string userName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\StaticAnalysisData\BatchFiles\GitDownload.bat";
            proc.StartInfo.Arguments = String.Format("{0},{1}",path, userName);
            proc.Start();
            proc.WaitForExit();

           
            if (!Directory.Exists(gitClonePath + userName))
            {
                return false;
            }

            return true;
        }

        public bool Download(string path, string branchName, string userName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\StaticAnalysisData\BatchFiles\GitBranchDownload.bat";
            proc.StartInfo.Arguments = String.Format("{0},{1},{2}", path, userName,branchName);
            proc.Start();
            proc.WaitForExit();


            if (!Directory.Exists(gitClonePath + userName))
            {
                return false;
            }

            return true;
        }
    }
}
