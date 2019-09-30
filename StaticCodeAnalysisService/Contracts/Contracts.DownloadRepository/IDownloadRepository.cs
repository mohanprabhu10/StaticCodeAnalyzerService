using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DownloadRepository
{
    public interface IDownloadRepository
    {
        bool Download(string path,string userName);

        bool Download(string path,string branchName,string userName);
    }
}
