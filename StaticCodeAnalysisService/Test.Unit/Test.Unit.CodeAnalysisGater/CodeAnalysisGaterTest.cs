using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contracts.AuthenticationRepository;
using Contracts.CodeAnalysisGater;
using Contracts.UserCodeAnalysisRepository;
using CodeAnalysisGater.GaterLib;
using DataAccessLayer.UserCodeAnalysisRepository;
using DataAccessLayer.UserAuthenticationRepository;
using Models.Tools;
using System.Threading;

namespace Test.Unit.CodeAnalysisGater
{
    [TestClass]
    public class CodeAnalysisGaterTest
    {
        IAuthenticationRepository authenticationRepo;
        ICodeAnalysisGater codeAnalysisGater;
        IUserCodeAnalysisRepository userCodeAnalysisRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            authenticationRepo = new DataAccessLayer.UserAuthenticationRepository.UserAuthenticationRepository(true);
            codeAnalysisGater = new CodeGater(true);
            userCodeAnalysisRepo = new DataAccessLayer.UserCodeAnalysisRepository.UserCodeAnalysisRepository(true);
            authenticationRepo.SignUp("Admin", "pass");
            userCodeAnalysisRepo.Add("Admin", "path", "master", Tools.PVS, 20,-1);
            userCodeAnalysisRepo.Add("Admin", "path", "master", Tools.PVS, 10,-1);


        }

        [TestMethod]
        public void Given_Existing_ParsedReport_With_Greater_Thresold_When_AbsoluteGate_Invoked_Then_True_Asserted()
        {
            
            Assert.IsTrue(codeAnalysisGater.AbsoluteGate(30, "Admin", Tools.PVS, "path", "master"));
            
        }

        [TestMethod]
        public void Given_Existing_ParsedReport_With_Less_Thresold_When_AbsoluteGate_Invoked_Then_false_Asserted()
        {

            Assert.IsFalse(codeAnalysisGater.AbsoluteGate(5, "Admin", Tools.PVS, "path", "master"));

        }


        [TestMethod]
        public void Given_NoN_Existing_ParsedReport_With_Less_Thresold_When_AbsoluteGate_Invoked_Then_false_Asserted()
        {

            Assert.IsFalse(codeAnalysisGater.AbsoluteGate(5, "AdminInvalid", Tools.PVS, "path", "master"));

        }


        [TestMethod]
        public void Given_Existing_ParsedReport_With_Greater_Thresold_When_RelativeGate_Invoked_Then_True_Asserted()
        {

            Assert.IsTrue(codeAnalysisGater.RelativeGate("Admin", Tools.PVS, "path", "master"));

        }

        [TestMethod]
        public void Given_Existing_ParsedReport_With_Less_Thresold_When_RelativeGate_Invoked_Then_false_Asserted()
        {
            Thread.Sleep(1000);
            userCodeAnalysisRepo.Add("Admin", "path", "master", Tools.PVS, 5,-1);
            Thread.Sleep(1000);
            userCodeAnalysisRepo.Add("Admin", "path", "master", Tools.PVS, 15, -1);
            var boolcond = codeAnalysisGater.RelativeGate("Admin", Tools.PVS, "path", "master");
            Assert.IsFalse(boolcond);

        }


        [TestMethod]
        public void Given_NoN_Existing_ParsedReport_With_Less_Thresold_When_RelativeGate_Invoked_Then_false_Asserted()
        {
            
            Assert.IsFalse(codeAnalysisGater.RelativeGate("AdminInvalid", Tools.PVS, "path", "master"));

        }




        [TestCleanup]
        public void DeleteUsers()
        {
            authenticationRepo.Delete("Admin","pass");
        }
    }
}
