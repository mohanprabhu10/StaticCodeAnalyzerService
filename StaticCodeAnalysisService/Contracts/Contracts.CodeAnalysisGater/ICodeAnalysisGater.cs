using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Tools;

namespace Contracts.CodeAnalysisGater
{
    public interface ICodeAnalysisGater
    {
        bool AbsoluteGate(int threshold, string userName, Tools tool, string repository, string branch);
        bool RelativeGate(string userName, Tools tool, string repository, string branch);
    }
}
