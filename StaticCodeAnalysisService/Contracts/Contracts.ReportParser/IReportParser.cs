using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.AnalysisReport;
using Models.Tools;

namespace Contracts.ReportParser
{
    public interface IReportParser
    {
        List<AnalysisReport> ParseReport(string userName, string path, string branch);
    }
}
