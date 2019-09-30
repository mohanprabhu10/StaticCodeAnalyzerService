using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.CodeAnalysisGater;
using DataAccessLayer.UserCodeAnalysisRepository;
using Models.Tools;

namespace CodeAnalysisGater.GaterLib
{
    public class CodeGater : ICodeAnalysisGater
    {
        readonly Contracts.UserCodeAnalysisRepository.IUserCodeAnalysisRepository userCodeAnalysisRepo;

        public CodeGater(bool test = false)
        {
            userCodeAnalysisRepo = new UserCodeAnalysisRepository(test);
        }

        public bool AbsoluteGate(int threshold, string userName, Tools tool, string repository, string branch)
        {
            
            var result = userCodeAnalysisRepo.ReadLatestResult(userName, repository, branch, tool);
            if (!result.Any())
                return false;
            if (result[0] == -1)
                userCodeAnalysisRepo.UpdateThreshold(userName,repository,tool,branch,threshold,result[1]);

            return result[1] <= threshold;
        }

        public bool RelativeGate(string userName, Tools tool, string repository, string branch)
        {

            var result = userCodeAnalysisRepo.ReadLatestResult(userName, repository, branch, tool);
            if (!result.Any())
                return false;
            if (result[0] == -1)
            {
                int threshold = userCodeAnalysisRepo.ReadRelativeThreshold(userName, repository, branch, tool);
                userCodeAnalysisRepo.UpdateThreshold(userName, repository, tool, branch, threshold, result[1]);

                return result[1] <= threshold;
            }

            return result[1] <= result[0];
        }
    }
}
