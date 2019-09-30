using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Tools;

namespace Test.Unit.UserCodeAnalysisRepository
{
    [TestClass]
    public class UserCodeAnalysisRepositoryUnitTest
    {
        Contracts.AuthenticationRepository.IAuthenticationRepository authenticationRepo;
        Contracts.UserCodeAnalysisRepository.IUserCodeAnalysisRepository codeAnalysisRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            authenticationRepo = new DataAccessLayer.UserAuthenticationRepository.UserAuthenticationRepository(true);
            SignUpUsers();

            codeAnalysisRepo = new DataAccessLayer.UserCodeAnalysisRepository.UserCodeAnalysisRepository(true);
        }



        private void SignUpUsers()
        {
            authenticationRepo.SignUp("Admin", "pass");
        }

        [TestCleanup]
        public void DeleteUsers()
        {
            authenticationRepo.Delete("Admin", "pass");
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_Add_Invoked_Then_true_Asserted()
        {

            Assert.IsTrue(codeAnalysisRepo.Add("Admin","github.com", "master",Tools.Resharper,5,-1));
            Assert.IsTrue(codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 4,-1));
        }


        [TestMethod]
        public void Given_Invalid_Arguments_When_Add_Invoked_Then_false_Asserted()
        {
            Assert.IsFalse(codeAnalysisRepo.Add("AdminInvalid", "github.com", "master", Tools.Resharper, 5,-1));
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_Delete_Invoked_Then_true_Asserted()
        {
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 5,-1);
            Assert.IsTrue(codeAnalysisRepo.Delete("Admin", "github.com", "master", Tools.Resharper));

        }



        [TestMethod]
        public void Given_Invalid_Arguments_When_Delete_Invoked_Then_false_Asserted()
        {
            Assert.IsFalse(codeAnalysisRepo.Delete("Admin", "github.com", "master", Tools.Resharper));
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_ReadAllResults_Invoked_Then_valid_Results_Asserted()
        {
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 5,-1);
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 4,-1);
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 2,-1);
            Assert.AreEqual(3,codeAnalysisRepo.ReadAllResults("Admin", "github.com", "master", Tools.Resharper).Count);
            Assert.AreEqual(2, codeAnalysisRepo.ReadAllResults("Admin", "github.com", "master", Tools.Resharper)[0]);
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 5,-1);
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 4,-1);
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 6,-1);
            Assert.AreEqual(5, codeAnalysisRepo.ReadAllResults("Admin", "github.com", "master", Tools.Resharper).Count);
            Assert.AreEqual(6, codeAnalysisRepo.ReadAllResults("Admin", "github.com", "master", Tools.Resharper)[0]);
        }



        [TestMethod]
        public void Given_InValid_Arguments_When_ReadAllResults_Invoked_Then_null_Asserted()
        {
            Assert.AreEqual(0,codeAnalysisRepo.ReadAllResults("AdminInvalid", "github.comInvalid", "master", Tools.Resharper).Count);
        }


        [TestMethod]
        public void Given_Valid_Arguments_When_ReadLatestResult_Invoked_Then_valid_Results_Asserted()
        {
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 5,-1);
            Thread.Sleep(1000);
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper, 4,-1);
            Thread.Sleep(1000);
            codeAnalysisRepo.Add("Admin", "github.com", "master", Tools.Resharper,2,-1);
            Assert.AreEqual(-1, codeAnalysisRepo.ReadLatestResult("Admin", "github.com", "master", Tools.Resharper)[0]);
            Assert.AreEqual(2, codeAnalysisRepo.ReadLatestResult("Admin", "github.com", "master", Tools.Resharper)[1]);
        }



        [TestMethod]
        public void Given_InValid_Arguments_When_ReadLatestResult_Invoked_Then_null_Asserted()
        {
            Assert.AreEqual(0,codeAnalysisRepo.ReadLatestResult("AdminInvalid", "github.comInvalid", "master", Tools.Resharper).Count);
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_ReadRepoList_Invoked_Then_valid_Results_Asserted()
        {
            codeAnalysisRepo.Add("Admin", "Repo1", "master", Tools.Resharper, 5,-1);
            codeAnalysisRepo.Add("Admin", "Repo2", "master", Tools.Resharper, 4,-1);
            codeAnalysisRepo.Add("Admin", "Repo3", "master", Tools.Resharper, 2,-1);
            Assert.AreEqual(3, codeAnalysisRepo.ReadRepoList("Admin").Count);
            codeAnalysisRepo.Add("Admin", "Repo4", "master", Tools.Resharper, 5,-1);
            codeAnalysisRepo.Add("Admin", "Repo5", "master", Tools.Resharper, 4,-1);
            codeAnalysisRepo.Add("Admin", "Repo6", "master", Tools.Resharper, 6,-1);
            codeAnalysisRepo.Add("Admin", "Repo6", "master", Tools.Resharper, 6, -1);
            codeAnalysisRepo.Add("Admin", "Repo6", "master", Tools.Resharper, 6, -1);
            Assert.AreEqual(6, codeAnalysisRepo.ReadRepoList("Admin").Count);
            codeAnalysisRepo.Delete("Admin", "github.com", "master", Tools.Resharper);
        }



        [TestMethod]
        public void Given_InValid_Arguments_When_ReadRepoList_Invoked_Then_null_Asserted()
        {
            Assert.AreEqual(0,codeAnalysisRepo.ReadRepoList("AdminInvalid").Count);
        }



        [TestMethod]
        public void Given_Valid_Arguments_When_UpdateThreshold_Invoked_Then_valid_Results_Asserted()
        {
            codeAnalysisRepo.Add("Admin", "Repo1", "master", Tools.Resharper, 5, -1);
            var latestResult = codeAnalysisRepo.ReadLatestResult("Admin", "Repo1", "master", Tools.Resharper);
            Assert.AreEqual(-1, latestResult[0]);
            Assert.IsTrue(codeAnalysisRepo.UpdateThreshold("Admin","Repo1",Tools.Resharper, "master",  3, 5));
            latestResult = codeAnalysisRepo.ReadLatestResult("Admin", "Repo1", "master", Tools.Resharper);
            Assert.AreEqual(3,latestResult[0]);
          
        }



        [TestMethod]
        public void Given_InValid_Arguments_When_UpdateThreshold_Invoked_Then_False_Asserted()
        {
            Assert.IsFalse(codeAnalysisRepo.UpdateThreshold("AdminInvalid", "Repo1", Tools.Resharper, "master", 3, 5));
        }

    }
}
