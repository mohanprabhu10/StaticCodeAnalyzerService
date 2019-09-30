using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Unit.ResharperReportParser
{
    [TestClass]
    public class ResharperReportParserUnitTest
    {
        Contracts.ReportParser.IReportParser reportParser;
        Contracts.ToolExecuter.IToolExecuter toolExecuter;
        Contracts.AuthenticationRepository.IAuthenticationRepository authenticationRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            reportParser = new Resharper.ReportParserLib.ResharperReportParser(true);
            toolExecuter = new Resharper.ToolExecuterLib.ResharperToolExecuter();
            authenticationRepo = new DataAccessLayer.UserAuthenticationRepository.UserAuthenticationRepository(true);

            //Generate Report
            string path = @"C:\StaticAnalysisData\TestFiles\";
            toolExecuter.ExecuteTool("Admin", path);
            authenticationRepo.SignUp("Admin", "pass");

        }


        [TestMethod]
        public void Given_Valid_Arguments_When_ParseReport_Invoked_Then_true_Asserted()
        {

            string path = @"C:\StaticAnalysisData\TestFiles\";
            //Pasre
            var report = reportParser.ParseReport("Admin",path, "master");
            Assert.IsNotNull(report);
            Assert.AreEqual(10, report.Count);

        }


        [TestMethod]
        public void Given_InValid_Arguments_When_ParseReport_Invoked_Then_true_Asserted()
        {
            string path = @"C:\StaticAnalysisData\TestFiles\";

            //Pasre
            var report = reportParser.ParseReport("AdminInvalid", path, "master");
            Assert.AreEqual(0,report.Count);

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



    }
}
