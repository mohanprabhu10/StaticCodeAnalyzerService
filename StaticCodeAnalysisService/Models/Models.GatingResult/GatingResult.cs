using Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.GatingResult
{
    [DataContract]
    public class GatingResult
    {
        [DataMember]
        public string Reository { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public string DateTime { get; set; }
        [DataMember]
        public Tools.Tools Tool{ get; set; }
        [DataMember]
        public int NoOfError { get; set; }
        [DataMember]
        public bool Result { get; set; }
    }
}
