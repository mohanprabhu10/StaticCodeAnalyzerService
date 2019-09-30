using Contracts.ReportParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.AnalysisReport;
using System.IO;
using Contracts.UserCodeAnalysisRepository;
using Models.Tools;

namespace Simian.ReportParserLib
{
    public class SimianReportParser : IReportParser
    {

        readonly IUserCodeAnalysisRepository repo;
        public SimianReportParser(bool test = false)
        {
            repo = new DataAccessLayer.UserCodeAnalysisRepository.UserCodeAnalysisRepository(test);
        }


        public List<AnalysisReport> ParseReport(string userName, string path, string branch)
        {

            if (!File.Exists($@"C:\StaticAnalysisData\Results\{userName}\SimianResult.txt"))
            {
                return new List<AnalysisReport>();
            }


            string[] lines = File.ReadAllLines(
                $@"C:\StaticAnalysisData\Results\{userName}\SimianResult.txt");

            var report = ParseReportToList(lines);

            List<AnalysisReport> simianReport = new List<AnalysisReport>();
            simianReport.Add(report);

            repo.Add(userName, path, branch, Tools.Simian, report.Line, -1);

            return simianReport;
        }

        private AnalysisReport ParseReportToList(string[] lines)
        {

            

            int noOfDupilcates=-1;
            string message="";

            foreach (string line in lines)
            {
                if (line.Contains("Found"))
                {

                    
                    int startIndex;

                    startIndex = (line.IndexOf("Found ") + ("Found ").Length);
                    var lineNo = line.Substring(startIndex, line.IndexOf("duplicate") - startIndex);
                    noOfDupilcates = Convert.ToInt32(lineNo);

                }
                message = message + line+"\n";
            }

            AnalysisReport analysisReport = new AnalysisReport { Line = noOfDupilcates, Message = message };

            return analysisReport;
        }

    }
}
