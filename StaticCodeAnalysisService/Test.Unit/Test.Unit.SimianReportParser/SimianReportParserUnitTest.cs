using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Unit.SimianReportParser
{
    [TestClass]
    public class SimianReportParserUnitTest
    {
        Contracts.ReportParser.IReportParser reportParser;
        Contracts.ToolExecuter.IToolExecuter toolExecuter;
        Contracts.AuthenticationRepository.IAuthenticationRepository authenticationRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            reportParser = new Simian.ReportParserLib.SimianReportParser(true);
            toolExecuter = new Simian.ToolExecuterLib.SimianToolExecuter();
            authenticationRepo = new DataAccessLayer.UserAuthenticationRepository.UserAuthenticationRepository(true);

            //Generate Report
            string path = @"C:\StaticAnalysisData\TestFiles\Simian";
            toolExecuter.ExecuteTool("Admin", path);
            authenticationRepo.SignUp("Admin", "pass");

        }

        [TestCleanup]
        public void DeleteTestFolder()
        {
            authenticationRepo.Delete("Admin", "pass");
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\StaticAnalysisData\BatchFiles\DeleteTestResult.bat";
            proc.Start();
            proc.WaitForExit();
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_ParseReport_Invoked_Then_true_Asserted()
        {

            string path = @"C:\StaticAnalysisData\TestFiles\";
            //Pasre
            var report = reportParser.ParseReport("Admin", path, "master");
            Assert.IsNotNull(report);
            Assert.AreEqual(100, report[0].Line);


        }


        [TestMethod]
        public void Given_InValid_Arguments_When_ParseReport_Invoked_Then_true_Asserted()
        {
            string path = @"C:\StaticAnalysisData\TestFiles\";

            //Pasre
            var report = reportParser.ParseReport("AdminInvalid", path, "master");
            Assert.AreEqual(0, report.Count);

        }
    }
}
