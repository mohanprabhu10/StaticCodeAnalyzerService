using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts.ReportParser;
using Contracts.ToolExecuter;
using Models.AnalysisReport;
using Resharper.ToolExecuterLib;

namespace Controllers.ResharperAnalysisService
{
    public class ResharperAnalysisService : IResharperAnalysisService
    {
        public bool InvokeTool(string userName, string path)
        {
            IToolExecuter toolExecuter = new ResharperToolExecuter();
            return toolExecuter.ExecuteTool(userName, path);
        }

        public List<AnalysisReport> ParseReport(string userName, string path, string branch)
        {
            IReportParser parser = new Resharper.ReportParserLib.ResharperReportParser();
            return parser.ParseReport(userName, path, branch);
        }

    }
}