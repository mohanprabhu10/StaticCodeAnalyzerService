using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.ReportParser;
using Contracts.UserCodeAnalysisRepository;
using DataAccessLayer.UserCodeAnalysisRepository;
using Models.AnalysisReport;
using Models.Tools;

namespace PVS.ReportParserLib
{
    public class PvsReportParser : IReportParser
    {
        readonly IUserCodeAnalysisRepository repo;
        public PvsReportParser(bool test = false)
        {
            repo = new DataAccessLayer.UserCodeAnalysisRepository.UserCodeAnalysisRepository(test);
        }

        public List<AnalysisReport> ParseReport(string userName, string path, string branch)
        {
            if (!File.Exists($@"C:\StaticAnalysisData\Results\{userName}\Static_Analysis_Tools.plog.txt"))
            {
                return new List<AnalysisReport>();
            }


            string[] lines = File.ReadAllLines(
                $@"C:\StaticAnalysisData\Results\{userName}\Static_Analysis_Tools.plog.txt");

            var report = ParseReportToList(lines);
           
            repo.Add(userName, path, branch, Tools.PVS, report.Count,-1);

            return report;
        }

        private List<AnalysisReport> ParseReportToList(string[] lines)
        {
            List<AnalysisReport> report = new List<AnalysisReport>();

            foreach (string line in lines)
            {
                if (line.Contains("error"))
                {

                    AnalysisReport analysisReport = new AnalysisReport();
                    int startIndex;

                    startIndex = (line.IndexOf("): ") + ("): ").Length);
                    analysisReport.TypeId = line.Substring(startIndex, 11);

                    startIndex = 0;
                    analysisReport.FileName = line.Substring(startIndex, line.IndexOf(" (") - startIndex);

                    //analysisReport.Line
                    startIndex = line.IndexOf(" (") + (" (").Length;
                    var lineNo = line.Substring(startIndex, line.IndexOf("): ") - startIndex);
                    try
                    {
                        analysisReport.Line = Convert.ToInt32(lineNo);
                    }
                    catch
                    {
                        analysisReport.Line = 0;
                    }

                    startIndex = line.IndexOf("): ") + ("): ").Length + 12;
                    analysisReport.Message = line.Substring(startIndex, line.Length - startIndex);
                    report.Add(analysisReport);

                }
            }

            return report;
        }

    }
}
