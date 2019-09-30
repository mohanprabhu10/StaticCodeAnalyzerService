using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Contracts.AuthenticationRepository;
using Contracts.CodeAnalysisGater;
using Contracts.DownloadRepository;
using DataAccessLayer.GithubDownloadRepository;
using DataAccessLayer.UserCodeAnalysisRepository;
using Models.AnalysisReport;
using Models.GatingResult;
using Models.Tools;

namespace Controllers.StaticCodeAnalysisService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class StaticCodeAnalysisService : IStaticCodeAnalysisService
    {
        private string m_username;
        private bool m_signedIn;
        private readonly IAuthenticationRepository m_authentication;
        public StaticCodeAnalysisService()
        {
            m_authentication = new DataAccessLayer.UserAuthenticationRepository.UserAuthenticationRepository();
        }
        public bool SignUp(string userName, string password)
        {
            return m_authentication.SignUp(userName, password);
        }

        public bool SignIn(string userName, string password)
        {
            m_signedIn = m_authentication.SignIn(userName, password);
            m_username = userName;
            return m_signedIn;
        }

        public bool DeleteUser(string userName, string password)
        {
            return m_authentication.Delete(userName, password);
        }

        public bool Logout()
        {
            m_signedIn = false;
            return true;
        }

        public bool Download(string url, string branch)
        {
            if (m_signedIn)
            {
                IDownloadRepository downloadRepo = new GithubDownloadRepository();
                return downloadRepo.Download(url, branch, m_username);
            }

            return false;
        }
        
        public bool InvokeTool(string path, Tools tool)
        {
            if (m_signedIn)
            {
                if (tool == Tools.PVS)
                {
                    PVSAnalysisService.PvsAnalysisService pvsService = new PVSAnalysisService.PvsAnalysisService();
                    return pvsService.InvokeTool(m_username, path);
                }

                ResharperAnalysisService.ResharperAnalysisService resharperService =
                    new ResharperAnalysisService.ResharperAnalysisService();
                return resharperService.InvokeTool(m_username, path);
            }

            return false;
        }


        public List<AnalysisReport> ParseReport(Tools tool,string path, string branch)
        {
            if (m_signedIn)
            {   
                if (tool == Tools.PVS)
                {
                    PVSAnalysisService.PvsAnalysisService pvsService = new PVSAnalysisService.PvsAnalysisService();
                    return pvsService.ParseReport(m_username,path, branch);
                }

                ResharperAnalysisService.ResharperAnalysisService resharperService =
                    new ResharperAnalysisService.ResharperAnalysisService();
                return resharperService.ParseReport(m_username,path, branch);
            }

            return new List<AnalysisReport>();
        }

        public bool GetResult(int threshold, Tools tool, string repository, string branch)
        {
            if (m_signedIn)
            {
                ICodeAnalysisGater gater = new CodeAnalysisGater.GaterLib.CodeGater();
                if (Convert.ToInt32(threshold) < 0)
                    return gater.RelativeGate(m_username, tool, repository, branch);
                return gater.AbsoluteGate(Convert.ToInt32(threshold), m_username, (Tools)Convert.ToInt32(tool), repository, branch);
            }
            return false;
        }

        public List<GatingResult> GetRecentResults(string username)
        {
            var userRepo = new UserCodeAnalysisRepository();
            return userRepo.ReadRepoList(username);
        }
    }
}