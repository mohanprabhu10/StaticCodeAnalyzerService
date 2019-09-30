using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tools
{
    [DataContract(Name = "Tools")]
    public enum Tools
    {
        [EnumMember(Value = "Resharper")]
        Resharper,
        [EnumMember(Value = "PVS")]
        PVS,
        [EnumMember(Value = "Simian")]
        Simian
    }
}
