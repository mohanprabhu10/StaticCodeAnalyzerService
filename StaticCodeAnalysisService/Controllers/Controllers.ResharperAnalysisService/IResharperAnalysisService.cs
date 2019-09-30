using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Models.AnalysisReport;

namespace Controllers.ResharperAnalysisService
{
    [ServiceContract]
    public interface IResharperAnalysisService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/InvokeTool/{path}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool InvokeTool(string userName, string path);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/ParseReport", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<AnalysisReport> ParseReport(string userName,string path, string branch);
    }
}
