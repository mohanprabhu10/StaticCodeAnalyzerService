using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.ToolExecuter;
using Models.AnalysisReport;
using PVS.ToolExecuterLib;

namespace Controllers.PVSAnalysisService
{
    public class PvsAnalysisService : IPvsAnalysisService
    {
        public bool InvokeTool(string userName, string path)
        {
            IToolExecuter toolExecuter = new PVSToolExecuter();
            return toolExecuter.ExecuteTool(userName, path);
        }

        public List<AnalysisReport> ParseReport(string userName,string path, string branch)
        {
            Contracts.ReportParser.IReportParser parser=new PVS.ReportParserLib.PvsReportParser();
            return parser.ParseReport(userName,path, branch);
        }
    }
}