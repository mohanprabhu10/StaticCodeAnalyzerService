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

namespace Resharper.ReportParserLib
{
    public class ResharperReportParser : IReportParser
    {
        readonly IUserCodeAnalysisRepository repo;
        public ResharperReportParser(bool test = false)
        {
            repo = new DataAccessLayer.UserCodeAnalysisRepository.UserCodeAnalysisRepository(test);
        }


        public List<AnalysisReport> ParseReport(string userName,string path, string branch)
        {


            if (!File.Exists($@"C:\StaticAnalysisData\Results\{userName}\ResharperOutput.html"))
            {
                return new List<AnalysisReport>();
            }
            string[] lines = File.ReadAllLines($@"C:\StaticAnalysisData\Results\{userName}\ResharperOutput.html");

            var report = ParseReportToList(lines);

            
            repo.Add(userName, path, branch, Tools.Resharper, report.Count,-1);

            return report;

        }


        private List<AnalysisReport> ParseReportToList(string[] lines)
        {
            List<AnalysisReport> report = new List<AnalysisReport>();

            foreach (string line in lines)
            {
                if (line.Contains("<Issue TypeId="))
                {

                    AnalysisReport analysisReport = new AnalysisReport();
                    int startIndex;

                    startIndex = (line.IndexOf("TypeId=") + ("TypeId=").Length + 1);
                    analysisReport.TypeId = line.Substring(startIndex, line.IndexOf("File=") - startIndex - 2);

                    startIndex = line.IndexOf("File=") + ("File=").Length + 1;
                    analysisReport.FileName = line.Substring(startIndex, line.IndexOf("Offset=") - startIndex - 2);

                    //analysisReport.Line
                    startIndex = line.IndexOf("Line=") + ("Line=").Length + 1;
                    var lineNo = line.Substring(startIndex, line.IndexOf("Message=") - startIndex - 2);
                    try
                    {
                        analysisReport.Line = Convert.ToInt32(lineNo);
                    }
                    catch
                    {
                        analysisReport.Line = 0;
                    }

                    startIndex = line.IndexOf("Message=") + ("Message=").Length + 1;
                    analysisReport.Message = line.Substring(startIndex, line.IndexOf("/>") - startIndex - 2);
                    report.Add(analysisReport);

                }
            }

            return report;
        }


    }
}
