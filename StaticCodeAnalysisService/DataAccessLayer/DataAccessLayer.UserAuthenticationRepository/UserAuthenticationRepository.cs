using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.AuthenticationRepository;
using Utility.SqlHelper;

namespace DataAccessLayer.UserAuthenticationRepository
{
    public class UserAuthenticationRepository : IAuthenticationRepository
    {
        private readonly string conString;

        public UserAuthenticationRepository(bool test = false)
        {
            if (test)
                conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StaticCodeAnalysisDB.Test;Integrated Security=True";
            else
                conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StaticCodeAnalysisDB;Integrated Security=True";

        }

        public bool Delete(string userName, string password)
        {
            var query = string.Format("DELETE FROM UserAuthentication WHERE UserName='{0}' AND PasswordKey='{1}'", userName,password);
            return SqlCommands.ExecuteCommand(query, conString);
        }

        public bool Exists(string userName)
        {
            string query = string.Format("Select * FROM UserAuthentication WHERE UserName='{0}'", userName);
            return SqlCommands.Exists(conString, query);
        }

        public bool SignIn(string userName, string password)
        {
            string query = string.Format("Select * FROM UserAuthentication WHERE UserName='{0}' AND PasswordKey='{1}'", userName,password);
            return SqlCommands.Exists(conString, query);
        }

        public bool SignUp(string userName, string password)
        {
            
            var query= string.Format("Insert into UserAuthentication values('{0}','{1}')",userName,password);

            if (SqlCommands.ExecuteCommand(query, conString))
            {

                return true;

            }
            return false;

        }
    }
}
