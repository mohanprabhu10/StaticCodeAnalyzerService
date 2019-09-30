using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.GatingResult;
using Models.Tools;

namespace Contracts.UserCodeAnalysisRepository
{
    public interface IUserCodeAnalysisRepository
    {
        bool Add(string userName, string path, string branch, Tools tool, int result,int threshold);
        bool Delete(string userName, string path, string branch, Tools tool);
        List<int> ReadAllResults(string userName, string path, string branch, Tools tool);
        int ReadRelativeThreshold(string userName, string path, string branch, Tools tool);
        List<GatingResult> ReadRepoList(string userName);
       List<int> ReadLatestResult(string userName, string path, string branch, Tools tool);

        bool UpdateThreshold(string userName, string path, Tools tool, string branch, int threshold, int result);
    }

}
