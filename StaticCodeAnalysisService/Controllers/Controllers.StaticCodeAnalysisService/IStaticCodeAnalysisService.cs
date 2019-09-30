using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Models.AnalysisReport;
using Models.GatingResult;
using Models.Tools;

namespace Controllers.StaticCodeAnalysisService
{
    [ServiceContract]
    public interface IStaticCodeAnalysisService
    {
        [OperationContract]
        bool SignUp(string userName, string password);

        [OperationContract]
        bool SignIn(string userName, string password);

        [OperationContract]
        bool DeleteUser(string userName, string password);

        [OperationContract]
        bool Logout();

        [OperationContract]
        bool Download(string url, string branch);

        [OperationContract]
        bool InvokeTool(string path, Tools tool);

        [OperationContract]
        List<AnalysisReport> ParseReport(Tools tool,string path, string branch);

        [OperationContract]
        bool GetResult(int threshold, Tools tool, string repository, string branch);

        [OperationContract]
        List<GatingResult> GetRecentResults(string username);
    }
}
