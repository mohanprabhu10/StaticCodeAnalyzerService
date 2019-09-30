using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.E2E.StaticCodeAnalysis.StaticCodeAnalysisService;

namespace Test.E2E.StaticCodeAnalysis
{
    [TestClass]
    public class StaticCodeAnalysisE2ETest
    {
        private StaticCodeAnalysisServiceClient client;
        [TestInitialize]
        public void TestInitialize()
        {
            client = new StaticCodeAnalysisServiceClient();
        }
        [TestMethod]
        public void Given_Valid_Arguments_When_SignUp_Invoked_Then_true_Asserted()
        {
            Assert.IsTrue(client.SignUp("marut", "password"));
            client.DeleteUser("marut", "password");
        }
        [TestMethod]
        public void Given_Invalid_Arguments_When_SignUp_Invoked_Then_false_Asserted()
        {
            Assert.IsTrue(client.SignUp("marut", "password"));
            Assert.IsFalse(client.SignUp("marut", "password"));
            client.DeleteUser("marut", "password");
        }

        [TestMethod]
        public void Given_Invalid_Arguments_When_SignIn_Invoked_Then_false_Asserted()
        {
            Assert.IsFalse(client.SignIn("marut", "password"));
            Assert.IsTrue(client.SignUp("marut", "password"));
            Assert.IsFalse(client.SignIn("maruti", "password"));
            Assert.IsFalse(client.SignIn("marut", "passwordi"));
            client.DeleteUser("marut", "password");
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_SignIn_Invoked_Then_true_Asserted()
        {
            Assert.IsTrue(client.SignUp("marut", "password"));
            Assert.IsTrue(client.SignIn("marut", "password"));
            client.DeleteUser("marut", "password");
        }

        [TestMethod]
        public void Given_Not_Signed_In_When_Download_Invoked_Then_false_Asserted()
        {
            client.SignUp("marut", "password");
            client.SignIn("marut", "password");
            StaticCodeAnalysisServiceClient client2 = new StaticCodeAnalysisServiceClient();
            Assert.IsFalse(client2.Download("https://github.com/phani-rajbn/PhilipsJune2019Dotnet.git", "master"));
            client.DeleteUser("marut", "password");
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_Download_Invoked_Then_true_Asserted()
        {
            client.SignUp("marut", "password");
            client.SignIn("marut", "password");
            Assert.IsTrue(client.Download("https://github.com/phani-rajbn/PhilipsJune2019Dotnet.git","master"));
            client.DeleteUser("marut", "password");
        }

        [TestMethod]
        public void Given_Invalid_Arguments_When_Download_Invoked_Then_false_Asserted()
        {
            client.SignUp("marut", "password");
            client.SignIn("marut", "password");
            Assert.IsFalse(client.Download("https://github.com/phani-rajbn/PhilipsJune2019Dotnet.giti","master"));
            client.DeleteUser("marut", "password");
        }


        [TestMethod]
        public void Given_Valid_Arguments_When_InvokeTool_For_PVS_Invoked_Then_true_Asserted()
        {
            client.SignUp("marut", "password");
            client.SignIn("marut", "password");
            Assert.IsTrue(client.InvokeTool(@"C:\StaticAnalysisData\TestFiles", Tools.PVS));
            Assert.IsNotNull(client.ParseReport(Tools.PVS,@"C:\StaticAnalysisData\TestFiles", "master"));
            client.DeleteUser("marut", "password");
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_InvokeTool_Resharper_Invoked_Then_true_Asserted()
        {
            client.SignUp("marut", "password");
            client.SignIn("marut", "password");
            Assert.IsTrue(client.InvokeTool(@"C:\StaticAnalysisData\TestFiles", Tools.Resharper));
            Assert.IsNotNull(client.ParseReport(Tools.Resharper, @"C:\StaticAnalysisData\TestFiles", "master"));
            client.DeleteUser("marut", "password");
        }


    }
}
