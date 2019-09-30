using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit.Test.GithubDownloadRepository
{
    [TestClass]
    public class GithubDownloadRepositoryUnitTest
    {
        Contracts.DownloadRepository.IDownloadRepository downloadRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            downloadRepo = new DataAccessLayer.GithubDownloadRepository.GithubDownloadRepository();
        }


        [TestMethod]
        public void Given_Valid_Arguments_When_Download_Invoked_Then_true_Asserted()
        {
            string url = "https://github.com/phani-rajbn/PhilipsJune2019Dotnet.git";
            Assert.IsTrue(downloadRepo.Download(url,"master","Admin"));

        }

        [TestMethod]
        public void Given_InValid_Arguments_When_Download_Invoked_Then_true_Asserted()
        {
            string url = "https://github.com";
            Assert.IsFalse(downloadRepo.Download(url, "master","Admin"));
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_GitBranchDownload_Invoked_Then_true_Asserted()
        {
            string url = "https://github.com/jquery/jquery-ui.git";
            Assert.IsTrue(downloadRepo.Download(url, "interactions", "Admin"));

        }

        [TestMethod]
        public void Given_InValid_Arguments_When_GitBranchDownload_Invoked_Then_true_Asserted()
        {
            string url = "https://github.com/jquery/jquery-ui.git";
            Assert.IsFalse(downloadRepo.Download(url,"InvalidBranch", "Admin"));
        }


        [TestCleanup]
        public void DeleteTestFolder()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\StaticAnalysisData\BatchFiles\DeleteTestResult.bat";
            proc.Start();
            proc.WaitForExit();
        }
    }
}
