using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace khVSAutomation
{
    class Television
    {
        public Television(string p_strName, string p_strIP, string p_strMAC)
        {
            TelevisionName = p_strName;
            TelevisionIPAddress = p_strIP;
            TelevisionMACAddress = p_strMAC;
        }
        
        public string TelevisionName { get; set; }
        public string TelevisionIPAddress { get; set; }
        public string TelevisionMACAddress { get; set; }
    }
}
