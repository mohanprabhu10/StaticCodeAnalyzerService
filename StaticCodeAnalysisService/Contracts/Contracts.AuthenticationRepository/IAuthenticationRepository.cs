using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.AuthenticationRepository
{
    public interface IAuthenticationRepository
    {
        bool SignIn(string userName,string password);
        bool SignUp(string userName, string password);
        bool Exists(string userName);
        bool Delete(string userName, string password);
    }
}
