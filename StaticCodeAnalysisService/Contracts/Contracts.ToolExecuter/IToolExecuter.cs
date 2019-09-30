using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ToolExecuter
{
    public interface IToolExecuter
    {
        bool ExecuteTool(string userName, string path);
    }
}
