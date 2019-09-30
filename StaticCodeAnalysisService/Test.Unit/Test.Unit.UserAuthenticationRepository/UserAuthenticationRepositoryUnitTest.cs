using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Unit.UserAuthenticationRepository
{
    [TestClass]
    public class UserAuthenticationRepositoryUnitTest
    {
        Contracts.AuthenticationRepository.IAuthenticationRepository authenticationRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            authenticationRepo = new DataAccessLayer.UserAuthenticationRepository.UserAuthenticationRepository(true);
        }

        

        private void SignUpUsers()
        {
            authenticationRepo.SignUp("Admin","pass");
            authenticationRepo.SignUp("User", "key");
        }

        private void DeleteUsers()
        {
            authenticationRepo.Delete("Admin", "pass");
            authenticationRepo.Delete("User", "key");

        }


        [TestMethod]
        public void Given_Valid_Arguments_When_SignUp_Invoked_Then_true_Asserted()
        {
            
            Assert.IsTrue(authenticationRepo.SignUp("Admin", "pass"));
            authenticationRepo.Delete("Admin", "pass");

        }


        [TestMethod]
        public void Given_Invalid_Arguments_When_SignUp_Invoked_Then_false_Asserted()
        {

            SignUpUsers();
            Assert.IsFalse(authenticationRepo.SignUp("Admin", "pass"));
            DeleteUsers();
        }


        [TestMethod]
        public void Given_Valid_Arguments_When_SignIn_Invoked_Then_true_Asserted()
        {
            SignUpUsers();
            Assert.IsTrue(authenticationRepo.SignIn("Admin", "pass"));
            DeleteUsers();

        }


        [TestMethod]
        public void Given_Invalid_UserName_Arguments_When_SignIN_Invoked_Then_false_Asserted()
        {

            SignUpUsers();
            Assert.IsFalse(authenticationRepo.SignIn("AdminInvavlid", "pass"));
            DeleteUsers();
        }

        [TestMethod]
        public void Given_Invalid_Password_Arguments_When_SignIN_Invoked_Then_false_Asserted()
        {

            SignUpUsers();
            Assert.IsFalse(authenticationRepo.SignIn("Admin", "passInvalid"));
            DeleteUsers();
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_Delete_Invoked_Then_true_Asserted()
        {
            SignUpUsers();
            Assert.IsTrue(authenticationRepo.Delete("Admin", "pass"));
            Assert.IsTrue(authenticationRepo.Delete("User", "key"));

        }



        [TestMethod]
        public void Given_Invalid_Arguments_When_Delete_Invoked_Then_false_Asserted()
        {
            Assert.IsFalse(authenticationRepo.Delete("Admin", "pass"));
        }

        [TestMethod]
        public void Given_Valid_Arguments_When_Exists_Invoked_Then_true_Asserted()
        {
            SignUpUsers();
            Assert.IsTrue(authenticationRepo.Exists("Admin"));
            DeleteUsers();
        }

        [TestMethod]
        public void Given_Invalid_Arguments_When_Exists_Invoked_Then_false_Asserted()
        {
            Assert.IsFalse(authenticationRepo.Exists("Admin"));
        }


    }
}
