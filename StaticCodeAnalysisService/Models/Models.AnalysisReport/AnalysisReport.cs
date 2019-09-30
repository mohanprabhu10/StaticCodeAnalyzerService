using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.AnalysisReport
{
    [DataContract]
    public class AnalysisReport
    {
        [DataMember]
        public string TypeId { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public int Line { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
